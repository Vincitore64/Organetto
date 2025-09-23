Clean-architecture projects usually surface **domain events through three cooperating abstractions** rather than a single “magic” class:

1. **A minimal `IDomainEvent` interface and immutable event classes that live in the Domain layer** (Core).
2. **Entities (or, if you prefer, command decorators or “factories”) that *create* those events**.
3. **An `IEventDispatcher` (or `IEventPublisher`) that *publishes* every event it receives after the unit of work commits**.

Whether the *creator* is the entity itself or an external helper depends on where you want the rule to live—but in Clean Architecture the pattern is always the same: *collect the events first, then dispatch them once per transaction*. This keeps rules explicit, respects the dependency rule, and avoids tight coupling between your command handlers and side-effects ([Microsoft Learn][1], [lostechies.com][2]).

---

## 1  Define the cross-cutting abstractions once

### `IDomainEvent`

```csharp
public interface IDomainEvent { DateTime OccurredOn { get; } }
```

*Immutability and past-tense naming are key* ([milanjovanovic.tech][3], [nbottarini.com][4]).

### `IHasDomainEvents`

```csharp
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent e);
    void ClearDomainEvents();
}
```

Ardalis’ Clean Architecture template and Jimmy Bogard’s “better domain events” pattern both follow this approach, storing events on the entity until the DbContext publishes them ([GitHub][5], [lostechies.com][2]).

### `IEventDispatcher`

```csharp
public interface IEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct);
}
```

The dispatcher belongs in the Application layer; its concrete implementation (MediatR, channel, message bus, etc.) sits in Infrastructure so Core stays framework-agnostic ([Microsoft Learn][1]).

---

## 2  Choose *where* events are **created**

### Option A – Entities raise events themselves

Add a protected `RaiseDomainEvent` method in a base `Entity` class; call it from rich domain methods such as `Order.Complete()` ([milanjovanovic.tech][3], [nbottarini.com][4]).
*Pros:* aligns with ubiquitous language; no extra plumbing.
*Cons:* entities need just enough state to decide when to fire.

### Option B – **Command-specific “event factories”**

If your entities are anemic or you don’t want domain logic inside them, inject an `IDomainEventFactory<TCommand,TEntity>` into a CQRS decorator:

```csharp
public interface IDomainEventFactory<TCommand,TEntity>
{
     IEnumerable<IDomainEvent> Create(TCommand cmd, TEntity updated);
}
```

Each command gets a factory that knows how to translate “update X” into one or many events; the decorator calls the factory after mapping the updates, adds the returned events to the entity, then saves — exactly the refactor you sketched earlier. This keeps the rule close to the use-case while the entity remains a POCO ([Stack Overflow][6], [Medium][7]).

### Option C – Domain services

Some teams expose a domain-service method (e.g., `PaymentDomainService.ProcessRefund(...)`) that both mutates aggregates and returns a `RefundProcessed` event. The handler receives the event and persists it. This is essentially a specialized factory but can coordinate multiple aggregates when a single entity isn’t the right place ([Reddit][8]).

---

## 3  Publish once per transaction

Collect every changed entity’s events inside your EF Core `DbContext`:

```csharp
public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
{
    var result = await base.SaveChangesAsync(ct);

    var domainEvents = ChangeTracker.Entries<IHasDomainEvents>()
        .SelectMany(e => e.Entity.DomainEvents)
        .ToList();

    foreach (var entity in ChangeTracker.Entries<IHasDomainEvents>())
        entity.Entity.ClearDomainEvents();

    await _dispatcher.DispatchAsync(domainEvents, ct);
    return result;
}
```

Microsoft’s microservices guide, Milan Jovanović’s article, and many production templates recommend dispatching **after** `SaveChangesAsync` and pairing it with the Outbox pattern for reliability ([Microsoft Learn][1], [milanjovanovic.tech][3], [Stack Overflow][9]).

---

## 4  Practical tips & pitfalls

