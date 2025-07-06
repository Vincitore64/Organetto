using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Shared.IntegrationEvents.Services
{
    /// <summary>
    /// Абстракция для публикации интеграционных событий.
    /// </summary>
    public interface IIntegrationEventBus
    {
        /// <summary>
        /// Публикует интеграционное событие во внешнюю шину.
        /// </summary>
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T: IIntegrationEvent;
        Task PublishAsync(object integrationEvent, Type type, CancellationToken cancellationToken = default);
    }
}