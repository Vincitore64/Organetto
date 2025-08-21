using Organetto.Core.Boards.Shared.Models;
using Organetto.BuildingBlocks.Core.Models;
using Organetto.Core.Users.Data;

namespace Organetto.Core.Boards.Data
{
    /// <summary>
    /// Represents a Kanban board.
    /// </summary>
    public class Board : BaseEntity
    {
        public Board()
        {
            Members = new HashSet<BoardMember>();
            Lists = new HashSet<BoardList>();
            Title = string.Empty;
            Description = string.Empty;
        }

        public string Title { get; set; }                               // Board title (название доски)
        public string Description { get; set; }                         // Board description (описание доски)
        public long OwnerId { get; set; }                               // FK to User (внешний ключ к User)
        public DateTime CreatedAt { get; set; }                          // Creation timestamp (время создания)
        public DateTime UpdatedAt { get; set; }                          // Last update timestamp (время последнего обновления)
        public bool IsArchived { get; set; }                             // Soft-delete / archive flag (флаг архивирования)

        // Navigation properties
        public User? Owner { get; set; }                                  // Owner relationship (отношение «владелец»)
        public ICollection<BoardMember> Members { get; set; }            // Many-to-many: users on this board (участники доски)
        public HashSet<BoardList> Lists { get; set; }                // Lists (columns) on this board (списки на этой доске)

        private BoardList? Find(long id) => Lists.FirstOrDefault(x => x.Id == id);

        public void ReorderList(long listId, long? leftSiblingId, long? rightSiblingId)
        {
            var list = Find(listId) ?? throw new InvalidOperationException("List not found on this board.");
            var left = leftSiblingId.HasValue ? Find(leftSiblingId.Value) : null;
            var right = rightSiblingId.HasValue ? Find(rightSiblingId.Value) : null;

            if (left is not null && right is not null && left.Position.Value >= right.Position.Value)
                throw new InvalidOperationException("Left must be before right.");

            var mid = Position.Between(left?.Position, right?.Position);
            if (mid is not null)
            {
                list.Position = mid.Value;
                return;
            }

            // No gap → rebalance with insertion at intended index
            var ordered = Lists.ToList();
            ordered.RemoveAll(x => x.Id == list.Id);

            int insertAt =
                left is not null ? ordered.FindIndex(x => x.Id == left.Id) + 1 :
                right is not null ? Math.Max(0, ordered.FindIndex(x => x.Id == right.Id)) :
                ordered.Count;

            insertAt = Math.Clamp(insertAt, 0, ordered.Count);
            ordered.Insert(insertAt, list);

            var distributed = Position.Distribute(ordered.Count);
            for (int i = 0; i < ordered.Count; i++)
                ordered[i].Position = distributed[i];

            Lists.Clear();
            foreach (var item in ordered)
            {
                Lists.Add(item);
            }
        }

        public void AcceptIncomingList(BoardList list, long? leftSiblingId, long? rightSiblingId)
        {
            if (list.BoardId != Id) list.BoardId = Id;
            Lists.RemoveWhere(x => x.Id == list.Id);
            Lists.Add(list);
            ReorderList(list.Id, leftSiblingId, rightSiblingId);
        }

        public static Board Rehydrate(long id, string name, IEnumerable<BoardList> lists)
        {
            var b = new Board { Id = id, Title = name };

            foreach (var item in lists.Where(x => x.BoardId == id))
            {
                b.Lists.Add(item);
            }

            return b;
        }
    }
}
