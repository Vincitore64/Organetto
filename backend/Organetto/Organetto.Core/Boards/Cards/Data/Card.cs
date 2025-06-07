using Organetto.Core.Boards.Data;

namespace Organetto.Core.Boards.Cards.Data
{
    /// <summary>
    /// Represents a card/task in a list.
    /// </summary>
    public class Card
    {
        public Card()
        {
            Comments = new HashSet<Comment>();
            Attachments = new HashSet<Attachment>();
            DueDates = new HashSet<DueDate>();
            Title = string.Empty;
            Description = string.Empty;
        }

        public long Id { get; set; }                                    // Surrogate PK (суррогатный первичный ключ)
        public long BoardListId { get; set; }                           // FK to BoardList (внешний ключ к BoardList)
        public string Title { get; set; }                               // Card title (название карточки)
        public string Description { get; set; }                         // Card description/body (описание карточки)
        public int Position { get; set; }                                // Order index within list (позиция)
        public DateTime CreatedAt { get; set; }                          // Creation timestamp (время создания)
        public DateTime UpdatedAt { get; set; }                          // Last update timestamp (время последнего обновления)

        // Navigation properties
        public BoardList? BoardList { get; set; }                         // List navigation (связь с списком)
        public ICollection<Comment> Comments { get; set; }               // Comments on this card (комментарии)
        public ICollection<Attachment> Attachments { get; set; }         // Attachments on this card (вложения)
        public ICollection<DueDate> DueDates { get; set; }               // Due dates for this card (сроки выполнения)
    }
}
