namespace Organetto.Core.Shared.Models
{
    public interface ICrudEntity
    {
        void MarkCreated();
        void MarkDeleted();
        void MarkUpdated();
    }

    public interface ICrudEntity<TId> : ICrudEntity, IEntity<TId> { }
}