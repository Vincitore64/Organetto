using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Shared.Outbox.Services
{
    /// <summary>
    /// Сервис для добавления интеграционных событий в таблицу Outbox.
    /// </summary>
    public interface IOutboxService
    {
        /// <summary>
        /// Добавляет событие в Outbox для последующей асинхронной публикации.
        /// </summary>
        /// <param name="integrationEvent">Интеграционное событие.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Scans the supplied aggregates for any un‐published domain events,
        /// converts them to integration events and enqueues them in the Outbox.
        /// </summary>
        /// <param name="aggregates">All entities that may have domain events.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        Task ProcessDomainEventsAsync(
            CancellationToken cancellationToken = default
        );
    }
}