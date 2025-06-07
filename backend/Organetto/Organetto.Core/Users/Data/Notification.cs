namespace Organetto.Core.Users.Data
{
    /// <summary>
    /// Represents a notification sent to a user.
    /// </summary>
    public class Notification
    {
        public Notification()
        {
            Message = string.Empty;

        }

        public long Id { get; set; }                                    // Surrogate PK (суррогатный первичный ключ)
        public long UserId { get; set; }                                // FK to User (внешний ключ к User)
        public string Message { get; set; }                              // Notification text (текст уведомления)
        public DateTime SentAt { get; set; }                              // Sent timestamp (время отправки)
        public bool IsRead { get; set; }                                  // Read/unread flag (прочитано / не прочитано)

        // Navigation properties
        public User? User { get; set; }                                   // Recipient navigation (связь с получателем)
    }
}
