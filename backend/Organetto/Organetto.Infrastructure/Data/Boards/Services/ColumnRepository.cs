using Microsoft.EntityFrameworkCore;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Data.Shared.Exceptions;
using Organetto.Infrastructure.Data.Shared.Services;

namespace Organetto.Infrastructure.Data.Boards.Services
{
    public class ColumnRepository : EfCoreGenericRepository<IColumnRepository, BoardList, long>, IColumnRepository
    {
        private readonly ApplicationDbContext _db;

        public ColumnRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public override async Task<BoardList> GetByIdAsync(long columnId, CancellationToken cancellationToken)
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
    }
}
