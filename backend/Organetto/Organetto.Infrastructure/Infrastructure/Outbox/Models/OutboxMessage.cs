namespace Organetto.Infrastructure.Infrastructure.Outbox.Models
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
        /// Сколько раз при публикации упало (retry logic).
        /// </summary>
        public int RetryCount { get; set; } = 0;

        /// <summary>
        /// Когда сообщение было успешно отправлено.
        /// </summary>
        public DateTimeOffset? ProcessedOn { get; set; }

        /// <summary>
        /// Текст последней ошибки (обрезается до 3000 символов).
        /// </summary>
        public string? Error { get; set; }
    }
}
