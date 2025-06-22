using Newtonsoft.Json;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Infrastructure.IntegrationEvents.Models;
using Organetto.Infrastructure.Infrastructure.Outbox.Models;
using Organetto.Infrastructure.Infrastructure.Outbox.Services;

namespace Organetto.Infrastructure.Data.Outbox
{
    /// <inheritdoc />
    public class OutboxService : IOutboxService
    {
        private readonly ApplicationDbContext _dbContext;

        public OutboxService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
                throw new ArgumentNullException(nameof(integrationEvent));

            var message = new OutboxMessage
            {
                Id = integrationEvent.Id,
                OccurredOn = integrationEvent.OccurredOn,
                Type = integrationEvent.GetType().FullName!,
                Payload = JsonConvert.SerializeObject(integrationEvent),
                CorrelationId = null,
                RetryCount = 0,
                ProcessedOn = null,
                Error = null
            };

            await _dbContext.OutboxMessages.AddAsync(message, cancellationToken);
            // Не вызываем SaveChanges здесь, чтобы сохранить транзакционность вместе с бизнес-операцией
        }
    }
}
