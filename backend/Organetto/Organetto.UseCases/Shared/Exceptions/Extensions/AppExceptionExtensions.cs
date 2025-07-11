using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organetto.UseCases.Shared.Exceptions.Models;

namespace Organetto.UseCases.Shared.Exceptions.Extensions
{
    public static class AppExceptionExtensions
    {
        public static ProblemDetails ToProblemDetails(this AppException ex, HttpContext httpContext)
        {
            var problem = new ProblemDetails
            {
                Status = ex.Status,
                Title = ex.Title,
                Type = ex.Type,
                Detail = ex.FullMessage(),
                Instance = ex.Instance ?? httpContext.Request.Path
            };

            problem.Extensions["code"] = ex.Code;
            if (ex.Errors?.Count > 0)
                problem.Extensions["errors"] = ex.Errors;

            return problem;
        }
    }
}
