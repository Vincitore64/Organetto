using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Boards.Hubs;
using Organetto.UseCases.Boards.Hubs.Extensions;
using Organetto.UseCases.Boards.Services;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.IntegrationEvents
{
    public record BoardMetadataUpdatedIntegrationEvent(long BoardId) : IntegrationEvent;

    /// <summary>
    /// Consumes BoardMetadataUpdatedIntegrationEvent,
    /// loads the Board entity, maps to BoardDto,
    /// and notifies the owner's SignalR group.
    /// </summary>
    public class SignalRBoardMetadataUpdatedIntegrationEventConsumer
        : IConsumer<BoardMetadataUpdatedIntegrationEvent>
    {
        private readonly IBoardRepository _boards;
        private readonly IMapper _mapper;
        private readonly IHubContext<BoardHub, IBoardClient> _hub;
        private readonly ILogger<SignalRBoardMetadataUpdatedIntegrationEventConsumer> _logger;

        public SignalRBoardMetadataUpdatedIntegrationEventConsumer(
            IBoardRepository boards,
            IMapper mapper,
            IHubContext<BoardHub, IBoardClient> hub,
            ILogger<SignalRBoardMetadataUpdatedIntegrationEventConsumer> logger)
        {
            _boards = boards ?? throw new ArgumentNullException(nameof(boards));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BoardMetadataUpdatedIntegrationEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation("Received BoardMetadataUpdatedIntegrationEvent for BoardId {BoardId}", evt.BoardId);

            // 1. Load the full board entity
            var board = await _boards.GetByIdAsync(evt.BoardId, context.CancellationToken);

            // 2. Map to public DTO via AutoMapper
            var dto = _mapper.Map<BoardDto>(board);

            // 3. Notify the owner's group: "boards:{ownerId}"
            await _hub.Clients
                      .BoardGroup(board.OwnerId)
                      .BoardMetadataUpdated(dto);
        }
    }
}
