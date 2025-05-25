using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Models;

namespace Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Extensions
{
    public static class ApiExceptionExtensions
    {
        public static ProblemDetails ToProblemDetails(this ApiException ex, HttpContext httpContext)
        {
            var problem = new ProblemDetails
            {
                Status = ex.Status,
                Title = ex.Title,
                Type = ex.Type,
                Detail = ex.Message,
                Instance = ex.Instance ?? httpContext.Request.Path
            };

            problem.Extensions["code"] = ex.Code;
            if (ex.Errors?.Count > 0)
                problem.Extensions["errors"] = ex.Errors;

            return problem;
        }
    }
}
