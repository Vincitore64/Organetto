using Amazon.S3;
using Amazon.S3.Model;
using Organetto.Core.Shared.FileStorage.Models;
using Organetto.Core.Shared.FileStorage.Services;
using Organetto.Infrastructure.Infrastructure.AWS.Exceptions;

namespace Organetto.Infrastructure.Infrastructure.AWS.Services
{
    public class S3ObjectStorage : IObjectStoragePort
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3ObjectStorage(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
        }

        public async Task PutAsync(IStorageKey key, Stream content, IContentType contentType, CancellationToken ct)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key.Value,
                InputStream = content,
                ContentType = contentType.Value,
                AutoCloseStream = false,
                //ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
            };
            try
            {
                var response = await _s3Client.PutObjectAsync(request, ct);
                // Возвращаем URL объекта
                //return new Uri($"{_s3Client.Config.ServiceURL}/{_bucketName}/{key}");
            }
            catch (AmazonS3Exception ex)
            {
                throw StorageException.FromAmazonS3Exception(ex);
            }
        }

        public async Task<Stream> GetAsync(IStorageKey key, CancellationToken ct)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key.Value
            };

            try
            {
                var response = await _s3Client.GetObjectAsync(request, ct);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception ex)
            {
                throw StorageException.FromAmazonS3Exception(ex);
            }
        }

        public async Task DeleteAsync(IStorageKey key, CancellationToken ct)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key.Value
            };

            try
            {
                await _s3Client.DeleteObjectAsync(request, ct);
            }
            catch (AmazonS3Exception ex)
            {
                throw StorageException.FromAmazonS3Exception(ex);
            }
        }
    }
}
