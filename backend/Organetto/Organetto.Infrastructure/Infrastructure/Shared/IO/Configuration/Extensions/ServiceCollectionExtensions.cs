using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organetto.Core.Shared.IO.Services;
using Organetto.Infrastructure.Infrastructure.Shared.IO.Services;

namespace Organetto.Infrastructure.Infrastructure.Shared.IO.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedIOServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IStreamHasher, StreamHasher>();

            return services;
        }
    }
}
