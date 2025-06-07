using Organetto.Core.Shared.Databases.Transactions;

namespace Organetto.Core.Shared.Databases
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
