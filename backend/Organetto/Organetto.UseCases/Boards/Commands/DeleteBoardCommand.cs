using MediatR;
using Organetto.Core.Boards.Services;

namespace Organetto.UseCases.Boards.Commands
{
    /// <summary>
    /// Command to delete (archive) a board. (Команда для удаления (архивирования) доски.)
    /// </summary>
    /// <param name="BoardId"> ID of the board to delete. (ID доски для удаления.) </param>
    /// <param name="UserId"> ID of the user requesting deletion (must be owner). (ID пользователя, запрашивающего удаление (должен быть владельцем).) </param>
    public record DeleteBoardCommand(long BoardId, long UserId) : IRequest<Unit>;

    /// <summary>
    /// Handles DeleteBoardCommand by verifying ownership and then archiving the board. 
    /// (Хендлер проверяет владельца и затем архивирует доску.)
    /// </summary>
    public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, Unit>
    {
        private readonly IBoardRepository _boardRepository;

        public DeleteBoardCommandHandler(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task<Unit> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
        {
            // 1. Load the board to verify existence and owner.
            var board = await _boardRepository.GetByIdAsync(request.BoardId);
            if (board == null)
                throw new KeyNotFoundException($"Board with id {request.BoardId} not found.");

            // 2. Ensure only owner may delete.
            if (board.OwnerId != request.UserId)
                throw new UnauthorizedAccessException("Only the board owner can delete the board.");

            // 3. Perform delete (archive) operation.
            await _boardRepository.DeleteAsync(request.BoardId);

            return Unit.Value;
        }
    }
}