| Concern                     | Recommendation                                                                                                                                                               |
| --------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Testing**                 | Because events are only *recorded*, unit tests assert on the entity’s `DomainEvents` collection instead of spying on a static dispatcher ([lostechies.com][2])               |
| **Granularity**             | Keep events business-centric (`CourseCompleted`) rather than technical (`EntityUpdated`) for clearer intent — even in Clean Architecture ([nbottarini.com][4], [Medium][10]) |
| **When no event is needed** | Don’t fire one! Clean Architecture is pragmatic; raise events only when another part of the app genuinely needs to react ([Reddit][8])                                       |
| **Integration events**      | Translate domain events to integration events in a handler, not in the entity, so you don’t leak external concerns into Core ([Microsoft Learn][1])                          |
| **Framework choice**        | MediatR is convenient but optional; any dispatcher that satisfies `IEventDispatcher` will work, so your Clean Architecture remains framework-agnostic ([Medium][7])          |

---

## 5  Putting it together

```csharp
// Application layer decorator (simplified)
public class DomainEventsDecorator<TCmd,TEnt,TDto,TKey>
    : IRequestHandler<TCmd,TDto>
    where TCmd : IRequest<TDto>, IHasId<TKey>
    where TEnt : class, IHasDomainEvents
{
    private readonly IRequestHandler<TCmd,TDto> _inner;
    private readonly IReadRepo<TEnt,TKey> _repo;
    private readonly IMapper _map;
    private readonly IDomainEventFactory<TCmd,TEnt> _factory;
    public DomainEventsDecorator(...){ ... }

    public async Task<TDto> Handle(TCmd cmd, CancellationToken ct)
    {
        var ent = await _repo.GetByIdAsync(cmd.Id, ct);
        _map.Map(cmd, ent);
        foreach (var ev in _factory.Create(cmd, ent))
            ent.AddDomainEvent(ev);

        return await _inner.Handle(cmd, ct); // inner persists
    }
}
```

Register the decorator with Scrutor (or manually) so every update command automatically gains event capability without duplicating code ([GitHub][5]).

---

### Bottom line

In Clean Architecture you **don’t need heavyweight DDD aggregates to use domain events**.
All you need is:

* A tiny **event interface** and an **entity mix-in** to store them,
* Either the entity itself **or** a pluggable **factory** to create events,
* And an **event dispatcher** invoked once per `SaveChangesAsync`.

With those three abstractions your handlers stay slim, your business rules stay explicit, and your system stays decoupled—no matter which framework you plug in underneath.

[1]: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation "Domain events: Design and implementation - .NET | Microsoft Learn"
[2]: https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/ "
    
      A better domain events pattern · Los Techies
    
  "
[3]: https://www.milanjovanovic.tech/blog/how-to-use-domain-events-to-build-loosely-coupled-systems "How To Use Domain Events To Build Loosely Coupled Systems"
[4]: https://nbottarini.com/en/posts/2021/12/16/domain_events_guide/ "Domain Events guide - Nicolás Bottarini Blog"
[5]: https://github.com/ardalis/CleanArchitecture "GitHub - ardalis/CleanArchitecture: Clean Architecture Solution Template: A proven Clean Architecture Template for ASP.NET Core 9"
[6]: https://stackoverflow.com/questions/30625363/implementing-domain-event-handler-pattern-in-c-sharp-with-simple-injector "generics - Implementing Domain Event Handler pattern in C# with Simple Injector - Stack Overflow"
[7]: https://medium.com/unil-ci-software-engineering/be-careful-with-domain-events-2ef8866f6cd6 "Be careful with Domain Events. Many examples of Domain-Driven Design… | by George | Technical blog from UNIL engineering teams | Medium"
[8]: https://www.reddit.com/r/dotnet/comments/1auqr51/clean_architecture_onion_architecture_domain/ "Clean Architecture, Onion Architecture, Domain Events and Outbox Pattern Question : r/dotnet"
[9]: https://stackoverflow.com/questions/51864332/serialize-and-deserialize-domain-events-to-persist-and-retrieve-from-event-store "c# - Serialize and Deserialize domain events to persist and retrieve from Event Store in generic implementation - Stack Overflow"
[10]: https://medium.com/c-sharp-programming/domain-events-2782ff03154d "Domain Events. Basic,  MediatR & CQRS | by Ben Witt | .Net Programming | Medium"
