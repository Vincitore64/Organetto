using AutoMapper;
using MediatR;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Data;

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
        private readonly IMapper _mapper;

        public CreateBoardCommandHandler(IBoardRepository boardRepository, IMapper mapper)
        {
            _boardRepository = boardRepository;
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

            // Map to DTO
            var dto = _mapper.Map<BoardDto>(created);
            return dto;
        }
    }
}
