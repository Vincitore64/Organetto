namespace Organetto.BuildingBlocks.Core.Models
{
    public interface IEntity<TId> : IHasId<TId>, IHasDomainEvents
    {
    }
}
