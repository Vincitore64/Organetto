using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Columns.Cards.Hubs;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Boards.Hubs.Extensions;
using Organetto.UseCases.Shared.IntegrationEvents.Consumers;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.Columns.Cards.IntegrationEvents
{
    public record CardUpdatedIntegrationEvent(long BoardId, long BoardListId, long EntityId) : IntegrationEvent, IEntityIntegrationEvent<long>
    {
        public CardUpdatedIntegrationEvent() : this(0, 0, 0)
        {

        }
    }

    /// <summary>
    /// Consumer for ColumnCreatedIntegrationEvent.
    /// Uses the generic CrudIntegrationEventConsumer to load the entity and map to DTO.
    /// </summary>
    public class CardUpdatedIntegrationEventConsumer
        : CrudIntegrationEventConsumer<CardUpdatedIntegrationEvent, long, Card, CardDto>
    {
        private readonly IHubContext<CardHub, ICardClient> _hub;

        public CardUpdatedIntegrationEventConsumer(
            ICardRepository lookup,
            IMapper mapper,
            IHubContext<CardHub, ICardClient> hub,
            ILogger<CardUpdatedIntegrationEventConsumer> logger)
            : base(lookup, mapper, logger)
        {
            _hub = hub;
        }

        protected override async Task ProcessEventAsync(
            CardUpdatedIntegrationEvent evt,
            CardDto dto)
        {
            await _hub.Clients.BoardGroup(evt.BoardId).CardUpdated(dto);
        }
    }
}
