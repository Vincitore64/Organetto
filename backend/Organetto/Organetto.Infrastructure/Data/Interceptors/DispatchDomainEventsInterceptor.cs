using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Organetto.Core.Shared.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers;
using Organetto.UseCases.Shared.Outbox.Services;

namespace Organetto.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {
        private readonly IOutboxService _outboxService;
        private readonly IEventsMapper _eventsMapper;

        public DispatchDomainEventsInterceptor(IOutboxService outboxService, IEventsMapper eventsMapper)
        {
            this._outboxService = outboxService;
            this._eventsMapper = eventsMapper;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;

            var entities = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.Events.Any())
                .Select(e => e.Entity);

            var domainEvents = entities
                .SelectMany(e => e.Events)
                .ToList();

            entities.ToList().ForEach(e => e.Events.Clear());

            var integrationEvents = _eventsMapper.Map(domainEvents).ToArray();

            foreach (var integrationEvent in integrationEvents)
            {
                await _outboxService.AddAsync(integrationEvent);
            }
        }
    }
}
