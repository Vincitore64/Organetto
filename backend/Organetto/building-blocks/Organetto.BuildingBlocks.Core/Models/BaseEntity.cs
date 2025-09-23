using Organetto.BuildingBlocks.Core.Events.Models;

namespace Organetto.BuildingBlocks.Core.Models
{
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        private readonly List<IDomainEvent> _events;

        protected BaseEntity(TId id)
        {
            Id = id;
            _events = new List<IDomainEvent>();
        }

        public TId Id { get; set; }

        public IReadOnlyList<IDomainEvent> Events => _events;

        public void ClearEvents() => _events.Clear();

        public override bool Equals(object? obj)
        {
            return obj is not null && obj is BaseEntity<TId> other && Id != null && Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }

        protected void Raise(IDomainEvent e)
        {
            //    Guard against duplicates raised for the same entity instance within
            //    one Unit‑of‑Work.  If duplicates are desired, explicitly clear the list
            //    or use a distinct event payload type.
            if (Events.Any(d => d.GetType() == e.GetType()))
                return;
            _events.Add(e);
        }

    }

    public abstract class BaseEntity : BaseEntity<long>
    {
        public BaseEntity() : base(0)
        {

        }
    }
}
