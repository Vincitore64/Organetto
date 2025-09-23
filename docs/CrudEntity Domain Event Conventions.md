# CrudEntity Domain Event Conventions

## Overview

The `CrudEntity<TId>` class provides a convention-based approach for automatically raising domain events for common CRUD operations (Create, Update, Delete). This system eliminates boilerplate code by using reflection to discover and instantiate domain events that follow specific naming conventions.

## CrudEntity Base Class

The `CrudEntity<TId>` abstract class extends `BaseEntity<TId>` and provides three methods that automatically raise domain events:

```csharp
public abstract class CrudEntity<TId> : BaseEntity<TId>
{
    public void Create()    // Raises {EntityName}CreatedDomainEvent
    public void Update()    // Raises {EntityName}UpdatedDomainEvent
    public void Delete()    // Raises {EntityName}DeletedDomainEvent
}
```

## Domain Event Naming Convention

For the automatic event discovery to work, domain events must follow this exact naming pattern:

```
{EntityName}{Operation}DomainEvent
```

### Examples

If you have an entity called `Board`, the expected domain event names would be:
- `BoardCreatedDomainEvent`
- `BoardUpdatedDomainEvent`
- `BoardDeletedDomainEvent`

If you have an entity called `TaskItem`, the expected domain event names would be:
- `TaskItemCreatedDomainEvent`
- `TaskItemUpdatedDomainEvent`
- `TaskItemDeletedDomainEvent`

## Constructor Requirements

**IMPORTANT**: Domain events used with `CrudEntity` must have specific constructor signatures for automatic instantiation to work.

### Preferred Constructor Pattern

The system **prefers** constructors that take the entity itself as a parameter:

```csharp
public record BoardCreatedDomainEvent : IDomainEvent
{
    public BoardCreatedDomainEvent(Board board)
    {
        BoardId = board.Id;
        BoardName = board.Name;
        OwnerId = board.OwnerId;
        OccurredOn = DateTime.UtcNow;
    }
    
    public long BoardId { get; init; }
    public string BoardName { get; init; }
    public long OwnerId { get; init; }
    public DateTime OccurredOn { get; init; }
}
```

### Fallback Constructor Pattern

If no entity-parameter constructor is found, the system falls back to a parameterless constructor:

```csharp
public record BoardDeletedDomainEvent : IDomainEvent
{
    public BoardDeletedDomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
    }
    
    public DateTime OccurredOn { get; init; }
}
```

**Note**: Parameterless constructors are less useful since they cannot capture entity state at the time of the event.

## Implementation Details

### Automatic Event Discovery

The `TryAddDomainEvent` method uses reflection to:

1. **Build the expected type name**: `{EntityType.Name}{Operation}DomainEvent`
2. **Search the entity's assembly** for a type with that exact name
3. **Attempt instantiation** using the preferred constructor pattern
4. **Add the event** to the entity's event collection if successful

```csharp
private void TryAddDomainEvent(string operation)
{
    var entityType = GetType();
    var expectedTypeName = $"{entityType.Name}{operation}DomainEvent";
    var eventType = entityType.Assembly.GetType(expectedTypeName);

    if (eventType == null)
        return; // Convention type not found – nothing to do

    object? instance;

    // Prefer ctor that takes the entity itself; fall back to parameterless
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
}
```

### Silent Failure Behavior

The system is designed to fail silently in these scenarios:
- **Event type not found**: If no matching domain event class exists
- **Constructor not found**: If neither preferred nor fallback constructor exists
- **Invalid event type**: If the instantiated object doesn't implement `IDomainEvent`

This allows entities to use `CrudEntity` even if they don't need all three event types.

## Usage Examples

### Entity Implementation

```csharp
public class Board : CrudEntity<long>
{
    public Board(long id, string name, long ownerId) : base(id)
    {
        Name = name;
        OwnerId = ownerId;
    }
    
    public string Name { get; set; }
    public long OwnerId { get; set; }
    
    // Business methods that trigger events
    public void CreateBoard()
    {
        // Business logic here
        Create(); // Raises BoardCreatedDomainEvent
    }
    
    public void UpdateMetadata(string newName)
    {
        Name = newName;
        Update(); // Raises BoardUpdatedDomainEvent
    }
    
    public void RemoveBoard()
    {
        // Business logic here
        Delete(); // Raises BoardDeletedDomainEvent
    }
}
```

