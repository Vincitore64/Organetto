using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Shared.Events.Models;
using Organetto.Core.Shared.Extensions;

namespace Organetto.Core.Boards.Cards.Events
{

    public record CardCreatedDomainEvent(long BoardId, long BoardListId, long EntityId) : IDomainEvent
    {
        public CardCreatedDomainEvent(Card card) :
            this(card.BoardList.ThrowIfNull(new ArgumentException("For creating CardCreatedDomainEvent BoardList cannot be null")).BoardId, card.BoardListId, card.Id)
        {
        }
    }

    public record CardUpdatedDomainEvent(long BoardId, long BoardListId, long EntityId) : IDomainEvent
    {
        public CardUpdatedDomainEvent(Card card) :
            this(card.BoardList.ThrowIfNull(new ArgumentException("For creating CardUpdatedDomainEvent BoardList cannot be null")).BoardId, card.BoardListId, card.Id)
        {
        }
    }

    public record CardDeletedDomainEvent(long BoardId, long BoardListId, long EntityId) : IDomainEvent
    {
        public CardDeletedDomainEvent(Card card) :
            this(card.BoardList.ThrowIfNull(new ArgumentException("For creating CardDeletedDomainEvent BoardList cannot be null")).BoardId, card.BoardListId, card.Id)
        {
        }
    }
}
