using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Services;

namespace Organetto.Core.Boards.Services
{
    /// <summary>
    /// Defines data‐access operations for Column (list) entities.
    /// </summary>
    public interface IColumnRepository : IReadByIdRepository<BoardList, long>
    {
        ///// <summary>
        ///// Retrieves a single Column by its surrogate key.
        ///// </summary>
        //Task<BoardList> GetByIdAsync(long columnId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all Columns (including their Cards) for a given Board.
        /// </summary>
        Task<IEnumerable<BoardList>> GetByBoardIdAsync(long boardId, CancellationToken cancellationToken);

        /// <summary>
        /// Inserts a new Column into the database.
        /// </summary>
        Task<BoardList> CreateAsync(BoardList column, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing Column.
        /// </summary>
        Task UpdateAsync(BoardList column, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a Column by its id.
        /// </summary>
        Task DeleteAsync(long columnId, CancellationToken cancellationToken);
    }
}
