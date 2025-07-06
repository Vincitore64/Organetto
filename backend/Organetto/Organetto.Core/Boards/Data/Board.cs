using Organetto.Core.Users.Data;

namespace Organetto.Core.Boards.Data
{
    /// <summary>
    /// Represents a Kanban board.
    /// </summary>
    public class Board
    {
        public Board()
        {
            Members = new HashSet<BoardMember>();
            Lists = new HashSet<BoardList>();
            Title = string.Empty;
            Description = string.Empty;
        }

        public long Id { get; set; }                                    // Surrogate PK (суррогатный первичный ключ)
        public string Title { get; set; }                               // Board title (название доски)
        public string Description { get; set; }                         // Board description (описание доски)
        public long OwnerId { get; set; }                               // FK to User (внешний ключ к User)
        public DateTime CreatedAt { get; set; }                          // Creation timestamp (время создания)
        public DateTime UpdatedAt { get; set; }                          // Last update timestamp (время последнего обновления)
        public bool IsArchived { get; set; }                             // Soft-delete / archive flag (флаг архивирования)

        // Navigation properties
        public User? Owner { get; set; }                                  // Owner relationship (отношение «владелец»)
        public ICollection<BoardMember> Members { get; set; }            // Many-to-many: users on this board (участники доски)
        public ICollection<BoardList> Lists { get; set; }                // Lists (columns) on this board (списки на этой доске)
    }
}
