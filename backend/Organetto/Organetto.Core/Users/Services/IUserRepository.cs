using Organetto.Core.Users.Data;

namespace Organetto.Core.Users.Services
{
    /// <summary>
    /// Defines data‐access operations for User entities. (Определяет операции доступа к данным для сущностей User.)
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by its surrogate key. (Получает пользователя по суррогатному ключу.)
        /// </summary>
        Task<User> GetByIdAsync(long userId);

        /// <summary>
        /// Retrieves a user by their Firebase UID. (Получает пользователя по его Firebase UID.)
        /// </summary>
        Task<User> GetByFirebaseUidAsync(string firebaseUid);

        /// <summary>
        /// Retrieves all users. (Получает всех пользователей.)
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Inserts a new User into the database. (Вставляет нового пользователя в базу данных.)
        /// </summary>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// Updates an existing User. (Обновляет существующего пользователя.)
        /// </summary>
        Task UpdateAsync(User user);

        /// <summary>
        /// Deletes a User by its id. (Удаляет пользователя по его id.)
        /// </summary>
        Task DeleteAsync(long userId);
    }
}
