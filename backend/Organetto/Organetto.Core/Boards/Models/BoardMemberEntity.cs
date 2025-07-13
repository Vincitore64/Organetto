using Organetto.Core.Shared.Exceptions;
using Organetto.Core.Shared.Models;

namespace Organetto.Core.Boards.Models
{
    ///// <summary>
    ///// Представляет участника доски с его ролью внутри агрегата Board.
    ///// </summary>
    //public class BoardMemberEntity : BaseEntity<long>
    //{
    //    /// <summary>
    //    /// Идентификатор доски, к которой относится участник.
    //    /// </summary>
    //    public long BoardId { get; private set; }

    //    /// <summary>
    //    /// Идентификатор пользователя.
    //    /// </summary>
    //    public long UserId { get; private set; }

    //    /// <summary>
    //    /// Роль на доске (например, "Owner", "Admin", "Member").
    //    /// </summary>
    //    public string Role { get; private set; }

    //    // Для EF Core
    //    private BoardMemberEntity() : base(0) { }

    //    /// <summary>
    //    /// Создаёт нового участника доски.
    //    /// </summary>
    //    /// <param name="boardId">ID доски.</param>
    //    /// <param name="userId">ID пользователя.</param>
    //    /// <param name="role">Роль участника.</param>
    //    public BoardMemberEntity(long boardId, long userId, string role) : base(0)
    //    {
    //        if (userId <= 0)
    //            throw new DomainException("UserId must be positive.");
    //        if (string.IsNullOrWhiteSpace(role))
    //            throw new DomainException("Role cannot be empty.");

    //        BoardId = boardId;
    //        UserId = userId;
    //        Role = role;
    //    }

    //    /// <summary>
    //    /// Изменяет роль участника на доске.
    //    /// </summary>
    //    /// <param name="newRole">Новая роль.</param>
    //    public void ChangeRole(string newRole)
    //    {
    //        if (string.IsNullOrWhiteSpace(newRole))
    //            throw new DomainException("Role cannot be empty.");
    //        if (Role.Equals(newRole, StringComparison.OrdinalIgnoreCase))
    //            return;

    //        Role = newRole;
    //    }
    //}
}
