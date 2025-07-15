using Microsoft.Extensions.DependencyInjection;

namespace Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers.Configuration.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddEventsMapper(this IServiceCollection services)
        {
            services.AddTransient<IEventsMapper, ConventionEventsMapper>();
        }
    }
}
