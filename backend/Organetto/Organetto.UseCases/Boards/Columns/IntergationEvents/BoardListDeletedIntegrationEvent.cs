using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Boards.Columns.Hubs.Clients;
using Organetto.UseCases.Boards.Columns.Hubs;
using Organetto.UseCases.Shared.IntegrationEvents.Consumers;
using Organetto.UseCases.Shared.IntegrationEvents.Models;
using Organetto.UseCases.Boards.Hubs.Extensions;

namespace Organetto.UseCases.Boards.Columns.IntergationEvents
{
    public record BoardListDeletedIntegrationEvent(long EntityId, long BoardId) : IntegrationEvent, IEntityIntegrationEvent<long>
    {
        public BoardListDeletedIntegrationEvent() : this(0, 0)
        {

        }
    }

    /// <summary>
    /// Consumer for ColumnCreatedIntegrationEvent.
    /// Uses the generic CrudIntegrationEventConsumer to load the entity and map to DTO.
    /// </summary>
    public class SignalRColumnDeletedIntegrationEventConsumer
        : CrudIntegrationEventConsumer<BoardListDeletedIntegrationEvent, long, BoardList, BoardListDto>
    {
        private readonly IHubContext<ColumnHub, IColumnClient> _hub;

        public SignalRColumnDeletedIntegrationEventConsumer(
            IColumnRepository lookup,
            IMapper mapper,
            IHubContext<ColumnHub, IColumnClient> hub,
            ILogger<SignalRColumnDeletedIntegrationEventConsumer> logger)
            : base(lookup, mapper, logger)
        {
            _hub = hub;
        }

        protected override async Task ProcessEventAsync(
            BoardListDeletedIntegrationEvent evt,
            BoardListDto dto)
        {
            await _hub.Clients.BoardGroup(evt.BoardId).ColumnDeleted(dto.Id);
        }

        protected override async Task ProcessEventAsync(BoardListDeletedIntegrationEvent evt)
        {
            await _hub.Clients.BoardGroup(evt.BoardId).ColumnDeleted(evt.EntityId);
        }
    }
}
