namespace Organetto.UseCases.Boards.Data
{
    public record CardDto(long Id, string Title, string Description, int Position, DateTime? DueDate);
}
