using Microsoft.Extensions.DependencyInjection;
using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Services;
using Organetto.UseCases.Shared.IntegrationEvents.Services;

namespace Organetto.Infrastructure.Infrastructure.IntegrationEvents.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            services.AddTransient<IIntegrationEventBus, MassTransitIntegrationEventBus>();
        }

        //public static void AddMassTransitEventSourcing(this IServiceCollection services)
        //{
        //    services.AddMassTransit(busConfigurator =>
        //    {
        //        busConfigurator.SetKebabCaseEndpointNameFormatter();

        //        busConfigurator.UsingInMemory((context, configuration) => configuration.ConfigureEndpoints(context));
        //    });

        //    services.AddTransient<IIntegrationEventBus, MassTransitIntegrationEventBus>();
        //}
    }
}
