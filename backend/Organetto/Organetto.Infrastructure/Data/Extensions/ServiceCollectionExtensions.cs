using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Users.Services;
using Organetto.Infrastructure.Data.Boards.Services;
using Organetto.Infrastructure.Data.Outbox;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Data.Users.Services;
using Organetto.UseCases.Shared.Outbox.Services;

namespace Organetto.Infrastructure.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>((options) =>
            {
                options.UseNpgsql(connectionString)
                    .UseExceptionProcessor()
                    .UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOutboxService, OutboxService>();

            return services;
        }

        public static void MigrateApplicationDb(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                serviceScope?.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }
        }
    }
}
