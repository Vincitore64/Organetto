using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Boards.Columns.Hubs;
using Organetto.UseCases.Boards.Columns.Hubs.Clients;
using Organetto.UseCases.Boards.Hubs.Extensions;
using Organetto.UseCases.Shared.IntegrationEvents.Consumers;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.Columns.IntergationEvents
{
    public record BoardListUpdatedIntegrationEvent(long EntityId, long BoardId) : IntegrationEvent, IEntityIntegrationEvent<long>
    {
        public BoardListUpdatedIntegrationEvent() : this(0, 0)
        {
            
        }
    }

    /// <summary>
    /// Consumer for ColumnCreatedIntegrationEvent.
    /// Uses the generic CrudIntegrationEventConsumer to load the entity and map to DTO.
    /// </summary>
    public class SignalRColumnUpdatedIntegrationEventConsumer
        : CrudIntegrationEventConsumer<BoardListUpdatedIntegrationEvent, long, BoardList, BoardListDto>
    {
        private readonly IHubContext<ColumnHub, IColumnClient> _hub;

        public SignalRColumnUpdatedIntegrationEventConsumer(
            IColumnRepository lookup,
            IMapper mapper,
            IHubContext<ColumnHub, IColumnClient> hub,
            ILogger<SignalRColumnUpdatedIntegrationEventConsumer> logger)
            : base(lookup, mapper, logger)
        {
            this._hub = hub;
        }

        protected override async Task ProcessEventAsync(
            BoardListUpdatedIntegrationEvent evt,
            BoardListDto dto)
        {
            await _hub.Clients.BoardGroup(evt.BoardId).ColumnMetadataUpdated(dto);
        }
    }
}
