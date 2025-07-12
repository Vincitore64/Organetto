using MediatR;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.UseCases.Boards.IntegrationEvents;
using Organetto.UseCases.Shared.Outbox.Services;

namespace Organetto.UseCases.Boards.Commands
{
    /// <summary>
    /// Command to update board metadata (title and description).
    /// </summary>
    public record UpdateBoardMetadataCommand(long BoardId, string Title, string? Description) : IRequest<Unit>;

    /// <summary>
    /// Handler for UpdateBoardMetadataCommand.
    /// </summary>
    public class UpdateBoardMetadataCommandHandler : IRequestHandler<UpdateBoardMetadataCommand, Unit>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IOutboxService _outboxService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBoardMetadataCommandHandler(IBoardRepository boardRepository, IOutboxService outboxService, IUnitOfWork unitOfWork)
        {
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            this._outboxService = outboxService;
            this._unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBoardMetadataCommand request, CancellationToken cancellationToken)
        {
            var dbTransaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var board = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

            // Update metadata
            board.Title = request.Title;
            board.Description = request.Description ?? string.Empty;

            // Persist changes
            await _boardRepository.UpdateAsync(board, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _outboxService.AddAsync(new BoardMetadataUpdatedIntegrationEvent(board.Id), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await dbTransaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
