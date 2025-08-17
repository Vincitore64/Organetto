using Microsoft.EntityFrameworkCore;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Infrastructure.Data.Shared;
using Organetto.Infrastructure.Data.Shared.Services;

namespace Organetto.Infrastructure.Data.Boards.Services
{
    public class AttachmentRepository : EfCoreGenericRepository<IAttachmentRepository, Attachment, long>, IAttachmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttachmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<Attachment>> GetAllForCardAsync(long cardId, CancellationToken cancellationToken)
        {
            var links = await Query(_dbContext.Set<AttachmentLink>())
                .Where(x => x.OwnerId == cardId && x.OwnerKind == Core.Boards.Cards.Models.AttachmentOwnerKind.Card)
                .Include(x => x.Attachment)
                .ToArrayAsync(cancellationToken);

            var items = links.Select(l => l.Attachment).ToArray();
            return items;
        }
    }
}
