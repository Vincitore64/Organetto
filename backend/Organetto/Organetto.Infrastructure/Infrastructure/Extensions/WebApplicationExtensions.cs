using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Organetto.Infrastructure.Data.Extensions;

namespace Organetto.Infrastructure.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseInfrastructure(this WebApplication app, IWebHostEnvironment webHostEnvironment)
        {
            if (!webHostEnvironment.IsDevelopment())
            {
                app.MigrateApplicationDb();
            }
        }
    }
}
