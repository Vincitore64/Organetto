using Organetto.Core.Shared.FileStorage.Models;

namespace Organetto.Core.Shared.FileStorage.Services
{
    public interface IObjectStoragePort
    {
        Task PutAsync(IStorageKey key, Stream content, IContentType contentType, CancellationToken ct);
        Task<Stream> GetAsync(IStorageKey key, CancellationToken ct);
        Task DeleteAsync(IStorageKey key, CancellationToken ct);
    }
}
