using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Organetto.UseCases.Shared.Exceptions.Extensions;
using Organetto.UseCases.Shared.Exceptions.Models;

namespace Organetto.UseCases.Shared.Exceptions.Middleware
{
    public class AppExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger _log;

        public AppExceptionHandler(RequestDelegate next, ILogger<AppExceptionHandler> log)
        {
            this.next = next;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext context /* other dependencies */ )
        {
            try
            {
                await next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _log?.LogError("AppExceptionHandler. Unhandled ValidationException.");
                _log?.LogError(ex, ex.FullMessage());
                // build the errors dictionary once
                var errors = ex.Errors
                               .GroupBy(e => e.PropertyName)
                               .ToDictionary(g => g.Key,
                                             g => g.Select(e => e.ErrorMessage).ToArray());
                // TODO: Add codes from FluentValidation ValidationFailure
                var fluentValidationProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    Type = "https://docs.myapi.com/problems/ValidationError",
                    Detail = "Validation failed.",
                    Instance = context.Request.Path,
                    Extensions =
                    {
                        ["errors"] = errors,
                        ["code"] = "ERR_VALIDATION"
                    }
                };
                context.Response.StatusCode = fluentValidationProblemDetails.Status.Value;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(fluentValidationProblemDetails);
            }
            catch (AppException apiEx)
            {
                context.Response.StatusCode = apiEx.Status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(apiEx.ToProblemDetails(context));
            }
            catch (Exception ex)
            {
                _log?.LogError("AppExceptionHandler. Unhandled exception.");
                _log?.LogError(ex, ex.FullMessage());
                // fallback for non-ApiException
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var pd = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Type = "https://docs.myapi.com/problems/ServerError",
                    Detail = ex?.FullMessage()
                };
                await context.Response.WriteAsJsonAsync(pd);
            }
        }
    }
}
