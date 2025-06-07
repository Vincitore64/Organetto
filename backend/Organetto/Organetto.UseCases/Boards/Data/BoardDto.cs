namespace Organetto.UseCases.Boards.Data
{
    /// <summary>
    /// Data Transfer Object for a Board (DTO для доски).
    /// </summary>
    public class BoardDto
    {
        public BoardDto()
        {
            Title = string.Empty;
            Description = string.Empty;
        }

        public long Id { get; set; }               // Surrogate key (суррогатный ключ)
        public string Title { get; set; }          // Board title (название доски)
        public string Description { get; set; }    // Board description (описание)
        public long OwnerId { get; set; }          // Owner’s user ID (ID владельца)
        public DateTime CreatedAt { get; set; }    // Created timestamp (время создания)
        public DateTime UpdatedAt { get; set; }    // Last update timestamp (время последнего обновления)
        public bool IsArchived { get; set; }       // Archive flag (флаг архивирования)
    }
}
