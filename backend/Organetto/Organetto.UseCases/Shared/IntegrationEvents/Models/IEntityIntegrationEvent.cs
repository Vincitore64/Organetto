namespace Organetto.UseCases.Shared.IntegrationEvents.Models
{
    /// <summary>
    /// Marker interface for integration events carrying an entity ID.
    /// </summary>
    public interface IEntityIntegrationEvent<TEntityId>
    {
        TEntityId EntityId { get; }

    }
}
