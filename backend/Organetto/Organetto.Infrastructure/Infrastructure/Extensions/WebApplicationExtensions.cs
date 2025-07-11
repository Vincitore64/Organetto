using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Organetto.Infrastructure.Data.Extensions;
using Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Extensions;

namespace Organetto.Infrastructure.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseInfrastructurePipelines(this WebApplication app, IWebHostEnvironment webHostEnvironment)
        {
            app.UseApiExceptionHandler();
            if (!webHostEnvironment.IsDevelopment())
            {
                app.MigrateApplicationDb();
            }
        }
    }
}
