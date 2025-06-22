using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Models;

namespace Organetto.Infrastructure.Infrastructure.IntegrationEvents.Services
{
    /// <summary>
    /// Абстракция для публикации интеграционных событий.
    /// </summary>
    public interface IIntegrationEventBus
    {
        /// <summary>
        /// Публикует интеграционное событие во внешнюю шину.
        /// </summary>
        Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    }
}
