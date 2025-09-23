namespace Organetto.Core.Shared.IO.Services
{
    public interface IStreamHasher
    {
        Task<string> ComputeSha256Base64Async(Stream s, CancellationToken ct);
    }
}
