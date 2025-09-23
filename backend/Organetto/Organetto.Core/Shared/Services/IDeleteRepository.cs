namespace Organetto.Core.Shared.Services
{
    public interface IDeleteRepository<in TKey>
    {
        Task DeleteAsync(TKey id, CancellationToken cancellationToken);
    }
}
