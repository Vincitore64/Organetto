using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Services;

namespace Organetto.Core.Boards.Services
{
    /// <summary>
    /// Defines data‐access operations for Column (list) entities.
    /// </summary>
    public interface IColumnRepository : IGenericRepository<BoardList, long> // IReadByIdRepository<BoardList, long>, IReadByIdAndUpdateRepository<BoardList, long>
    {
        ///// <summary>
        ///// Retrieves a single Column by its surrogate key.
        ///// </summary>
        //Task<BoardList> GetByIdAsync(long columnId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all Columns (including their Cards) for a given Board.
        /// </summary>
        Task<IEnumerable<BoardList>> GetByBoardIdAsync(long boardId, CancellationToken cancellationToken);
    }
}
