namespace Organetto.UseCases.Boards.Data
{
    public class BoardDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        /// <summary>
        /// Users who have access to the board.
        /// </summary>
        public List<BoardMember> Members { get; set; } = new();

        /// <summary>
        /// Lists (columns) on the board, each with its cards.
        /// </summary>
        public List<BoardListDto> Columns { get; set; } = new();
    }
}
