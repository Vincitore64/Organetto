using Organetto.Core.Shared.Events.Models;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Organetto.Core.Shared.Models
{
    public abstract class CrudEntity : CrudEntity<long>
    {
        public CrudEntity() : base(0)
        {
            
        }
    }

    /// <summary>
    /// Base class for entities that need simple CRUD‑style domain events following the
    /// convention "{EntityName}{Created|Updated|Deleted}DomainEvent".
    ///
    /// Optimisations:
    /// * Reflection results (constructor delegates) are cached per <c>(EntityType, Operation)</c>.
    /// * Missing event types are <b>not</b> cached to prevent unbounded growth.
    /// * Enum → string allocations are avoided by mapping <see cref="Operation"/> to a static
    ///   string array.
    /// * Constructor matching is relaxed: an event constructor parameter may be any base type
    ///   that the concrete entity instance derives from (e.g. <c>BaseEntity</c>), not only the
    ///   exact entity type.  This supports shared event classes.
    /// * Duplicate domain events of the same <see cref="Type"/> are ignored by default to
    ///   prevent raising multiple identical events within a single Unit‑of‑Work.  Callers that
    ///   require duplicates can explicitly clear existing events before marking again.
    /// </summary>
    public abstract class CrudEntity<TId> : BaseEntity<TId>, ICrudEntity<TId>
    {
        protected CrudEntity(TId id) : base(id) { }

        #region Public API ─────────────────────────────────────────────────────

        /// <summary>Records a <c>{EntityName}CreatedDomainEvent</c> if present.</summary>
        public void MarkCreated() => TryAddDomainEvent(Operation.Created);

        /// <summary>Records a <c>{EntityName}UpdatedDomainEvent</c> if present.</summary>
        public void MarkUpdated() => TryAddDomainEvent(Operation.Updated);

        /// <summary>Records a <c>{EntityName}DeletedDomainEvent</c> if present.</summary>
        public void MarkDeleted() => TryAddDomainEvent(Operation.Deleted);

        #endregion

        #region Infrastructure ────────────────────────────────────────────────

        private enum Operation : byte { Created = 0, Updated = 1, Deleted = 2 }
        private static readonly string[] _operationNames = { "Created", "Updated", "Deleted" };
        private const int MaxCacheSize = 1000; // soft cap; tweak if necessary

        // Cache maps (ConcreteEntityType, Operation) → compiled factory delegate.
        private static readonly ConcurrentDictionary<(Type, Operation), Func<object, IDomainEvent>> _factoryCache = new();
        private static readonly List<Type> _assemblyTypes = new();

        private void TryAddDomainEvent(Operation op)
        {
            var key = (GetType(), op);

            // 1. Fast path – factory already cached
            if (!_factoryCache.TryGetValue(key, out var factory))
            {
                // 2. Build factory; only cache successful look‑ups
                factory = BuildFactory(key.Item1, key.Item2);

                if (factory is null)
                    return; // No matching event type / ctor – nothing to do

                if (_factoryCache.Count < MaxCacheSize)
                    _factoryCache.TryAdd(key, factory); // ignore race if over cap
            }

            // 3. Execute factory
            var domainEvent = factory(this);
            if (domainEvent is null)
                return;

            Raise(domainEvent);
        }

        private static Type? GetEventType(Type entityType, Operation op)
        {
            var suffix = _operationNames[(int)op];
            //var expectedTypeName = $"{entityType.Namespace}.{entityType.Name}{suffix}DomainEvent";
            var expectedTypeName = $"{entityType.Name}{suffix}DomainEvent";
            if (_assemblyTypes.Count == 0)
            {
                _assemblyTypes.AddRange(entityType.Assembly.GetTypes());
            }
            return _assemblyTypes.FirstOrDefault(t => t.Name == expectedTypeName);
        }

        /// <summary>
        /// Builds a delegate that constructs the event type matching the naming convention.
        /// Returns <c>null</c> when the type or a suitable constructor is missing.
        /// </summary>
        private static Func<object, IDomainEvent>? BuildFactory(Type entityType, Operation op)
        {
            var eventType = GetEventType(entityType, op);

            if (eventType is null || !typeof(IDomainEvent).IsAssignableFrom(eventType))
                return null;

            // Search for a constructor with single parameter that accepts the entity instance
            var ctorWithEntity = eventType.GetConstructors()
                                          .Select(c => new { Ctor = c, Params = c.GetParameters() })
                                          .FirstOrDefault(x => x.Params.Length == 1 &&
                                                               x.Params[0].ParameterType.IsAssignableFrom(entityType))
                                          ?.Ctor;

            if (ctorWithEntity is not null)
            {
                var paramType = ctorWithEntity.GetParameters()[0].ParameterType;
                var entityParam = Expression.Parameter(typeof(object), "e");
                var castParam = Expression.Convert(entityParam, paramType);
                var newExpr = Expression.New(ctorWithEntity, castParam);
                var body = Expression.Convert(newExpr, typeof(IDomainEvent));
                return Expression.Lambda<Func<object, IDomainEvent>>(body, entityParam).Compile();
            }

            // Fallback: parameter‑less ctor – new Event()
            var defaultCtor = eventType.GetConstructor(Type.EmptyTypes);
            if (defaultCtor is not null)
            {
                var ignoreParam = Expression.Parameter(typeof(object), "_");
                var newExpr = Expression.New(defaultCtor);
                var body = Expression.Convert(newExpr, typeof(IDomainEvent));
                return Expression.Lambda<Func<object, IDomainEvent>>(body, ignoreParam).Compile();
            }

            return null; // No usable constructor – skip
        }

        #endregion
    }
}
