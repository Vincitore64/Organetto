using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Organetto.Core.Shared.Services;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Shared.IntegrationEvents.Consumers
{
    public abstract class CrudIntegrationEventConsumer<
        TEvent,
        TKey,
        TEntity,
        TDto>
    : IConsumer<TEvent>
        where TEvent : IntegrationEvent, IEntityIntegrationEvent<TKey>
        //where THub : Hub<TClient>
        //where TClient : class
    {
        private readonly IReadByIdRepository<TEntity, TKey> _lookup;
        private readonly IMapper _mapper;
        //private readonly IHubContext<THub, TClient> _hub;
        private readonly ILogger _logger;

        protected CrudIntegrationEventConsumer(
            IReadByIdRepository<TEntity, TKey> lookup,
            IMapper mapper,
            ILogger logger)
        {
            _lookup = lookup ?? throw new ArgumentNullException(nameof(lookup));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            //_hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation("Received {Event} for ID {Id}", typeof(TEvent).Name, evt.Id);

            try
            {
                var entity = await _lookup.GetByIdAsync(evt.EntityId, context.CancellationToken);
                if (entity == null && !ShouldIgnoreMissing()) return;

                var dto = entity is not null
                    ? _mapper.Map<TDto>(entity)
                    : default;

                if (dto == null)
                {
                    await ProcessEventAsync(evt);
                    return;
                }

                await ProcessEventAsync(evt, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing {Event} for ID {Id}", typeof(TEvent).Name, evt.Id);
                await ProcessEventAsync(evt);
            }
        }

        protected virtual bool ShouldIgnoreMissing() => true;
        protected abstract Task ProcessEventAsync(TEvent evt, TDto dto);
        protected virtual Task ProcessEventAsync(TEvent evt)
        {
            return Task.CompletedTask;
        }
    }
}
