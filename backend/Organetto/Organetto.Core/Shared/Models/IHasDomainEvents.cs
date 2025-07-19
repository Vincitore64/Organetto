using Organetto.Core.Shared.Events.Models;

namespace Organetto.Core.Shared.Models
{
    public interface IHasDomainEvents
    {
        IReadOnlyList<IDomainEvent> Events { get; }
        void ClearEvents();
    }
}
