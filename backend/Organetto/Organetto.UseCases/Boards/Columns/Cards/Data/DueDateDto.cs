namespace Organetto.UseCases.Boards.Columns.Cards.Data
{
    public record DueDateDto(DateTime DueAt, bool IsComplete)
    {
        public DueDateDto() : this(DateTime.UnixEpoch, false)
        {
        }
    }
}
