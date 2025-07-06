namespace Organetto.UseCases.Boards.Data
{
    public record CardDto(Guid Id, string Title, string Description, int Position, DateTime? DueDate);
}
