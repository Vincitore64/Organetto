using Organetto.Core.Shared.Exceptions;
using Organetto.Core.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organetto.Core.Boards.Models
{
    ///// <summary>
    ///// Сущность комментария, привязанного к карточке.
    ///// </summary>
    //public class CommentEntity : BaseEntity<long>
    //{
    //    public long CardId { get; private set; }
    //    public long AuthorId { get; private set; }
    //    public string Body { get; private set; }
    //    public DateTime CreatedAt { get; private set; }
    //    public DateTime UpdatedAt { get; private set; }

    //    // Конструктор для EF
    //    private CommentEntity() : base(0) { }

    //    /// <summary>
    //    /// Создаёт новый комментарий для карточки.
    //    /// </summary>
    //    public CommentEntity(long cardId, long authorId, string body) : base(0)
    //    {
    //        if (authorId <= 0)
    //            throw new DomainException("AuthorId must be positive.");
    //        if (string.IsNullOrWhiteSpace(body))
    //            throw new DomainException("Comment body cannot be empty.");

    //        CardId = cardId;
    //        AuthorId = authorId;
    //        Body = body;
    //        CreatedAt = UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Обновляет текст комментария.
    //    /// </summary>
    //    public void UpdateBody(string newBody)
    //    {
    //        if (string.IsNullOrWhiteSpace(newBody))
    //            throw new DomainException("Comment body cannot be empty.");

    //        Body = newBody;
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Отмечает комментарий как удалённый (логическое удаление).
    //    /// </summary>
    //    public void Remove()
    //    {
    //        // В простейшем виде можно считать удалённым изменением тела или флага;
    //        // здесь выбрасываем исключение, если нужно всегда сохранять консистентность.
    //        throw new NotSupportedException("Use repository to delete comment.");
    //    }
    //}
}
