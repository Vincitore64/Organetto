using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organetto.Infrastructure.Infrastructure.Authentication.Extensions;

namespace Organetto.Infrastructure.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFirebaseAuthentication(configuration);
            return services;
        }
    }
}
