using Organetto.BuildingBlocks.Core.Databases.Transactions;

namespace Organetto.BuildingBlocks.Core.Databases
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
