namespace Organetto.Core.Boards.Cards.Data
{
    /// <summary>
    /// Represents a due date entry for a card.
    /// </summary>
    public class DueDate
    {
        public long Id { get; set; }                                    // Surrogate PK (суррогатный первичный ключ)
        public long CardId { get; set; }                                // FK to Card (внешний ключ к Card)
        public DateTime DueAt { get; set; }                              // Deadline timestamp (срок выполнения)
        public bool IsComplete { get; set; }                              // Completion flag (признак выполнения)

        // Navigation properties
        public Card? Card { get; set; }                                   // Card navigation (связь с карточкой)
    }
}
