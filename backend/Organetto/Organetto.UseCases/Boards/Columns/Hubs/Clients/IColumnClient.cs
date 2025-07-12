using Organetto.UseCases.Boards.Columns.Data;

namespace Organetto.UseCases.Boards.Columns.Hubs.Clients
{
    public interface IColumnClient
    {
        Task ColumnCreated(BoardListDto boardList);
        Task ColumnDeleted(long boardListId);
        Task ColumnMetadataUpdated(BoardListDto boardList);
    }
}
