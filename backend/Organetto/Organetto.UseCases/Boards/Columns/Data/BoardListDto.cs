using Organetto.UseCases.Boards.Data;

namespace Organetto.UseCases.Boards.Columns.Data
{
    /// <param name="Id"></param>
    /// <param name="Title"></param>
    /// <param name="Position"></param>
    /// <param name="Cards"> Cards within the column. </param>
    public record BoardListDto(long Id, string Title, long Position, List<CardDto> Cards) : IComparable<BoardListDto>
    {
        public int CompareTo(BoardListDto? other)
        {
            return Position.CompareTo(other?.Position);
        }
    }
}
