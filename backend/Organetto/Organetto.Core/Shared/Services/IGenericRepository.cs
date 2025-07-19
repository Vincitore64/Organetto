namespace Organetto.Core.Shared.Services
{
    public interface IGenericRepository<TEntity, in TKey> :
        IReadByIdAndUpdateRepository<TEntity, TKey>,
        IReadByIdAndDeleteRepository<TEntity, TKey>,
        ICreateRepository<TEntity>
    {
    }
}
