using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Shared.IntegrationEvents.Factories
{
    /// <summary>
    /// Factory that builds one or more integration events from a command <typeparamref name="TRequest"/>
    /// and its response <typeparamref name="TResponse"/>.
    /// </summary>
    public interface IIntegrationEventFactory<in TRequest, in TResponse>
    {
        IEnumerable<IntegrationEvent> Create(TRequest request, TResponse response);
    }
}
