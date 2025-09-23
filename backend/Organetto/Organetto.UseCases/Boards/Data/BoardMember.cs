namespace Organetto.UseCases.Boards.Data
{
    public record BoardMember(long Id, string Email, string Name, string Role)
    {
        public BoardMember() : this(0, string.Empty, string.Empty, string.Empty)
        {
            
        }
    }
}
