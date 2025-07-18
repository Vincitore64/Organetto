using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Organetto.UseCases.Boards.Hubs;
using Organetto.UseCases.Boards.Services;
using Organetto.UseCases.Shared.MassTransit.Configuration.Extensions;
using System.Reflection;

namespace Organetto.UseCases.Configuration.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(BoardMappingProfile).Assembly);
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
                o.MaximumReceiveMessageSize = 1024 * 32;      // 32 KB
            });
            services.AddMassTransitEventSourcing();
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));

            return services;
        }

        public static void UseApplicationHubs(this WebApplication app)
        {
            app.MapHub<BoardHub>("/hubs/boards");
        }
    }

}
