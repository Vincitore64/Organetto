using Organetto.Core.Shared.Events.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers
{
    public interface IEventsMapper
    {
        IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> domainEvents);
    }
}
