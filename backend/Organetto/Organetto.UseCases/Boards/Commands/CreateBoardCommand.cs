using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Boards.Hubs;
using Organetto.UseCases.Boards.Services;

namespace Organetto.UseCases.Boards.Commands
{
    /// <summary>
    /// Creates a new Board. (Команда создания доски.)
    /// </summary>
    /// <param name="OwnerId"></param>
    /// <param name="Title"></param>
    /// <param name="Description"></param>
    public record CreateBoardCommand(long OwnerId, string Title, string? Description = null) : IRequest<BoardDto>;

    /// <summary>
    /// Handles CreateBoardCommand by persisting a new Board. (Обрабатывает CreateBoardCommand, сохраняя доску.)
    /// </summary>
    public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, BoardDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<BoardHub, IBoardClient> _hub;
        private readonly IMapper _mapper;

        public CreateBoardCommandHandler(IBoardRepository boardRepository, IUnitOfWork unitOfWork, IHubContext<BoardHub, IBoardClient> hub, IMapper mapper)
        {
            _boardRepository = boardRepository;
            this._unitOfWork = unitOfWork;
            this._hub = hub;
            _mapper = mapper;
        }

        public async Task<BoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            // Map command → domain entity
            var board = new Board
            {
                OwnerId = request.OwnerId,
                Title = request.Title,
                Description = request.Description ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsArchived = false
            };

            // Persist
            var created = await _boardRepository.CreateAsync(board);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to DTO
            var dto = _mapper.Map<BoardDto>(created);
            await _hub.Clients.Group($"boards:{board.OwnerId}").BoardCreated(dto);
            return dto;
        }
    }
}
