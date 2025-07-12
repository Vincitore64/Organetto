using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Boards.Hubs;
using Organetto.UseCases.Boards.Hubs.Extensions;
using Organetto.UseCases.Boards.Services;
using Organetto.UseCases.Shared.IntegrationEvents.Consumers;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.IntegrationEvents
{
    public record BoardMetadataUpdatedIntegrationEvent(long EntityId, long OwnerId, long[] MemberIds) : IntegrationEvent, IEntityIntegrationEvent<long>;

    /// <summary>
    /// Consumes BoardMetadataUpdatedIntegrationEvent,
    /// loads the Board entity, maps to BoardDto,
    /// and notifies the owner's SignalR group.
    /// </summary>
    public class SignalRBoardMetadataUpdatedIntegrationEventConsumer
        : CrudIntegrationEventConsumer<BoardMetadataUpdatedIntegrationEvent, long, Board, BoardDto>
    {
        private readonly IHubContext<BoardHub, IBoardClient> _hub;

        public SignalRBoardMetadataUpdatedIntegrationEventConsumer(
            IBoardRepository boards,
            IMapper mapper,
            IHubContext<BoardHub, IBoardClient> hub,
            ILogger<SignalRBoardMetadataUpdatedIntegrationEventConsumer> logger) : base(boards, mapper, logger)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        protected override async Task ProcessEventAsync(BoardMetadataUpdatedIntegrationEvent evt, BoardDto dto)
        {
            await _hub.Clients.BoardGroup(evt.OwnerId).BoardMetadataUpdated(dto);
            // TODO: Notify other users. Need to support on frontend
            //var payload = evt.MemberIds.Select(id => _hub.Clients.UserGroup(id).BoardMetadataUpdated(dto));
            //await Task.WhenAll(payload);
        }
    }
}
