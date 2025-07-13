using Organetto.Core.Shared.Models;
using Organetto.Core.Users.Data;

namespace Organetto.Core.Boards.Cards.Data
{
    /// <summary>
    /// Represents an uploaded attachment for a card.
    /// </summary>
    public class Attachment : BaseEntity
    {
        public Attachment()
        {
            FileUrl = string.Empty;
            Filename = string.Empty;
        }

        public long CardId { get; set; }                                // FK to Card (внешний ключ к Card)
        public long UploaderId { get; set; }                             // FK to User (внешний ключ к User)
        public string FileUrl { get; set; }                              // URL or path to file (ссылка на файл)
        public string Filename { get; set; }                             // Original filename (оригинальное имя файла)
        public DateTime UploadedAt { get; set; }                         // Timestamp (время загрузки)

        // Navigation properties
        public Card? Card { get; set; }                                   // Card navigation (связь с карточкой)
        public User? Uploader { get; set; }                               // Uploader navigation (связь с загрузившим)
    }
}
