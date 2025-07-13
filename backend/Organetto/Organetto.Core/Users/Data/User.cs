
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Models;

namespace Organetto.Core.Users.Data
{
    /// <summary>
    /// Represents an application user with only Firebase authentication information.
    /// </summary>
    public class User : BaseEntity
    {
        public User()
        {
            OwnedBoards = new HashSet<Board>();
            BoardMemberships = new HashSet<BoardMember>();
            Comments = new HashSet<Comment>();
            Attachments = new HashSet<Attachment>();
            Notifications = new HashSet<Notification>();
            FirebaseUid = string.Empty;
            Email = string.Empty;
            Name = string.Empty;
        }

        public string FirebaseUid { get; set; }                         // CHAR(36) unique, from Firebase (уникальный, получен из Firebase)
        public string Email { get; set; }                               // User email (электронная почта пользователя)
        public string Name { get; set; }                                // User display name (имя пользователя)
        public DateTime CreatedAt { get; set; }                         // Account creation timestamp (время создания аккаунта)

        // Navigation properties
        public ICollection<Board> OwnedBoards { get; set; }             // Boards where this user is owner (доски, где пользователь — владелец)
        public ICollection<BoardMember> BoardMemberships { get; set; }   // Member link to boards (связь с участием в досках)
        public ICollection<Comment> Comments { get; set; }               // Comments authored (написанные комментарии)
        public ICollection<Attachment> Attachments { get; set; }         // Attachments uploaded (загруженные вложения)
        public ICollection<Notification> Notifications { get; set; }     // Notifications received (полученные уведомления)
    }
}
