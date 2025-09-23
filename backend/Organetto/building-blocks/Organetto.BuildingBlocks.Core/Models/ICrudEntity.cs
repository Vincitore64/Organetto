namespace Organetto.BuildingBlocks.Core.Models
{
    public interface ICrudEntity
    {
        void MarkCreated();
        void MarkDeleted();
        void MarkUpdated();
    }

    public interface ICrudEntity<TId> : ICrudEntity, IEntity<TId> { }
}