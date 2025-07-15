# Event Naming Convention

## Overview

Organetto implements a convention-based approach for mapping domain events to integration events. This system automatically discovers and maps domain events to their corresponding integration events based on naming patterns, eliminating the need for manual mapping configuration.

## Naming Convention

### Domain Events

Domain events should follow this naming pattern:

```
{EntityName}{Action}DomainEvent
```

**Examples:**
- `BoardCreatedDomainEvent`
- `BoardMetadataUpdatedDomainEvent`
- `BoardDeletedDomainEvent`
- `ColumnCreatedDomainEvent`
- `ColumnMetadataUpdatedDomainEvent`

**Requirements:**
- Must implement `IDomainEvent` interface
- Must be concrete classes (not abstract)
- Must end with "DomainEvent" suffix
- Should be located in the `Organetto.Core` project under appropriate domain folders
- Should use PascalCase naming convention

### Integration Events

Integration events should follow this naming pattern:

```
{EntityName}{Action}IntegrationEvent
```

**Examples:**
- `BoardCreatedIntegrationEvent`
- `BoardMetadataUpdatedIntegrationEvent`
- `BoardDeletedIntegrationEvent`
- `ColumnCreatedIntegrationEvent`
- `ColumnMetadataUpdatedIntegrationEvent`

**Requirements:**
- Must implement `IIntegrationEvent` interface
- Must be concrete classes (not abstract)
- Must end with "IntegrationEvent" suffix
- Should be located in the `Organetto.UseCases` project under appropriate feature folders
- Should use PascalCase naming convention
- Can optionally implement `IEntityIntegrationEvent<TEntityId>` for entity-specific events

## Automatic Mapping Logic

The system uses convention-based mapping implemented in `ConventionEventMappingExtensions.cs`:

### Mapping Algorithm

1. **Discovery Phase**: The system scans assemblies to find all concrete domain event types that implement `IDomainEvent`

2. **Name Transformation**: For each domain event, it applies this transformation:
   ```csharp
   var ieTypeName = deType.Name.Replace("DomainEvent", string.Empty) + "IntegrationEvent";
   ```

3. **Type Resolution**: It searches for integration event types that match the transformed name

4. **AutoMapper Configuration**: Creates mapping configuration between matching domain and integration events

### Example Transformation

| Domain Event | Integration Event |
|--------------|------------------|
| `BoardCreatedDomainEvent` | `BoardCreatedIntegrationEvent` |
| `ColumnMetadataUpdatedDomainEvent` | `ColumnMetadataUpdatedIntegrationEvent` |
| `UserRegisteredDomainEvent` | `UserRegisteredIntegrationEvent` |

## Implementation Details

### Domain Event Structure

```csharp
using Organetto.Core.Shared.Events.Models;

namespace Organetto.Core.Boards.Events
{
    public record BoardCreatedDomainEvent(long BoardId) : IDomainEvent
    {
    }
}
```

### Integration Event Structure

```csharp
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.IntegrationEvents
{
    public record BoardCreatedIntegrationEvent(long BoardId) : IntegrationEvent;
}
```

### Entity-Specific Integration Events

For events that carry entity information, implement `IEntityIntegrationEvent<TEntityId>`:

```csharp
public record BoardMetadataUpdatedIntegrationEvent(long EntityId, long OwnerId, long[] MemberIds) 
    : IntegrationEvent, IEntityIntegrationEvent<long>;
```

## Configuration

The convention-based mapping is configured in `ConventionEventMappingProfile.cs`:

```csharp
public class ConventionEventMappingProfile : Profile
{
    public ConventionEventMappingProfile()
    {
        // Automatically discovers and maps domain events to integration events
        var domainEventType = typeof(IDomainEvent);
        var asm = domainEventType.Assembly;

        var domainEventTypes = asm
            .GetTypes()
            .Where(t => domainEventType.IsAssignableFrom(t)
                     && t.IsClass
                     && !t.IsAbstract)
            .ToArray();

        foreach (var deType in domainEventTypes)
        {
            var ieType = deType.GetIntegrationEventType();
            if (ieType == null) return;
            CreateMap(deType, ieType);
        }
    }
}
```

## Usage in Application

The mapping is automatically applied when domain events are processed:

```csharp
public class ConventionEventsMapper : IEventsMapper
{
    public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(de =>
        {
            var ieType = de.GetIntegrationEventType();
            var ieObj = _mapper.Map(de, de.GetType(), ieType);
            return ieObj;
        }).OfType<IIntegrationEvent>();
    }
}
```

## Best Practices

### 1. Consistent Naming
- Always use the exact naming pattern: `{EntityName}{Action}DomainEvent` → `{EntityName}{Action}IntegrationEvent`
- Maintain consistency in entity and action names across domain and integration events

### 2. Property Alignment
- Ensure that domain and integration events have compatible properties for AutoMapper
- Use the same property names and types when possible
- Add explicit mapping configuration for complex transformations if needed

### 3. Assembly Organization
- Keep domain events in `Organetto.Core` project
- Keep integration events in `Organetto.UseCases` project
- Organize events in feature-specific folders

### 4. Interface Implementation
- Domain events must implement `IDomainEvent`
- Integration events must implement `IIntegrationEvent`
- Use `IEntityIntegrationEvent<TEntityId>` for entity-specific events

## Troubleshooting

### Common Issues

1. **Mapping Not Found**: Ensure both domain and integration events follow the exact naming convention
2. **Property Mapping Errors**: Verify that property names and types are compatible between events
3. **Assembly Loading Issues**: Ensure integration events are in the same assembly as the mapping configuration

### Debugging

To debug mapping issues:
1. Check that both event types exist and follow naming conventions
2. Verify that the integration event assembly is being scanned
3. Ensure AutoMapper profile is registered in DI container
4. Use logging to trace the mapping process

## Future Considerations

- Consider adding validation to ensure naming convention compliance
- Implement tooling to generate integration events from domain events
- Add support for versioned events if needed
- Consider adding custom attributes for non-standard mappings