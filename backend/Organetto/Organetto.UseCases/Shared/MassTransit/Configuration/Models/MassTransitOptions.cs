using System.ComponentModel.DataAnnotations;

namespace Organetto.UseCases.Shared.MassTransit.Configuration.Models
{
    public sealed class MassTransitOptions
    {
        public bool KebabCaseEndpointNames { get; init; } = true;
        [Required] public string SignalrEndpointName { get; init; } = "signalr-endpoints";
    }
}
