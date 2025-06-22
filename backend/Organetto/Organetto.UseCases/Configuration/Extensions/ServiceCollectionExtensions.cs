using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Services;
using Organetto.Infrastructure.Infrastructure.MassTransit.Services;
using Organetto.UseCases.Boards.Hubs;
using Organetto.UseCases.Boards.Services;
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

        public static void AddMassTransitEventSourcing(this IServiceCollection services)
        {
            //services.AddMassTransit(busConfigurator =>
            //{
            //    busConfigurator.SetKebabCaseEndpointNameFormatter();

            //    //busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());

            //    //busConfigurator.UsingInMemory((context, configuration) => configuration.ConfigureEndpoints(context));
            //});

            //services.AddMassTransit<IRabbitBus>(busConfigurator =>
            //{
            //    busConfigurator.SetKebabCaseEndpointNameFormatter();

            //    busConfigurator.AddConsumer<DomainEventConsumer>();     // Только rabbit-консьюмеры

            //    busConfigurator.UsingRabbitMq((ctx, cfg) =>
            //    {
            //        cfg.Host("rabbitmq:5672", h =>
            //        {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });

            //        cfg.ConfigureEndpoints(ctx);               // Автосоздание очередей
            //    });
            //});

            //// ---------- 2.2  Kafka Bus ----------
            //services.AddMassTransit<IKafkaBus>(kafka =>
            //{
            //    kafka.AddConsumer<AnalyticsConsumer>();        // Только kafka-консьюмеры

            //    kafka.UsingInMemory((ctx, cfg) =>              // Kafka использует Rider
            //    {
            //        cfg.ConfigureKafka(k =>
            //        {
            //            k.Host("kafka:9092");
            //        });

            //        cfg.ConfigureEndpoints(ctx);
            //    });
            //});

            // ---------- 2.3  In-Memory Bus ----------
            services.AddMassTransit<IInMemoryBus>(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());        // Локальные задачи

                busConfigurator.UsingInMemory((context, configuration) => configuration.ConfigureEndpoints(context));
            });

            services.AddTransient<IIntegrationEventBus, MassTransitIntegrationEventBus>();
        }

        public static void UseApplicationServices(this WebApplication app)
        {
            app.MapHub<BoardHub>("/hubs/boards");
        }
    }

}
