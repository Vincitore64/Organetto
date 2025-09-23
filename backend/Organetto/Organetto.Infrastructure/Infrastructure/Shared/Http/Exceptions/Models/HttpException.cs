using Organetto.UseCases.Shared.Exceptions.Models;
using System.Net;

namespace Organetto.Infrastructure.Infrastructure.Shared.Http.Exceptions.Models
{
    /// <summary>
    /// Exception thrown when HTTP-related errors occur.
    /// </summary>
    public class HttpException : AppException
    {
        public HttpStatusCode HttpStatusCode { get; }

        public HttpException(
            string message,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
            Exception? innerException = null,
            IDictionary<string, string[]>? errors = null,
            string? instance = null
        ) : base(
            status: (int)httpStatusCode,
            title: GetTitleFromStatusCode(httpStatusCode),
            code: GetErrorCodeFromStatusCode(httpStatusCode),
            message: message,
            errors: errors,
            instance: instance
        )
        {
            HttpStatusCode = httpStatusCode;
            if (innerException != null)
            {
                // Store inner exception in Data dictionary since AppException doesn't expose InnerException
                Data["InnerException"] = innerException;
            }
        }

        public HttpException(
            string message,
            Exception innerException,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
            IDictionary<string, string[]>? errors = null,
            string? instance = null
        ) : this(message, httpStatusCode, innerException, errors, instance)
        {
        }

        private static string GetTitleFromStatusCode(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "Bad Request",
                HttpStatusCode.Unauthorized => "Unauthorized",
                HttpStatusCode.Forbidden => "Forbidden",
                HttpStatusCode.NotFound => "Not Found",
                HttpStatusCode.Conflict => "Conflict",
                HttpStatusCode.UnprocessableEntity => "Unprocessable Entity",
                HttpStatusCode.InternalServerError => "Internal Server Error",
                HttpStatusCode.BadGateway => "Bad Gateway",
                HttpStatusCode.ServiceUnavailable => "Service Unavailable",
                HttpStatusCode.GatewayTimeout => "Gateway Timeout",
                _ => "HTTP Error"
            };
        }

        private static string GetErrorCodeFromStatusCode(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => AppErrorCode.INVALID_REQUEST.ToString(),
                HttpStatusCode.Unauthorized => AppErrorCode.INVALID_TOKEN.ToString(),
                HttpStatusCode.Forbidden => AppErrorCode.INSUFFICIENT_SCOPE.ToString(),
                HttpStatusCode.NotFound => AppErrorCode.ENTITY_NOT_FOUND.ToString(),
                HttpStatusCode.UnprocessableEntity => AppErrorCode.VALIDATION_FAILED.ToString(),
                HttpStatusCode.ServiceUnavailable => AppErrorCode.EXTERNAL_PROVIDER_ERROR.ToString(),
                HttpStatusCode.InternalServerError => AppErrorCode.UNKNOWN_ERROR.ToString(),
                _ => AppErrorCode.UNKNOWN_ERROR.ToString()
            };
        }
    }
}