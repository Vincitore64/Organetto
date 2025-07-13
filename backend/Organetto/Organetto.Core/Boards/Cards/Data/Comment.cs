using Organetto.Core.Shared.Models;
using Organetto.Core.Users.Data;

namespace Organetto.Core.Boards.Cards.Data
{
    /// <summary>
    /// Represents a comment on a card.
    /// </summary>
    public class Comment : BaseEntity
    {
        public Comment()
        {
            Body = string.Empty;
        }

        public long CardId { get; set; }                                // FK to Card (внешний ключ к Card)
        public long AuthorId { get; set; }                              // FK to User (внешний ключ к User)
        public string Body { get; set; }                                 // Comment text (текст комментария)
        public DateTime CreatedAt { get; set; }                          // Timestamp (время создания)

        // Navigation properties
        public Card? Card { get; set; }                                   // Card navigation (связь с карточкой)
        public User? Author { get; set; }                                 // Author navigation (связь с автором)
    }
}
