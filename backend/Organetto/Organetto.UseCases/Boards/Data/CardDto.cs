namespace Organetto.UseCases.Boards.Data
{
    public record CardDto(long Id, string Title, string Description, long Position, DateTimeOffset? DueDate)
    {
        public CardDto() : this(0, string.Empty, string.Empty, 0, null)
        {
            
        }
    }
}
