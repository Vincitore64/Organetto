using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Organetto.UseCases.Shared.Exceptions.Models;

namespace Organetto.Infrastructure.Infrastructure.AWS.Exceptions
{
    /// <summary>
    /// Exception thrown when storage operations fail.
    /// </summary>
    public class StorageException : AppException
    {
        public StorageException(
            string message,
            string code = nameof(AppErrorCode.STORAGE_ERROR),
            int status = StatusCodes.Status500InternalServerError,
            IDictionary<string, string[]>? errors = null,
            string? instance = null,
            Exception? innerException = null
        ) : base(
            status: status,
            title: "Storage Operation Failed",
            code: code,
            message: message,
            errors: errors,
            instance: instance
        )
        {
        }

        /// <summary>
        /// Creates a StorageException for when an object is not found.
        /// </summary>
        /// <param name="key">The key of the object that was not found</param>
        /// <param name="instance">The request instance path</param>
        /// <returns>A new StorageException</returns>
        public static StorageException ObjectNotFound(string key, string? instance = null)
        {
            return new StorageException(
                message: $"Object with key '{key}' was not found",
                code: AppErrorCode.OBJECT_NOT_FOUND.ToString(),
                status: StatusCodes.Status404NotFound,
                instance: instance
            );
        }

        /// <summary>
        /// Creates a StorageException from an AmazonS3Exception.
        /// </summary>
        /// <param name="ex">The AmazonS3Exception that was thrown</param>
        /// <param name="instance">The request instance path</param>
        /// <returns>A new StorageException</returns>
        public static StorageException FromAmazonS3Exception(AmazonS3Exception ex, string? instance = null)
        {
            // Determine if this is a "not found" error
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new StorageException(
                    message: $"The requested object was not found",
                    code: nameof(AppErrorCode.OBJECT_NOT_FOUND),
                    status: StatusCodes.Status404NotFound,
                    instance: instance,
                    innerException: ex
                );
            }

            // For access denied errors
            if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return new StorageException(
                    message: $"Access to the requested object is forbidden",
                    status: StatusCodes.Status403Forbidden,
                    code: ex.ErrorCode,
                    instance: instance,
                    innerException: ex
                );
            }

            // Default case - general storage error
            return new StorageException(
                message: $"S3 storage operation failed: {ex.Message}",
                code: ex.ErrorCode,
                instance: instance,
                innerException: ex
            );
        }

        ///// <summary>
        ///// Creates a StorageException from a YandexApiException.
        ///// </summary>
        ///// <param name="ex">The YandexApiException that was thrown</param>
        ///// <param name="instance">The request instance path</param>
        ///// <returns>A new StorageException</returns>
        //public static StorageException FromYandexApiException(YandexApiException ex, string? instance = null)
        //{
        //    // Determine if this is a "not found" error
        //    if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //    {
        //        return new StorageException(
        //            message: $"The requested object was not found",
        //            code: nameof(AppErrorCode.OBJECT_NOT_FOUND),
        //            status: StatusCodes.Status404NotFound,
        //            instance: instance,
        //            innerException: ex
        //        );
        //    }

        //    // For access denied errors
        //    if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden || ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //    {
        //        return new StorageException(
        //            message: $"Access to the requested object is forbidden",
        //            status: StatusCodes.Status403Forbidden,
        //            code: ex.Error.Error ?? "YANDEX_ACCESS_DENIED",
        //            instance: instance,
        //            innerException: ex
        //        );
        //    }

        //    // Default case - general storage error
        //    return new StorageException(
        //        message: $"YandexDisk storage operation failed: {ex.Message}",
        //        code: ex.Error.Error ?? "YANDEX_STORAGE_ERROR",
        //        instance: instance,
        //        innerException: ex
        //    );
        //}
    }

}
