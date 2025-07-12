using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Services;

namespace Organetto.Core.Boards.Services
{
    /// <summary>
    /// Defines data‐access operations for Board entities. (Определяет операции доступа к данным для сущностей Board.)
    /// </summary>
    public interface IBoardRepository : IReadByIdRepository<Board, long>
    {
        /// <summary>
        /// Retrieves all boards where the given user (by userId) is either owner or member. (Получает все доски, где указанный пользователь (по userId) является владельцем или участником.)
        /// </summary>
        Task<IEnumerable<Board>> GetAllForUserAsync(long userId, CancellationToken cancellationToken);

        /// <summary>
        /// Inserts a new Board into the database. (Вставляет новую доску в базу данных.)
        /// </summary>
        Task<Board> CreateAsync(Board board, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing Board. (Обновляет существующую доску.)
        /// </summary>
        Task UpdateAsync(Board board, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a Board by its id (soft-delete by setting IsArchived=true or hard delete, depending on implementation). (Удаляет доску по её id (мягкое удаление через IsArchived=true или физическое удаление в зависимости от реализации).)
        /// </summary>
        Task DeleteAsync(long boardId, CancellationToken cancellationToken);
    }
}
