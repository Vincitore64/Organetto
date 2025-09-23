namespace Organetto.Core.Shared.Services
{
    public interface IReadByIdAndUpdateRepository<TEntity, in TKey> : IReadByIdRepository<TEntity, TKey>, IUpdateRepository<TEntity>
    {
    }
}
