using System.ComponentModel.DataAnnotations;

namespace Organetto.UseCases.Shared.MassTransit.Configuration.Models
{
    public sealed class RabbitMqOptions
    {
        [Required] public string Host { get; init; } = default!;
        [Range(1, 65535)] public int Port { get; init; } = 5672;
        [Required] public string VirtualHost { get; init; } = "/";
        [Required] public string Username { get; init; } = default!;
        [Required] public string Password { get; init; } = default!;

        public bool UseSsl { get; init; } = false;
        public SslOptions Ssl { get; init; } = new();
        [Range(1, ushort.MaxValue)] public ushort PrefetchCount { get; init; } = 16;
        [Range(0, 10_000)] public int ConcurrentMessageLimit { get; init; } = 0;

        /// <summary>DelayedExchange | Quartz</summary>
        [Required] public string Scheduler { get; init; } = "DelayedExchange";

        public RetryOptions Retry { get; init; } = new();
        public sealed class SslOptions
        {
            public string? ServerName { get; init; }
            /// <summary>None|RemoteCertificateNameMismatch|RemoteCertificateChainErrors|All</summary>
            public string AcceptablePolicyErrors { get; init; } = "None";
        }

        public sealed class RetryOptions
        {
            /// <summary>Количество немедленных ретраев в текущей доставке.</summary>
            [Range(0, 100)] public int Immediate { get; init; } = 0;
            /// <summary>Задержанные редоставки (Scheduled Redelivery) — массив интервалов.</summary>
            [Required] public string[] DelayedIntervals { get; init; } = Array.Empty<string>();
        }
    }
}
