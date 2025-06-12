using Microsoft.EntityFrameworkCore;
using Organetto.Core.Boards.Data;
using Organetto.Core.Users.Data;
using Organetto.Core.Users.Services;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Data.Shared.Exceptions;

namespace Organetto.Infrastructure.Data.Users.Services
{
    /// <summary>
    /// EF Core implementation of IUserRepository. (Реализация IUserRepository на основе EF Core.)
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor: injects ApplicationDbContext. (Конструктор: внедряет ApplicationDbContext.)
        /// </summary>
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc/>
        public async Task<User> GetByIdAsync(long userId)
        {
            return await _dbContext.Users
                .Include(u => u.BoardMemberships)
                    .ThenInclude(bm => bm.Board)
                .Include(u => u.OwnedBoards)
                .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new EntityNotFoundException(400, nameof(Board));
        }

        /// <inheritdoc/>
        public async Task<User> GetByFirebaseUidAsync(string firebaseUid)
        {
            if (string.IsNullOrEmpty(firebaseUid))
                throw new ArgumentException("Firebase UID must be provided.", nameof(firebaseUid));

            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid) ?? throw new EntityNotFoundException(400, nameof(Board));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users
                .AsNoTracking()
                .ToListAsync();
        }

        /// <inheritdoc/>
        public Task<User> CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.CreatedAt = DateTime.UtcNow;
            _dbContext.Users.Add(user);
            return Task.FromResult(user);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existing = await _dbContext.Users.FindAsync(user.Id);
            if (existing == null)
                throw new KeyNotFoundException($"User with id {user.Id} not found.");

            // Update only mutable fields
            existing.Name = user.Name;
            existing.Email = user.Email;

            _dbContext.Users.Update(existing);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with id {userId} not found.");

            _dbContext.Users.Remove(user);
        }
    }
}
