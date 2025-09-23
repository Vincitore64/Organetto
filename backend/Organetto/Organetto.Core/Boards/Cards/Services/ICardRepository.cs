using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Shared.Services;

namespace Organetto.Core.Boards.Cards.Services
{
    /// <summary>
    /// Defines data‐access operations for Card entities.
    /// </summary>
    public interface ICardRepository : IGenericRepository<Card, long>
    {
        /// <summary>
        /// Retrieves all Cards (including comments, attachments, due-dates)
        /// for a given Column (BoardList).
        /// </summary>
        Task<IEnumerable<Card>> GetByColumnIdAsync(long columnId, CancellationToken cancellationToken);
    }
}
