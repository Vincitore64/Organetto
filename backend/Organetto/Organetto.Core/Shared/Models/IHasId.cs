namespace Organetto.Core.Shared.Models
{
    /// <summary>
    /// Marker interface for commands that carry an identifier.
    /// </summary>
    public interface IHasId<TKey>
    {
        TKey Id { get; }
    }
}
