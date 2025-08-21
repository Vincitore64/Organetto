using Organetto.BuildingBlocks.Core.Events.Models;

namespace Organetto.Core.Boards.Events
{
    public record BoardCreatedDomainEvent(long BoardId) : IDomainEvent
    {
    }
}
