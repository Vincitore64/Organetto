using AutoMapper;
using MediatR;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Data;

namespace Organetto.UseCases.Boards.Queries
{
    public record GetBoardQuery(long BoardId) : IRequest<BoardDetailDto>;

    // Handler implementation
    public class GetBoardQueryHandler : IRequestHandler<GetBoardQuery, BoardDetailDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;

        public GetBoardQueryHandler(
            IBoardRepository boardRepository,
            IMapper mapper)
        {
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BoardDetailDto> Handle(GetBoardQuery request, CancellationToken cancellationToken)
        {
            // Fetch board including its columns and cards
            Board board = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

            // Map domain entity to DTO
            var dto = _mapper.Map<BoardDetailDto>(board);
            return dto;
        }
    }
}
