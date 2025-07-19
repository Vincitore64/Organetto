using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Services;

namespace Organetto.Core.Boards.Services
{
    /// <summary>
    /// Defines data‐access operations for Board entities. (Определяет операции доступа к данным для сущностей Board.)
    /// </summary>
    public interface IBoardRepository : IGenericRepository<Board, long> // IReadByIdRepository<Board, long>
    {
        /// <summary>
        /// Retrieves all boards where the given user (by userId) is either owner or member. (Получает все доски, где указанный пользователь (по userId) является владельцем или участником.)
        /// </summary>
        Task<IEnumerable<Board>> GetAllForUserAsync(long userId, CancellationToken cancellationToken);
    }
}
