using Organetto.Core.Users.Data;

namespace Organetto.Core.Boards.Data
{
    /// <summary>
    /// Junction table representing membership of users in boards, with a role.
    /// </summary>
    public class BoardMember
    {
        public BoardMember()
        {
            Role = string.Empty;
        }

        public long Id { get; set; }                                    // Surrogate PK (суррогатный первичный ключ)
        public long BoardId { get; set; }                               // FK to Board (внешний ключ к Board)
        public long UserId { get; set; }                                // FK to User (внешний ключ к User)
        public string Role { get; set; }                                // Role in board, e.g. “member”, “admin” (роль на доске, напр. “member”, “admin”)

        // Navigation properties
        public Board? Board { get; set; }                                 // Board navigation (связь с доской)
        public User? User { get; set; }                                   // User navigation (связь с пользователем)
    }
}
