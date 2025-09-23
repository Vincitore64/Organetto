namespace Organetto.Core.Shared.Services
{
    /// <summary>
    /// Defines a read‐only lookup by ID for any aggregate.
    /// </summary>
    public interface IReadByIdRepository<TEntity, in TKey>
    {
        /// <summary>
        /// Fetches the entity of type TEntity by its key, or throws if not found.
        /// </summary>
        Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    }
}
