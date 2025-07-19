using Organetto.Core.Boards.Cards.Data;
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
        public int Position { get; set; }                                // Order index (позиция)

        // Navigation properties
        public Board? Board { get; set; }                                 // Board navigation (связь с доской)
        public ICollection<Card> Cards { get; set; }                    // Cards in this list (задачи в списке)
    }
}
