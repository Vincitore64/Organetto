namespace Organetto.UseCases.Shared.Outbox.Models
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Когда событие произошло (UTC).
        /// </summary>
        public DateTimeOffset OccurredOn { get; set; }

        /// <summary>
        /// CLR-тип или «semantic name» события.
        /// </summary>
        public string Type { get; set; } = null!;

        /// <summary>
        /// JSON-полезная нагрузка.
        /// </summary>
        public string Payload { get; set; } = null!;

        /// <summary>
        /// Опциональный correlation id для трассировки.
        /// </summary>
        public Guid? CorrelationId { get; set; }

        /// <summary>
        /// Когда событие было обработано (UTC).
        /// </summary>
        public DateTimeOffset? ProcessedOn { get; set; }

        /// <summary>
        /// Ошибка, если событие не удалось обработать.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Количество попыток обработки.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Когда была последняя попытка обработки (UTC).
        /// </summary>
        public DateTimeOffset? LastRetry { get; set; }
    }
}