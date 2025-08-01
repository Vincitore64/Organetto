﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Organetto.UseCases.Boards.IntegrationEvents;
using Organetto.UseCases.Shared.MassTransit.Services;
using System.Reflection;

namespace Organetto.UseCases.Shared.MassTransit.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {

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

                //busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());        // Локальные задачи
                busConfigurator.AddConsumer<SignalRBoardCreatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRBoardDeletedIntegrationEventConsumer>();

                busConfigurator.UsingInMemory((context, configuration) =>
                {
                    //configuration.ConfigureEndpoints(context);
                    configuration.ReceiveEndpoint("signalr-endpoints", (endpontCfg) =>
                    {
                        endpontCfg.ConfigureConsumer<SignalRBoardCreatedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<SignalRBoardDeletedIntegrationEventConsumer>(context);

                    });
                });

            });
        }
    }

}
