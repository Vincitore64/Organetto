using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Models;

namespace Organetto.Infrastructure.Infrastructure.Outbox.Services
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
    }
}
