using Microsoft.EntityFrameworkCore;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Data.Shared.Exceptions;
using Organetto.Infrastructure.Data.Shared.Services;

namespace Organetto.Infrastructure.Data.Boards.Services
{
    /// <summary>
    /// EF Core implementation of ICardRepository.
    /// Wrapped by TransactionDecorator and OutboxIntegrationEventDecorator via DI.
    /// </summary>
    public class CardRepository
        : EfCoreGenericRepository<ICardRepository, Card, long>,
          ICardRepository
    {
        private readonly ApplicationDbContext _db;

        public CardRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Load a single Card by its key, including comments, attachments, and due-dates.
        /// </summary>
        public override async Task<Card> GetByIdAsync(long cardId, CancellationToken cancellationToken)
        {
            return await Query(_db.Cards)
                .Include(c => c.Comments)
                .Include(c => c.BoardList)
                .Include(c => c.Attachments)
                .Include(c => c.DueDates)
                .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken)
                ?? throw new EntityNotFoundException(404, nameof(Card));
        }

        /// <summary>
        /// Load all Cards in the given Column (BoardList), ordered by Position.
        /// </summary>
        public async Task<IEnumerable<Card>> GetByColumnIdAsync(long columnId, CancellationToken cancellationToken)
        {
            return await Query(_db.Cards)
                .Where(c => c.BoardListId == columnId)
                .Include(c => c.Comments)
                .Include(c => c.Attachments)
                .Include(c => c.DueDates)
                .OrderBy(c => c.Position)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Card> CreateAsync(Card entity, CancellationToken cancellationToken = default)
        {
            var created = await base.CreateAsync(entity, cancellationToken);
            var createdEntry = _db.Entry(created);
            await createdEntry.Reference(c => c.BoardList).LoadAsync(cancellationToken);
            return createdEntry.Entity;
        }
    }
}
