using AutoMapper;
using MediatR;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Data;
using System.ComponentModel.DataAnnotations;

namespace Organetto.UseCases.Boards.Queries
{
    public record GetAllUserBoardsQuery([property: Range(1, long.MaxValue)] long UserId): IRequest<IEnumerable<BoardDto>>
    {
    }

    /// <summary>
    /// Handler for GetAllUserBoardsQuery. (Хендлер для GetAllUserBoardsQuery.)
    /// </summary>
    public class GetAllUserBoardsQueryHandler : IRequestHandler<GetAllUserBoardsQuery, IEnumerable<BoardDto>>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor: injects repository and AutoMapper. (Конструктор: внедряет репозиторий и AutoMapper.)
        /// </summary>
        public GetAllUserBoardsQueryHandler(IBoardRepository boardRepository, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<BoardDto>> Handle(GetAllUserBoardsQuery request, CancellationToken cancellationToken)
        {
            // Fetch all boards for the user. (Получаем все доски для пользователя.)
            var boards = await _boardRepository.GetAllForUserAsync(request.UserId, cancellationToken);

            // Map EF entities to DTOs. (Маппим EF-сущности в DTO.)
            var boardDtos = boards
                .Select(board => _mapper.Map<BoardDto>(board))
                .ToList();

            return boardDtos;
        }
    }
}
