using Organetto.BuildingBlocks.Core.Events.Models;

namespace Organetto.BuildingBlocks.Core.Models
{
    public interface IHasDomainEvents
    {
        IReadOnlyList<IDomainEvent> Events { get; }
        void ClearEvents();
    }
}
