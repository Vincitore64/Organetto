using Microsoft.EntityFrameworkCore;
using Organetto.Core.Shared.Models;
using Organetto.Core.Shared.Services;
using Organetto.Infrastructure.Data.Shared.Exceptions;
using System.Linq.Expressions;

namespace Organetto.Infrastructure.Data.Shared.Services
{
    /// <summary>
    /// Базовый generic-репозиторий, реализующий CRUD и переключение tracking.
    /// 
    /// ⚠️ ВАЖНО:
    /// Параметр TConcreteRepository ожидает конкретный репозиторий (класс или интерфейс, который реализует наследник)!
    /// Если вы попробуете использовать структуру или интерфейс, который не реализуется наследником, в качестве TConcreteRepository —
    /// вы _idiot_ и потоком ошибок утонете быстрее, чем успеете сказать “AsNoTracking()”.
    /// </summary>
    /// <typeparam name="TConcreteRepository">
    /// Тип конкретного репозитория, который наследует этот generic-базис.
    /// </typeparam>
    public abstract class EfCoreGenericRepository<TConcreteRepository, TEntity, TKey> : IGenericRepository<TEntity, TKey>, ITrackingRepository<TConcreteRepository>
        where TConcreteRepository : IGenericRepository<TEntity, TKey>
        where TEntity : class, IHasId<TKey>
    {
        private readonly ApplicationDbContext _db;

        public EfCoreGenericRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
            HasTracking = true;
        }

        /// <summary>
        /// Флаг, отвечающий за то, будет ли EF Core отслеживать изменения (AsTracking) или нет.
        /// </summary>
        protected bool HasTracking { get; private set; }

        /// <summary>
        /// Возвращает IQueryable с нужным tracking в зависимости от <see cref="HasTracking"/>.
        /// </summary>
        protected IQueryable<T> Query<T>(IQueryable<T> query)
            where T : class
        {
            return Query(query, HasTracking);
        }

        protected IQueryable<T> Query<T>(IQueryable<T> query, bool asTracking)
            where T : class
        {
            return asTracking
                ? query.AsTracking()
                : query.AsNoTracking();
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = await _db.Set<TEntity>().AddAsync(entity, cancellationToken);
            return entry.Entity;
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken)
        {
            var lambda = GetByIdExpression(id);
            var item = await _db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(lambda, cancellationToken);
            if (item == null) return;

            _db.Set<TEntity>().Remove(item);
        }

        private Expression<Func<TEntity, bool>> GetByIdExpression(TKey id)
        {
            // Построим выражение e => e.Id == id
            var param = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(param, nameof(IHasId<TKey>.Id));
            var constant = Expression.Constant(id, typeof(TKey));
            var equality = Expression.Equal(property, constant);
            Expression<Func<TEntity, bool>> expression = Expression.Lambda<Func<TEntity, bool>>(equality, param);
            return expression;
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var lambda = GetByIdExpression(id);
            return await Query(_db.Set<TEntity>()).FirstOrDefaultAsync(lambda, cancellationToken) ??
                throw new EntityNotFoundException(404, nameof(TEntity));
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _db.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Отключить tracking для всех последующих запросов.
        /// </summary>
        /// <remarks>
        /// Если вы передадите в TConcreteRepository структуру или интерфейс, который не реализуется наследником — прочитайте
        /// <see cref="EfCoreGenericRepository{TConcreteRepository}"/> предупреждение. 
        /// </remarks>
        public TConcreteRepository WithoutTracking()
        {
            HasTracking = false;
            return this as dynamic; // This is so cheaaaaaaatsssss =)
        }

        /// <summary>
        /// Включить tracking для всех последующих запросов.
        /// </summary>
        /// <remarks>
        /// Если вы передадите в TConcreteRepository структуру или интерфейс, который не реализуется наследником — прочитайте
        /// <see cref="EfCoreGenericRepository{TConcreteRepository}"/> предупреждение. 
        /// </remarks>
        public TConcreteRepository WithTracking()
        {
            HasTracking = true;
            return this as dynamic;  // Это читерство: если TRepo не ваш конкретный класс — страхов нет.
        }
    }
}
