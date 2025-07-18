using Organetto.Core.Shared.Events.Models;

namespace Organetto.Core.Shared.Models
{
    public interface IEntity<TId> : IHasId<TId>
    {
        IReadOnlyList<IDomainEvent> Events { get; }
    }
}
