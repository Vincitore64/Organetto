namespace Organetto.Core.Shared.Services
{
    public interface IUpdateRepository<TEntity>
    {
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
