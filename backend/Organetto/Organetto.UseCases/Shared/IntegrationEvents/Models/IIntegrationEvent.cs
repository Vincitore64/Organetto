namespace Organetto.UseCases.Shared.IntegrationEvents.Models
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
        DateTimeOffset OccurredOn { get; }
    }

    public record IntegrationEvent(Guid Id, DateTimeOffset OccurredOn) : IIntegrationEvent
    {
        public IntegrationEvent() : this(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
        }
    }
}