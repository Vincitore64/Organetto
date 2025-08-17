using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Shared.Services;

namespace Organetto.Core.Boards.Cards.Services
{
    public interface IAttachmentRepository : IGenericRepository<Attachment, long>
    {
        Task<IEnumerable<Attachment>> GetAllForCardAsync(long cardId, CancellationToken cancellationToken);
    }
}
