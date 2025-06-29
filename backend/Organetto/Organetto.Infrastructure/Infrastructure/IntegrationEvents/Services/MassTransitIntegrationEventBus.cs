using MassTransit;
using Microsoft.Extensions.Logging;
using Organetto.UseCases.Shared.IntegrationEvents.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Services;
using Organetto.UseCases.Shared.MassTransit.Services;

namespace Organetto.Infrastructure.Infrastructure.IntegrationEvents.Services
{
    /// <summary>
    /// Реализация IIntegrationEventBus на основе MassTransit.
    /// </summary>
    public class MassTransitIntegrationEventBus : IIntegrationEventBus
    {
        private readonly IInMemoryBus _publishEndpoint;
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

        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : IIntegrationEvent
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

        public async Task PublishAsync(object integrationEvent, Type type, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
                throw new ArgumentNullException(nameof(integrationEvent));

            try
            {
                await _publishEndpoint.Publish(integrationEvent, type, cancellationToken);
                _logger.LogInformation(
                    "Integration event published: {EventType} (ID: {EventId})",
                    type.Name,
                    type.GetProperty("Id")?.GetValue(integrationEvent));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to publish integration event: {EventType} (ID: {EventId})",
                    type.Name,
                    type.GetProperty("Id")?.GetValue(integrationEvent));
                throw;
            }
        }
    }
}
