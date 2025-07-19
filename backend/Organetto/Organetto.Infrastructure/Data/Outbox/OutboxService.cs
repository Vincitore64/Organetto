using Newtonsoft.Json;
using Organetto.Core.Shared.Models;
using Organetto.Infrastructure.Data.Shared;
using Organetto.UseCases.Shared.IntegrationEvents.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers;
using Organetto.UseCases.Shared.Outbox.Models;
using Organetto.UseCases.Shared.Outbox.Services;

namespace Organetto.Infrastructure.Data.Outbox
{
    /// <inheritdoc />
    public class OutboxService : IOutboxService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEventsMapper _eventsMapper;

        public OutboxService(ApplicationDbContext dbContext, IEventsMapper eventsMapper)
        {
            _dbContext = dbContext;
            this._eventsMapper = eventsMapper;
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

        public async Task ProcessDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var entities = _dbContext.ChangeTracker
                .Entries<IHasDomainEvents>()
                .Where(e => e.Entity.Events.Any())
                .Select(e => e.Entity);

            var domainEvents = entities
                .SelectMany(e => e.Events)
            .ToList();

            entities.ToList().ForEach(e => e.ClearEvents());

            var integrationEvents = _eventsMapper.Map(domainEvents).ToArray();

            foreach (var integrationEvent in integrationEvents)
            {
                await AddAsync(integrationEvent, cancellationToken);
            }
        }
    }
}
