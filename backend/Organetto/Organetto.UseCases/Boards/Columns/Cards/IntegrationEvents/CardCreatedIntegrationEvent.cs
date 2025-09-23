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
    /// <summary>
    /// Fired when a new card is created. Carries the card's ID.
    /// </summary>
    public record CardCreatedIntegrationEvent(long BoardId, long BoardListId, long EntityId)
        : IntegrationEvent, IEntityIntegrationEvent<long>
    {
        public CardCreatedIntegrationEvent(): this(0, 0, 0)
        {
            
        }
    }

    /// <summary>
    /// Consumer for CardCreatedIntegrationEvent.
    /// Uses the generic CrudIntegrationEventConsumer to load the entity and map to DTO.
    /// </summary>
    public class CardCreatedIntegrationEventConsumer
        : CrudIntegrationEventConsumer<CardCreatedIntegrationEvent, long, Card, CardDto>
    {
        private readonly IHubContext<CardHub, ICardClient> _hub;

        public CardCreatedIntegrationEventConsumer(
            ICardRepository lookup,
            IMapper mapper,
            IHubContext<CardHub, ICardClient> hub,
            ILogger<CardCreatedIntegrationEventConsumer> logger)
            : base(lookup, mapper, logger)
        {
            _hub = hub;
        }

        protected override async Task ProcessEventAsync(
            CardCreatedIntegrationEvent evt,
            CardDto dto)
        {
            await _hub.Clients.BoardGroup(evt.BoardId).CardCreated(dto);
        }
    }
}
