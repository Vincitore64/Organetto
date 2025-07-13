using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Shared.Exceptions;
using Organetto.Core.Shared.Models;

namespace Organetto.Core.Boards.Models
{
    ///// <summary>
    ///// Задача/карточка в рамках колонки BoardList.
    ///// </summary>
    //public class CardEntity : BaseEntity<long>
    //{
    //    private readonly List<Comment> _comments = new();
    //    private readonly List<Attachment> _attachments = new();
    //    private readonly List<DueDate> _dueDates = new();

    //    public long BoardListId { get; private set; }
    //    public string Title { get; private set; }
    //    public string Description { get; private set; }
    //    public int Position { get; private set; }
    //    public DateTime CreatedAt { get; private set; }
    //    public DateTime UpdatedAt { get; private set; }

    //    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
    //    public IReadOnlyCollection<Attachment> Attachments => _attachments.AsReadOnly();
    //    public IReadOnlyCollection<DueDate> DueDates => _dueDates.AsReadOnly();

    //    // Для EF Core
    //    private CardEntity() : base(0) { }

    //    /// <summary>
    //    /// Создаёт новую карточку.
    //    /// </summary>
    //    public CardEntity(long boardListId, string title, string description, int position) : base(0)
    //    {
    //        if (string.IsNullOrWhiteSpace(title))
    //            throw new DomainException("Card title cannot be empty.");

    //        BoardListId = boardListId;
    //        Title = title;
    //        Description = description ?? string.Empty;
    //        Position = position;
    //        CreatedAt = UpdatedAt = DateTime.UtcNow;
    //    }

    //    // Unified metadata update
    //    public void UpdateMetadata(string title, string description, int position)
    //    {
    //        if (string.IsNullOrWhiteSpace(title))
    //            throw new DomainException("Card title cannot be empty.");
    //        if (position < 0)
    //            throw new DomainException("Position must be non-negative.");
    //        Title = title;
    //        Description = description ?? string.Empty;
    //        Position = position;
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Добавляет комментарий к карточке.
    //    /// </summary>
    //    public Comment AddComment(long authorId, string body)
    //    {
    //        if (string.IsNullOrWhiteSpace(body))
    //            throw new DomainException("Comment body cannot be empty.");

    //        var comment = new Comment(Id, authorId, body);
    //        _comments.Add(comment);
    //        UpdatedAt = DateTime.UtcNow;
    //        return comment;
    //    }

    //    /// <summary>
    //    /// Удаляет комментарий.
    //    /// </summary>
    //    public void RemoveComment(long commentId)
    //    {
    //        var comment = _comments.FirstOrDefault(c => c.Id == commentId);
    //        if (comment == null)
    //            throw new DomainException($"Comment {commentId} not found.");

    //        _comments.Remove(comment);
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Добавляет вложение.
    //    /// </summary>
    //    public Attachment AddAttachment(long uploaderId, string fileUrl, string filename)
    //    {
    //        if (string.IsNullOrWhiteSpace(fileUrl) || string.IsNullOrWhiteSpace(filename))
    //            throw new DomainException("Attachment URL and filename cannot be empty.");

    //        var att = new Attachment(Id, uploaderId, fileUrl, filename);
    //        _attachments.Add(att);
    //        UpdatedAt = DateTime.UtcNow;
    //        return att;
    //    }

    //    /// <summary>
    //    /// Удаляет вложение.
    //    /// </summary>
    //    public void RemoveAttachment(long attachmentId)
    //    {
    //        var att = _attachments.FirstOrDefault(a => a.Id == attachmentId);
    //        if (att == null)
    //            throw new DomainException($"Attachment {attachmentId} not found.");

    //        _attachments.Remove(att);
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Устанавливает дедлайн для карточки.
    //    /// </summary>
    //    public DueDate SetDueDate(DateTime dueAt)
    //    {
    //        if (dueAt < CreatedAt)
    //            throw new DomainException("Due date cannot be earlier than creation date.");

    //        var dd = new DueDate(Id, dueAt);
    //        _dueDates.Add(dd);
    //        UpdatedAt = DateTime.UtcNow;
    //        return dd;
    //    }

    //    /// <summary>
    //    /// Отмечает дедлайн выполненным.
    //    /// </summary>
    //    public void CompleteDueDate(long dueDateId)
    //    {
    //        var dd = _dueDates.FirstOrDefault(d => d.Id == dueDateId);
    //        if (dd == null)
    //            throw new DomainException($"DueDate {dueDateId} not found.");

    //        dd.MarkComplete();
    //        UpdatedAt = DateTime.UtcNow;
    //    }
    //}
}
