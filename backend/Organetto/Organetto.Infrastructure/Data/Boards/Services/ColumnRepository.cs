using Microsoft.EntityFrameworkCore;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Data.Shared.Exceptions;

namespace Organetto.Infrastructure.Data.Boards.Services
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly ApplicationDbContext _db;

        public ColumnRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<BoardList> GetByIdAsync(long columnId, CancellationToken cancellationToken)
        {
            return await _db.BoardLists
                .Include(c => c.Cards)
                .FirstOrDefaultAsync(c => c.Id == columnId, cancellationToken)
                ?? throw new EntityNotFoundException(404, nameof(BoardList));
        }

        public async Task<IEnumerable<BoardList>> GetByBoardIdAsync(long boardId, CancellationToken cancellationToken)
        {
            return await _db.BoardLists
                .Where(c => c.BoardId == boardId)
                .Include(c => c.Cards)
                .OrderBy(c => c.Position)
                .ToListAsync(cancellationToken);
        }

        public async Task<BoardList> CreateAsync(BoardList column, CancellationToken cancellationToken)
        {
            var entry = await _db.BoardLists.AddAsync(column, cancellationToken);
            return entry.Entity;
        }

        public Task UpdateAsync(BoardList column, CancellationToken cancellationToken)
        {
            _db.BoardLists.Update(column);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(long columnId, CancellationToken cancellationToken)
        {
            var column = await _db.BoardLists.FirstOrDefaultAsync(l => l.Id == columnId, cancellationToken);
            if (column != null)
            {
                _db.BoardLists.Remove(column);
            }
        }
    }
}
