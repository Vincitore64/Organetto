using Microsoft.AspNetCore.Http;

namespace Organetto.UseCases.Shared.Exceptions.Models
{
    /// <summary>
    /// Exception thrown when validation fails.
    /// </summary>
    public class AppValidationException : AppException
    {
        public AppValidationException(
            string message,
            IDictionary<string, string[]>? errors = null,
            string? instance = null
        ) : base(
            status: StatusCodes.Status400BadRequest,
            title: "Validation Failed",
            code: AppErrorCode.VALIDATION_FAILED,
            message: message,
            errors: errors,
            instance: instance
        )
        {
        }

        /// <summary>
        /// Creates a ValidationException from a single error message.
        /// </summary>
        /// <param name="message">The validation error message</param>
        /// <param name="instance">The request instance path</param>
        /// <returns>A new ValidationException</returns>
        public static AppValidationException FromMessage(string message, string? instance = null)
        {
            return new AppValidationException(message, instance: instance);
        }

        /// <summary>
        /// Creates a ValidationException from multiple validation errors.
        /// </summary>
        /// <param name="errors">Dictionary of field names and their error messages</param>
        /// <param name="instance">The request instance path</param>
        /// <returns>A new ValidationException</returns>
        public static AppValidationException FromErrors(IDictionary<string, string[]> errors, string? instance = null)
        {
            var message = string.Join("\n", errors.SelectMany(kvp => kvp.Value));
            return new AppValidationException(message, errors, instance);
        }
    }
}