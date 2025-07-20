using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Services;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Boards.Columns.Hubs;
using Organetto.UseCases.Boards.Columns.Hubs.Clients;
using Organetto.UseCases.Boards.Hubs.Extensions;
using Organetto.UseCases.Shared.IntegrationEvents.Consumers;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.Columns.IntergationEvents
{
    /// <summary>
    /// Fired when a new column (list) is created. Carries the column's ID.
    /// </summary>
    public record ColumnCreatedIntegrationEvent(long EntityId, long BoardId)
        : IntegrationEvent, IEntityIntegrationEvent<long>;

    /// <summary>
    /// Consumer for ColumnCreatedIntegrationEvent.
    /// Uses the generic CrudIntegrationEventConsumer to load the entity and map to DTO.
    /// </summary>
    public class SignalRColumnCreatedIntegrationEventConsumer
        : CrudIntegrationEventConsumer<ColumnCreatedIntegrationEvent, long, BoardList, BoardListDto>
    {
        private readonly IHubContext<ColumnHub, IColumnClient> _hub;

        public SignalRColumnCreatedIntegrationEventConsumer(
            IColumnRepository lookup,
            IMapper mapper,
            IHubContext<ColumnHub, IColumnClient> hub,
            ILogger<SignalRColumnCreatedIntegrationEventConsumer> logger)
            : base(lookup, mapper, logger)
        {
            _hub = hub;
        }

        protected override async Task ProcessEventAsync(
            ColumnCreatedIntegrationEvent evt,
            BoardListDto dto)
        {
            await _hub.Clients.BoardGroup(evt.BoardId).ColumnCreated(dto);
        }
    }
}
