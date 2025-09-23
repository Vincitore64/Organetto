using Organetto.Core.Shared.IO.Services;
using System.Buffers;
using System.Security.Cryptography;

namespace Organetto.Infrastructure.Infrastructure.Shared.IO.Services
{
    public class StreamHasher : IStreamHasher
    {
        public async Task<string> ComputeSha256Base64Async(Stream s, CancellationToken ct)
        {
            using var sha = SHA256.Create();

            var pool = ArrayPool<byte>.Shared;
            var buffer = pool.Rent(81920);

            try
            {
                int read;
                while ((read = await s.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
                    sha.TransformBlock(buffer, 0, read, null, 0);
                sha.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                return Convert.ToBase64String(sha.Hash!);
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
