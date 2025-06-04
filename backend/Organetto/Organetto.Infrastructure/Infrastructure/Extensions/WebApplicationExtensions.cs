using Microsoft.AspNetCore.Builder;
using Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Extensions;

namespace Organetto.Infrastructure.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseInfrastructureServices(this WebApplication app)
        {
            app.UseApiExceptionHandler();
        }
    }
}
