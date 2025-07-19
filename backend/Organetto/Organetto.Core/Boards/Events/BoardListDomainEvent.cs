using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Events.Models;

namespace Organetto.Core.Boards.Events
{
    public record BoardListCreatedDomainEvent(long BoardId, long BoardListId) : IDomainEvent
    {
        public BoardListCreatedDomainEvent(BoardList boardList) : this(boardList.BoardId, boardList.Id)
        {
        }
    }

    public record BoardListUpdatedDomainEvent(long BoardId, long BoardListId) : IDomainEvent
    {
        public BoardListUpdatedDomainEvent(BoardList boardList) : this(boardList.BoardId, boardList.Id)
        {
        }
    }

    public record BoardListDeletedDomainEvent(long BoardId, long EntityId) : IDomainEvent
    {
        public BoardListDeletedDomainEvent(BoardList boardList) : this(boardList.BoardId, boardList.Id)
        {
        }
    }
}
