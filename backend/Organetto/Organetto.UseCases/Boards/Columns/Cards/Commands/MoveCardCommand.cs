using MediatR;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;

namespace Organetto.UseCases.Boards.Columns.Cards.Commands
{
    public sealed record MoveCardCommand(
        long CardId,
        long TargetColumnId,
        long? LeftSiblingId,
        long? RightSiblingId
    ) : IRequest<Unit>;

    public sealed class MoveCardCommandHandler : IRequestHandler<MoveCardCommand, Unit>
    {
        private readonly ICardRepository _cards;
        private readonly IColumnRepository _columns;
        private readonly IUnitOfWork _unitOfWork;

        public MoveCardCommandHandler(
            ICardRepository cards,
            IColumnRepository columns,
            IUnitOfWork unitOfWork)
        {
            _cards = cards;
            _columns = columns;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(MoveCardCommand cmd, CancellationToken ct)
        {
            var card = await _cards.GetByIdAsync(cmd.CardId, ct);

            var neighborIds = new[] { cmd.LeftSiblingId, cmd.RightSiblingId }
                .Where(x => x.HasValue).Select(x => x!.Value).ToArray();

            var column = await _columns.GetByIdAsync(cmd.TargetColumnId, ct);

            var neighbors = column.Cards.Where(x => neighborIds.Contains(x.Id)).ToArray();

            var left = neighbors.FirstOrDefault(x => x.Id == cmd.LeftSiblingId);
            var right = neighbors.FirstOrDefault(x => x.Id == cmd.RightSiblingId);
            var columnMeta = await _columns.GetByIdAsync(cmd.TargetColumnId, ct);

            column.PlaceCardAt(card, left, right);

            await _unitOfWork.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
