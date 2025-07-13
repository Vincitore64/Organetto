using Organetto.Core.Shared.Events.Models;

namespace Organetto.Core.Shared.Models
{
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        protected BaseEntity(TId id)
        {
            Id = id;
            Events = new List<IDomainEvent>();
        }

        public TId Id { get; set; }

        public List<IDomainEvent> Events { get; }

    }

    public abstract class BaseEntity : BaseEntity<long>
    {
        public BaseEntity() : base(0)
        {
            
        }
    }
}
