namespace Organetto.BuildingBlocks.Core.Databases.Transactions
{
    public interface IDatabaseTransaction : IDisposable
    {
        public Task CommitAsync(CancellationToken cancellationToken = default);

        public Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
