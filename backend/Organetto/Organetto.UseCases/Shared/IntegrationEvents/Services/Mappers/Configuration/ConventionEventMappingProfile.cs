using AutoMapper;
using Organetto.Core.Shared.Events.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers.Configuration.Extensions;

namespace Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers.Configuration
{
    public class ConventionEventMappingProfile : Profile
    {
        public ConventionEventMappingProfile()
        {
            // assume your domain events and integration events live in the same assembly
            var domainEventType = typeof(IDomainEvent);
            var asm = domainEventType.Assembly;

            // find all concrete domain‐event types
            var domainEventTypes = asm
                .GetTypes()
                .Where(t => domainEventType.IsAssignableFrom(t)
                         && t.IsClass
                         && !t.IsAbstract)
                .ToArray();

            foreach (var deType in domainEventTypes)
            {
                // look for a matching integration event: Foo → FooIntegrationEvent
                var ieType = deType.GetIntegrationEventType();
                if (ieType == null) continue;
                CreateMap(deType, ieType);
            }
        }
    }
}
