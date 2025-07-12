using Microsoft.EntityFrameworkCore;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Data.Shared.Exceptions;

namespace Organetto.Infrastructure.Data.Boards.Services
{
    /// <summary>
    /// EF Core implementation of IBoardRepository. (Реализация IBoardRepository на основе EF Core.)
    /// </summary>
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor: injects ApplicationDbContext. (Конструктор: внедряет ApplicationDbContext.)
        /// </summary>
        public BoardRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc/>
        public async Task<Board> GetByIdAsync(long boardId, CancellationToken cancellationToken)
        {
            // Возвращаем доску по её id, включая связанные списки и членов (если нужно)
            return await _dbContext.Boards
                .Include(b => b.Owner)
                .Include(b => b.Members)
                    .ThenInclude(m => m.User)
                .Include(b => b.Lists)
                .FirstOrDefaultAsync(b => b.Id == boardId, cancellationToken) ?? throw new EntityNotFoundException(400, nameof(Board));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Board>> GetAllForUserAsync(long userId, CancellationToken cancellationToken)
        {
            // Получаем доски, где пользователь является владельцем.
            var ownerBoards = await _dbContext.Boards
                .Where(b => b.OwnerId == userId)
                .Include(b => b.Members).ThenInclude(m => m.User).Include(b => b.Lists)
                .ToListAsync(cancellationToken);

            // Получаем доски, где пользователь – участник.
            var memberBoards = (await _dbContext.BoardMembers
                .Where(bm => bm.UserId == userId)
                .Include(bm => bm.User)
                .Include(bm => bm.Board!)
                .ThenInclude(b => b.Lists)
                .ToListAsync())
                .Select(bm => bm.Board!)
                .ToList(); // TODO: check that the Board is not null

            // Объединяем оба набора без дубликатов.
            return ownerBoards.Union(memberBoards).ToList();
        }

        /// <inheritdoc/>
        public Task<Board> CreateAsync(Board board, CancellationToken cancellationToken)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            board.CreatedAt = DateTime.UtcNow;
            board.UpdatedAt = DateTime.UtcNow;
            _dbContext.Boards.Add(board);
            return Task.FromResult(board);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Board board, CancellationToken cancellationToken)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            // Обновляем только поля, которые можно менять.
            var existing = await _dbContext.Boards.FindAsync(board.Id, cancellationToken);
            if (existing == null)
                throw new KeyNotFoundException($"Board with id {board.Id} not found.");

            existing.Title = board.Title;
            existing.Description = board.Description;
            existing.IsArchived = board.IsArchived;
            existing.UpdatedAt = DateTime.UtcNow;

            _dbContext.Boards.Update(existing);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long boardId, CancellationToken cancellationToken)
        {
            var board = await _dbContext.Boards.FindAsync(boardId);
            if (board == null)
                throw new KeyNotFoundException($"Board with id {boardId} not found.");

            // Пример мягкого удаления: ставим флаг IsArchived=true.
            board.IsArchived = true;
            board.UpdatedAt = DateTime.UtcNow;
            _dbContext.Boards.Update(board);

            // Для физического удаления вместо этого используйте:
            // _dbContext.Boards.Remove(board);

            //await _dbContext.SaveChangesAsync();
        }
    }
}
