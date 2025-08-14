namespace Organetto.UseCases.Boards.Data
{
    public record CardDto(long Id, string Title, string Description, long Position, DateTimeOffset? DueDate) : IComparable<CardDto>
    {
        public CardDto() : this(0, string.Empty, string.Empty, 0, null)
        {
            
        }

        public int CompareTo(CardDto? other)
        {
            return Position.CompareTo(other?.Position);
        }
    }
}
