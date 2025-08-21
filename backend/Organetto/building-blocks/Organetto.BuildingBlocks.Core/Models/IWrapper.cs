namespace Organetto.BuildingBlocks.Core.Models
{
    public interface IWrapper<TIn> where TIn : notnull
    {
        TIn Value { get; }

        bool Equals(IWrapper<TIn> other)
        {
            return Value.Equals(other.Value);
        }
        int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
