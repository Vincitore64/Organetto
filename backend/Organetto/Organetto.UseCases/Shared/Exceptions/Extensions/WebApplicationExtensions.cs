using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organetto.UseCases.Shared.Exceptions.Middleware;
using Organetto.UseCases.Shared.Exceptions.Models;

namespace Organetto.UseCases.Shared.Exceptions.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<AppExceptionHandler>();
        }

        public static void UseApiExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errApp =>
            {
                errApp.Run(async context =>
                {
                    // 1) Очищаем всё, что могло настрогать до этого
                    context.Response.Clear();
                    // 2) Указываем правильный Content-Type
                    context.Response.ContentType = "application/problem+json";

                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (ex is FluentValidation.ValidationException fv)
                    {
                        // build the errors dictionary once
                        var errors = fv.Errors
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
                        await context.Response.WriteAsJsonAsync(fluentValidationProblemDetails);
                        return;
                    }
                    if (ex is AppException apiEx)
                    {
                        context.Response.StatusCode = apiEx.Status;
                        await context.Response.WriteAsJsonAsync(apiEx.ToProblemDetails(context));
                    }
                    else
                    {
                        // fallback for non-ApiException
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        var pd = new ProblemDetails
                        {
                            Status = 500,
                            Title = "Internal Server Error",
                            Type = "https://docs.myapi.com/problems/ServerError",
                            Detail = ex?.Message
                        };
                        await context.Response.WriteAsJsonAsync(pd);
                    }
                });
            });
        }
    }
}
