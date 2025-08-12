using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Shared.Models;
using Organetto.Core.Shared.Models;

namespace Organetto.Core.Boards.Data
{
    /// <summary>
    /// Represents a list (column) within a board.
    /// </summary>
    public class BoardList : CrudEntity
    {
        public BoardList()
        {
            Cards = new HashSet<Card>();
            Title = string.Empty;
        }

        public long BoardId { get; set; }                               // FK to Board (внешний ключ к Board)
        public string Title { get; set; }                               // List title (название списка)
        public Position Position { get; set; }                                // Order index (позиция)

        // Navigation properties
        public Board? Board { get; set; }                                 // Board navigation (связь с доской)
        public ICollection<Card> Cards { get; set; }                    // Cards in this list (задачи в списке)


        public void PlaceCardAt(Card card, Card? left, Card? right)
        {
            //if (card.) throw new InvalidOperationException("Archived cards cannot be moved.");
            if (left is not null && left.BoardListId != Id) throw new InvalidOperationException("Left sibling is not in target column.");
            if (right is not null && right.BoardListId != Id) throw new InvalidOperationException("Right sibling is not in target column.");
            if (left is not null && right is not null && left.Position.Value >= right.Position.Value)
                throw new InvalidOperationException("Invalid sibling order (left must precede right).");

            var between = Position.Between(left?.Position, right?.Position);
            if (between is not null)
            {
                card.BoardListId = Id;
                card.Position = between.Value;
                return;
                //return MoveOutcome.Single(card);
            }

            // Slow path: full-column rebalance
            var orderedColumnCards = Cards.OrderBy(c => c.Position.Value).ToList();
            orderedColumnCards = orderedColumnCards.Where(c => c.Id != card.Id).ToList();

            int insertAt =
                left is not null ? orderedColumnCards.FindIndex(c => c.Id == left.Id) + 1 :
                right is not null ? Math.Max(0, orderedColumnCards.FindIndex(c => c.Id == right.Id)) :
                orderedColumnCards.Count;

            insertAt = Math.Clamp(insertAt, 0, orderedColumnCards.Count);

            card.BoardListId = Id;
            orderedColumnCards.Insert(insertAt, card);

            var distributed = Organetto.Core.Boards.Shared.Models.Position.Distribute(orderedColumnCards.Count);
            for (int i = 0; i < orderedColumnCards.Count; i++)
                orderedColumnCards[i].Position = distributed[i];

            Cards.Clear();
            foreach (var item in orderedColumnCards)
            {
                Cards.Add(item);
            }
        }

    }
}
