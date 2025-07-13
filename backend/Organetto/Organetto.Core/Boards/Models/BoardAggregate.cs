using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Models;

namespace Organetto.Core.Boards.Models
{
    /// <summary>
    /// Aggregate root for a Kanban board, encapsulating lists (columns) and membership.
    /// </summary>
    //public class BoardAggregate : IAggregateRoot<long>
    //{
    //    private readonly List<BoardListEntity> _lists = new();
    //    private readonly List<BoardMemberEntity> _members = new();

    //    public long Id { get; private set; }
    //    public string Title { get; private set; }
    //    public string Description { get; private set; }
    //    public long OwnerId { get; private set; }
    //    public bool IsArchived { get; private set; }
    //    public DateTime CreatedAt { get; private set; }
    //    public DateTime UpdatedAt { get; private set; }

    //    public IReadOnlyCollection<BoardListEntity> Lists => _lists.AsReadOnly();
    //    public IReadOnlyCollection<BoardMemberEntity> Members => _members.AsReadOnly();

    //    // for EF Core
    //    private BoardAggregate() { }

    //    /// <summary>
    //    /// Creates a new Board aggregate.
    //    /// </summary>
    //    public BoardAggregate(string title, long ownerId)
    //    {
    //        Title = title;
    //        Description = string.Empty;
    //        OwnerId = ownerId;
    //        IsArchived = false;
    //        CreatedAt = UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Updates the board description.
    //    /// </summary>
    //    public void UpdateMetadata(string title, string? description)
    //    {
    //        Title = title;
    //        Description = description ?? string.Empty;
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Adds a new list (column) to the board.
    //    /// </summary>
    //    public BoardListEntity AddList(string title, string description, int position)
    //    {
    //        //if (_lists.Any(l => l.Position == position))
    //        //    throw new DomainException("A list at this position already exists.");

    //        var list = new BoardList(title, position);
    //        _lists.Add(list);
    //        UpdatedAt = DateTime.UtcNow;
    //        return list;
    //    }

    //    /// <summary>
    //    /// Removes an existing list (column) from the board.
    //    /// </summary>
    //    public void RemoveList(long listId)
    //    {
    //        var list = _lists.FirstOrDefault(l => l.Id == listId);
    //        if (list == null) return;

    //        _lists.Remove(list);
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Archives the board (soft-delete).
    //    /// </summary>
    //    public void Archive()
    //    {
    //        IsArchived = true;
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Unarchives the board.
    //    /// </summary>
    //    public void Unarchive()
    //    {
    //        IsArchived = false;
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Adds a member to the board.
    //    /// </summary>
    //    public void AddMember(long userId, string role)
    //    {
    //        if (_members.Any(m => m.UserId == userId))
    //            throw new DomainException("User is already a member of this board.");

    //        _members.Add(new BoardMember(userId, role));
    //        UpdatedAt = DateTime.UtcNow;
    //    }

    //    /// <summary>
    //    /// Removes a member from the board.
    //    /// </summary>
    //    public void RemoveMember(long userId)
    //    {
    //        var member = _members.FirstOrDefault(m => m.UserId == userId);
    //        if (member == null) return;

    //        _members.Remove(member);
    //        UpdatedAt = DateTime.UtcNow;
    //    }
    //}
}
