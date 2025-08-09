using AutoMapper;
using MediatR;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.UseCases.Boards.Columns.Data;

namespace Organetto.UseCases.Boards.Columns.Commands
{
    public sealed record MoveColumnCommand(
        long ListId,
        long TargetBoardId,
        long? LeftSiblingId,
        long? RightSiblingId
    ) : IRequest<BoardListDto[]>;

    public sealed class MoveColumnCommandHandler : IRequestHandler<MoveColumnCommand, BoardListDto[]>
    {
        private readonly IColumnRepository _columns;
        private readonly IBoardRepository _boardRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MoveColumnCommandHandler(IColumnRepository columns, IBoardRepository boardRepository, IUnitOfWork uow, IMapper mapper)
        {
            _columns = columns;
            this._boardRepository = boardRepository;
            _uow = uow;
            this._mapper = mapper;
        }

        public async Task<BoardListDto[]> Handle(MoveColumnCommand cmd, CancellationToken ct)
        {
            var list = await _columns.GetByIdAsync(cmd.ListId, ct)
                           ?? throw new InvalidOperationException("List not found.");

            // Load source and target aggregates with their lists
            var sourceBoard = await _boardRepository.GetByIdAsync(list.BoardId, ct); // Rehydrate with lists
            var targetBoard = cmd.TargetBoardId == list.BoardId
                ? sourceBoard
                : await _boardRepository.GetByIdAsync(cmd.TargetBoardId, ct);

            if (sourceBoard.Id == targetBoard.Id)
            {
                targetBoard.ReorderList(list.Id, cmd.LeftSiblingId, cmd.RightSiblingId);
            }
            else
            {
                // detach from source
                // (optionally: sourceBoard.RemoveForMove(list.Id) to enforce invariant that a list belongs to exactly one board)
                targetBoard.AcceptIncomingList(list, cmd.LeftSiblingId, cmd.RightSiblingId);
            }

            await _uow.SaveChangesAsync(ct);

            var result = _mapper.Map<BoardListDto[]>(targetBoard.Lists);

            return result;

            //var dbTransaction = await _uow.BeginTransactionAsync(ct);
            //try
            //{
            //    var list = await _columns.GetByIdAsync(cmd.ListId, ct)
            //               ?? throw new InvalidOperationException("List not found.");

            //    Load source and target aggregates with their lists
            //    var sourceBoard = await _boardRepository.GetByIdAsync(list.BoardId, ct); // Rehydrate with lists
            //    var targetBoard = cmd.TargetBoardId == list.BoardId
            //        ? sourceBoard
            //        : await _boardRepository.GetByIdAsync(cmd.TargetBoardId, ct);

            //    if (sourceBoard.Id == targetBoard.Id)
            //    {
            //        targetBoard.ReorderList(list.Id, cmd.LeftSiblingId, cmd.RightSiblingId);
            //    }
            //    else
            //    {
            //        detach from source
            //        (optionally: sourceBoard.RemoveForMove(list.Id) to enforce invariant that a list belongs to exactly one board)
            //        targetBoard.AcceptIncomingList(list, cmd.LeftSiblingId, cmd.RightSiblingId);
            //    }

            //    await _uow.SaveChangesAsync(ct);

            //    await _boards.SaveAsync(targetBoard, ct);
            //    if (sourceBoard.Id != targetBoard.Id)
            //        await _boards.SaveAsync(sourceBoard, ct); // if you changed it (e.g., removed list placeholder)

            //    await dbTransaction.CommitAsync(ct);
            //}
            //catch
            //{
            //    await dbTransaction.RollbackAsync(ct);
            //    throw;
            //}
        }
    }
}
