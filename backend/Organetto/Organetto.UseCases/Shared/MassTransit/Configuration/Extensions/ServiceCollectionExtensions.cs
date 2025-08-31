using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Organetto.UseCases.Boards.Columns.Cards.IntegrationEvents;
using Organetto.UseCases.Boards.Columns.IntergationEvents;
using Organetto.UseCases.Boards.IntegrationEvents;
using Organetto.UseCases.Shared.MassTransit.Configuration.Models;
using Organetto.UseCases.Shared.MassTransit.Services;

namespace Organetto.UseCases.Shared.MassTransit.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddMassTransitEventSourcing(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<RabbitMqOptions>()
                .Bind(configuration.GetSection("RabbitMQ"))
                .ValidateDataAnnotations()
                .Validate(o => !string.IsNullOrWhiteSpace(o.Host), "RabbitMQ:Host is required")
                .Validate(o => !string.IsNullOrWhiteSpace(o.Username), "RabbitMQ:Username is required")
                .Validate(o => !string.IsNullOrWhiteSpace(o.Password), "RabbitMQ:Password is required")
                .ValidateOnStart();

            services.AddOptions<MassTransitOptions>()
                .Bind(configuration.GetSection("MassTransit"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            //services.AddMassTransit(busConfigurator =>
            //{
            //    busConfigurator.SetKebabCaseEndpointNameFormatter();

            //    //busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());

            //    //busConfigurator.UsingInMemory((context, configuration) => configuration.ConfigureEndpoints(context));
            //});

            services.AddMassTransit<IRabbitBus>(busConfigurator =>
            {

                //var sp = services.BuildServiceProvider();
                //var mt = sp.GetRequiredService<IOptions<MassTransitOptions>>().Value;

                //if (mt.KebabCaseEndpointNames)
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<SignalRBoardCreatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRBoardDeletedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRBoardMetadataUpdatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRColumnCreatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRColumnUpdatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRColumnDeletedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<CardCreatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<CardUpdatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<CardDeletedIntegrationEventConsumer>();

                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    var opt = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                    var mtOpt = context.GetRequiredService<IOptions<MassTransitOptions>>().Value;

                    cfg.Host(opt.Host, (ushort)opt.Port, opt.VirtualHost, h =>
                    {
                        h.Username(opt.Username);
                        h.Password(opt.Password);

                    });

                    // Scheduler choice
                    if (string.Equals(opt.Scheduler, "DelayedExchange", StringComparison.OrdinalIgnoreCase))
                    {
                        // Требует включённый plugin rabbitmq_delayed_message_exchange
                        cfg.UseDelayedMessageScheduler();
                    }

                    // Глобальный префетч (можно и на endpoint)
                    cfg.PrefetchCount = opt.PrefetchCount;

                    // Единый receive endpoint
                    cfg.ReceiveEndpoint(mtOpt.SignalrEndpointName, e =>
                    {
                        // Лимит конкурентности по желанию
                        if (opt.ConcurrentMessageLimit > 0)
                            e.ConcurrentMessageLimit = opt.ConcurrentMessageLimit;

                        // Немедленные ретраи (in-memory, без requeue)
                        if (opt.Retry.Immediate > 0)
                        {
                            e.UseMessageRetry(r => r.Immediate(opt.Retry.Immediate));
                        }

                        // Задержанные редоставки
                        if (opt.Retry.DelayedIntervals.Length > 0)
                        {
                            var intervals = opt.Retry.DelayedIntervals
                                .Select(TimeSpan.Parse)
                                .ToArray();

                            e.UseScheduledRedelivery(r => r.Intervals(intervals));
                        }

                        // Регистрируем все consumers на endpoint
                        e.ConfigureConsumer<SignalRBoardCreatedIntegrationEventConsumer>(context);
                        e.ConfigureConsumer<SignalRBoardDeletedIntegrationEventConsumer>(context);
                        e.ConfigureConsumer<SignalRBoardMetadataUpdatedIntegrationEventConsumer>(context);

                        e.ConfigureConsumer<SignalRColumnCreatedIntegrationEventConsumer>(context);
                        e.ConfigureConsumer<SignalRColumnUpdatedIntegrationEventConsumer>(context);
                        e.ConfigureConsumer<SignalRColumnDeletedIntegrationEventConsumer>(context);

                        e.ConfigureConsumer<CardCreatedIntegrationEventConsumer>(context);
                        e.ConfigureConsumer<CardUpdatedIntegrationEventConsumer>(context);
                        e.ConfigureConsumer<CardDeletedIntegrationEventConsumer>(context);
                    });

                    // Если нужен fanout/topic/direct для конкретных типов сообщений — добавьте cfg.Publish<T>(...) / e.Bind<T>(...)
                    // cfg.ConfigureEndpoints(context); // НЕ вызываем, т.к. используем один кастомный endpoint выше

                    //cfg.ConfigureEndpoints(context);               // Автосоздание очередей
                });
            });

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

                //busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());        // Локальные задачи. It's not work. I don't know why
                busConfigurator.AddConsumer<SignalRBoardCreatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRBoardDeletedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRBoardMetadataUpdatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRColumnCreatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRColumnUpdatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<SignalRColumnDeletedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<CardCreatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<CardUpdatedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<CardDeletedIntegrationEventConsumer>();

                busConfigurator.UsingInMemory((context, configuration) =>
                {
                    //configuration.ConfigureEndpoints(context);
                    configuration.ReceiveEndpoint("signalr-endpoints", (endpontCfg) =>
                    {
                        endpontCfg.ConfigureConsumer<SignalRBoardCreatedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<SignalRBoardDeletedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<SignalRBoardMetadataUpdatedIntegrationEventConsumer>(context);

                        endpontCfg.ConfigureConsumer<SignalRColumnCreatedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<SignalRColumnUpdatedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<SignalRColumnDeletedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<CardCreatedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<CardUpdatedIntegrationEventConsumer>(context);
                        endpontCfg.ConfigureConsumer<CardDeletedIntegrationEventConsumer>(context);

                    });
                });

            });
        }
    }

}
