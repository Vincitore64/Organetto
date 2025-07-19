using Organetto.Core.Shared.Events.Models;

namespace Organetto.Core.Shared.Models
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
