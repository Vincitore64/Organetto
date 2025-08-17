using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organetto.Infrastructure.Infrastructure.Authentication.Extensions;
using Organetto.Infrastructure.Infrastructure.AWS.Configuration.Extensions;
using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Configuration.Extensions;
using Organetto.Infrastructure.Infrastructure.Outbox.Services;
using Organetto.Infrastructure.Infrastructure.Shared.IO.Configuration.Extensions;

namespace Organetto.Infrastructure.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFirebaseAuthentication(configuration);
            services.AddHostedService<OutboxProcessor>();
            services.AddEventBus();
            services.AddAWS(configuration);
            services.AddSharedIOServices(configuration);
            return services;
        }
    }
}
