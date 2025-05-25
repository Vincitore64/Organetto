using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Models;

namespace Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseApiExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(errApp =>
            {
                errApp.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (ex is ApiException apiEx)
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
