using Organetto.UseCases.Boards.Data;

namespace Organetto.UseCases.Boards.Services
{
    public interface IBoardClient
    {
        Task BoardCreated(BoardDto board);
        Task BoardDeleted(long boardId);
        Task BoardMetadataUpdated(BoardDto board);
    }
}
