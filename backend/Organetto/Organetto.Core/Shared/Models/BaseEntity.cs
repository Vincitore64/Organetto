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

        protected void Raise(IDomainEvent e)
        {
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
