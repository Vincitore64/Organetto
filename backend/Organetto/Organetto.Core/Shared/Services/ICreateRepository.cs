namespace Organetto.Core.Shared.Services
{
    // Repository abstractions for use by the handlers
    public interface ICreateRepository<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
