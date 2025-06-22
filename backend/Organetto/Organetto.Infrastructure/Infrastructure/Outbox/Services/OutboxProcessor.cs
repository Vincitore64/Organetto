using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Models;
using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Services;

namespace Organetto.Infrastructure.Infrastructure.Outbox.Services
{
    /// <summary>
    /// Фоновая служба для публикации сообщений из таблицы Outbox.
    /// </summary>
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxProcessor> _logger;
        private const int BatchSize = 20;
        private static readonly TimeSpan Delay = TimeSpan.FromSeconds(30);

        public OutboxProcessor(IServiceProvider serviceProvider, ILogger<OutboxProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var eventBus = scope.ServiceProvider.GetRequiredService<IIntegrationEventBus>();

                    var messages = await dbContext.OutboxMessages
                        .Where(x => x.ProcessedOn == null)
                        .OrderBy(x => x.OccurredOn)
                        .Take(BatchSize)
                        .ToListAsync(stoppingToken);

                    foreach (var message in messages)
                    {
                        try
                        {
                            var type = Type.GetType(message.Type, throwOnError: true)!;
                            var integrationEvent = (IIntegrationEvent)JsonConvert.DeserializeObject(message.Payload, type)!;

                            await eventBus.PublishAsync(integrationEvent, stoppingToken);
                            message.ProcessedOn = DateTimeOffset.UtcNow;
                        }
                        catch (Exception ex)
                        {
                            message.RetryCount++;
                            message.Error = ex.Message.Length <= 3000 ? ex.Message : ex.Message.Substring(0, 3000);
                            _logger.LogError(ex, "Error publishing Outbox message {MessageId}", message.Id);
                        }
                    }

                    if (messages.Any())
                    {
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "OutboxProcessor loop error");
                }

                await Task.Delay(Delay, stoppingToken);
            }
        }
    }
}
