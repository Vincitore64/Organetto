using Organetto.Core.Shared.Events.Models;

namespace Organetto.Core.Shared.Models
{
    /// <summary>
    /// Base class for entities that need simple CRUD‑style domain events
    /// (EntityName + Created/Updated/Deleted + DomainEvent).
    /// </summary>
    public abstract class CrudEntity<TId> : BaseEntity<TId>
    {
        protected CrudEntity(TId id) : base(id) { }

        /// <summary>
        /// Call when the entity is first persisted.
        /// Tries to raise <c>{EntityName}CreatedDomainEvent</c>.
        /// </summary>
        public void Create()
        {
            TryAddDomainEvent("Created");
        }

        /// <summary>
        /// Call after the entity has been modified.
        /// Tries to raise <c>{EntityName}UpdatedDomainEvent</c>.
        /// </summary>
        public void Update()
        {
            TryAddDomainEvent("Updated");
        }

        /// <summary>
        /// Call before the entity is removed.
        /// Tries to raise <c>{EntityName}DeletedDomainEvent</c>.
        /// </summary>
        public void Delete()
        {
            TryAddDomainEvent("Deleted");
        }

        /// <summary>
        /// Creates and records a domain event whose type follows the convention
        /// {EntityName}{operation}DomainEvent. If the type does not exist in the
        /// same assembly as the entity, the call is silently ignored.
        /// </summary>
        private void TryAddDomainEvent(string operation)
        {
            var entityType = GetType();
            var expectedTypeName = $"{entityType.Name}{operation}DomainEvent";
            var eventType = entityType.Assembly.GetType(expectedTypeName);

            if (eventType == null)
                return; // Convention type not found – nothing to do

            object? instance;

            // Prefer ctor that takes the entity itself; fall back to parameter‑less
            if (eventType.GetConstructor(new[] { entityType }) != null)
            {
                instance = Activator.CreateInstance(eventType, this);
            }
            else
            {
                instance = Activator.CreateInstance(eventType);
            }

            if (instance is IDomainEvent domainEvent)
            {
                Raise(domainEvent);
            }
            // If the instance does not implement IDomainEvent, swallow.
        }
    }
}