### Domain Event Implementation

```csharp
// Preferred: Constructor with entity parameter
public record BoardCreatedDomainEvent : IDomainEvent
{
    public BoardCreatedDomainEvent(Board board)
    {
        BoardId = board.Id;
        BoardName = board.Name;
        OwnerId = board.OwnerId;
        OccurredOn = DateTime.UtcNow;
    }
    
    public long BoardId { get; init; }
    public string BoardName { get; init; }
    public long OwnerId { get; init; }
    public DateTime OccurredOn { get; init; }
}

public record BoardUpdatedDomainEvent : IDomainEvent
{
    public BoardUpdatedDomainEvent(Board board)
    {
        BoardId = board.Id;
        UpdatedName = board.Name;
        OccurredOn = DateTime.UtcNow;
    }
    
    public long BoardId { get; init; }
    public string UpdatedName { get; init; }
    public DateTime OccurredOn { get; init; }
}

// Fallback: Parameterless constructor
public record BoardDeletedDomainEvent : IDomainEvent
{
    public BoardDeletedDomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
    }
    
    public DateTime OccurredOn { get; init; }
}
```

## Best Practices

### 1. Always Prefer Entity Constructor

**DO**: Create constructors that accept the entity to capture relevant state
```csharp
public TaskCreatedDomainEvent(TaskItem task)
{
    TaskId = task.Id;
    Title = task.Title;
    AssigneeId = task.AssigneeId;
    // ... capture relevant state
}
```

**DON'T**: Rely on parameterless constructors unless absolutely necessary
```csharp
public TaskCreatedDomainEvent() // Limited usefulness
{
    OccurredOn = DateTime.UtcNow;
}
```

### 2. Implement IDomainEvent

All domain events must implement the `IDomainEvent` interface:

```csharp
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
```

### 3. Use Record Types

Prefer `record` types for immutable domain events:

```csharp
public record UserRegisteredDomainEvent(User user) : IDomainEvent
{
    public long UserId { get; init; } = user.Id;
    public string Email { get; init; } = user.Email;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
```

### 4. Assembly Placement

Ensure domain events are in the **same assembly** as the entity for automatic discovery to work. Typically this means placing them in the `Organetto.Core` project.

### 5. Naming Consistency

Be precise with entity names in the convention:
- Entity: `TaskItem` → Events: `TaskItemCreatedDomainEvent`, `TaskItemUpdatedDomainEvent`
- Entity: `BoardColumn` → Events: `BoardColumnCreatedDomainEvent`, `BoardColumnUpdatedDomainEvent`

## Integration with Event Processing

The `CrudEntity` system works seamlessly with the existing event processing pipeline:

1. **Entity calls** `Create()`, `Update()`, or `Delete()`
2. **CrudEntity** automatically discovers and instantiates the appropriate domain event
3. **BaseEntity** stores the event in its internal collection
4. **DbContext** publishes all collected events after `SaveChangesAsync()`
5. **Event handlers** process the domain events
6. **Integration events** are created and published to external systems

This maintains the clean separation between domain logic and infrastructure concerns while providing a convenient convention-based approach for common CRUD scenarios.

## Troubleshooting

### Event Not Raised

**Check**:
1. Domain event class exists with exact naming convention
2. Domain event is in the same assembly as the entity
3. Domain event implements `IDomainEvent`
4. Domain event has either entity constructor or parameterless constructor

### Constructor Not Found

**Ensure**:
1. Constructor parameter type exactly matches the entity type
2. If using inheritance, the constructor parameter should match the concrete entity type, not the base type

### Event Properties Empty

**Verify**:
1. Using entity constructor (not parameterless)
2. Constructor properly assigns entity properties to event properties
3. Entity state is set before calling `Create()`, `Update()`, or `Delete()`

## Related Documentation

- [Event Naming Convention](./Event%20Naming%20Convention.md) - Integration event mapping conventions
- [Domain events creation](./Domain%20events%20creation.md) - General domain event patterns and best practices