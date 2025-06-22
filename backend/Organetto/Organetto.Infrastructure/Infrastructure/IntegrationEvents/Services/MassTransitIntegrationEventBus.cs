using MassTransit;
using Microsoft.Extensions.Logging;
using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Models;
using Organetto.Infrastructure.Infrastructure.MassTransit.Services;

namespace Organetto.Infrastructure.Infrastructure.IntegrationEvents.Services
{
    /// <summary>
    /// Реализация IIntegrationEventBus на основе MassTransit.
    /// </summary>
    public class MassTransitIntegrationEventBus : IIntegrationEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<MassTransitIntegrationEventBus> _logger;

        public MassTransitIntegrationEventBus(
            IInMemoryBus publishEndpoint,
            ILogger<MassTransitIntegrationEventBus> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        //public MassTransitIntegrationEventBus(
        //    IPublishEndpoint publishEndpoint,
        //    ILogger<MassTransitIntegrationEventBus> logger)
        //{
        //    _publishEndpoint = publishEndpoint;
        //    _logger = logger;
        //}

        public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
                throw new ArgumentNullException(nameof(integrationEvent));

            try
            {
                await _publishEndpoint.Publish(integrationEvent, cancellationToken);
                _logger.LogInformation(
                    "Integration event published: {EventType} (ID: {EventId})",
                    integrationEvent.GetType().Name,
                    integrationEvent.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to publish integration event: {EventType} (ID: {EventId})",
                    integrationEvent.GetType().Name,
                    integrationEvent.Id);
                throw;
            }
        }
    }
}
