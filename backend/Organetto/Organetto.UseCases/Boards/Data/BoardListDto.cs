namespace Organetto.UseCases.Boards.Data
{
    /// <param name="Id"></param>
    /// <param name="Title"></param>
    /// <param name="Position"></param>
    /// <param name="Cards"> Cards within the column. </param>
    public record BoardListDto(Guid Id, string Title, int Position, List<CardDto> Cards);
}
