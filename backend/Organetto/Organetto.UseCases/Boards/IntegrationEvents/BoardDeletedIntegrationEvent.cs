using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Hubs;
using Organetto.UseCases.Boards.Hubs.Extensions;
using Organetto.UseCases.Boards.Services;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.IntegrationEvents
{
    /// <summary>
    /// Fired when a board is deleted (archived). Carries only the board’s ID.
    /// </summary>
    /// <param name="Id"><inheritdoc/></param>
    /// <param name="OccurredOn"><inheritdoc/></param>
    /// <param name="BoardId"></param>
    public record BoardDeletedIntegrationEvent(long BoardId) : IntegrationEvent
    {
    }

    /// <summary>
    /// Consumes BoardDeletedIntegrationEvent,
    /// loads the board to get OwnerId, 
    /// and notifies via SignalR.
    /// </summary>
    public class SignalRBoardDeletedIntegrationEventConsumer :
        IConsumer<BoardDeletedIntegrationEvent>
    {
        private readonly IBoardRepository _boards;
        private readonly IHubContext<BoardHub, IBoardClient> _hub;

        public SignalRBoardDeletedIntegrationEventConsumer(
            IBoardRepository boards,
            IHubContext<BoardHub, IBoardClient> hub)
        {
            _boards = boards;
            _hub = hub;
        }

        public async Task Consume(ConsumeContext<BoardDeletedIntegrationEvent> context)
        {
            var boardId = context.Message.BoardId;

            // 1. Fetch the board (it’s still in DB, albeit archived)
            var board = await _boards.GetByIdAsync(boardId, context.CancellationToken);
            if (board == null)
                return; // nothing to notify

            // 2. Notify clients in the owner’s group
            await _hub.Clients
                      .BoardGroup(board.OwnerId)
                      .BoardDeleted(boardId);
        }
    }
}
