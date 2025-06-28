using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Boards.Hubs;
using Organetto.UseCases.Boards.Hubs.Extensions;
using Organetto.UseCases.Boards.Services;
using Organetto.UseCases.Shared.SignalR.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.IntegrationEvents
{
    /// <summary>
    /// Fired when a board is created. Carries only the board’s ID. 
    /// </summary>
    /// <param name="BoardId"></param>
    public record BoardCreatedIntegrationEvent(long BoardId) : IntegrationEvent;

    /// <summary>
    /// Consumes BoardCreatedIntegrationEvent,
    /// loads the Board entity, maps to BoardDto, 
    /// and notifies the owner’s SignalR group.
    /// </summary>
    public class BoardCreatedIntegrationEventConsumer :
        IConsumer<BoardCreatedIntegrationEvent>
    {
        private readonly IBoardRepository _boards;
        private readonly IMapper _mapper;
        private readonly IHubContext<BoardHub, IBoardClient> _hub;
        private readonly ILogger<BoardCreatedIntegrationEventConsumer> _logger;

        public BoardCreatedIntegrationEventConsumer(
            IBoardRepository boards,
            IMapper mapper,
            IHubContext<BoardHub, IBoardClient> hub,
            ILogger<BoardCreatedIntegrationEventConsumer> logger)
        {
            _boards = boards;
            _mapper = mapper;
            _hub = hub;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<BoardCreatedIntegrationEvent> ctx)
        {
            var evt = ctx.Message;

            // 1. Load the full board entity
            var board = await _boards.GetByIdAsync(evt.BoardId, ctx.CancellationToken);

            // 2. Map to your public DTO via AutoMapper
            var dto = _mapper.Map<BoardDto>(board);

            // 3. Push to the owner’s group: "boards:{ownerId}"
            await _hub.Clients
                      .BoardGroup(board.OwnerId)
                      .BoardCreated(dto);
        }
    }
}
