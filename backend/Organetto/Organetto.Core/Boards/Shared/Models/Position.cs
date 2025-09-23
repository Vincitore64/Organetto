using System.Numerics;

namespace Organetto.Core.Boards.Shared.Models
{
    public readonly record struct Position(long Value) : IComparable<Position>
    {
        public static readonly Position Min = new(long.MinValue + 1);
        public static readonly Position Max = new(long.MaxValue - 1);

        public int CompareTo(Position other) => Value.CompareTo(other.Value);
        public override string ToString() => Value.ToString();

        // Factory with bounds (use when constructing from untrusted input)
        public static Position From(long value)
        {
            if (value <= long.MinValue || value >= long.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), "Position is out of safe bounds.");
            return new Position(value);
        }

        /// <summary>
        /// Compute a position strictly between left and right.
        /// If one side is null, treats it as open end (Min/Max).
        /// Returns null when no gap remains (caller should rebalance).
        /// </summary>
        public static Position? Between(Position? left, Position? right)
        {
            var a = left?.Value ?? Min.Value;
            var b = right?.Value ?? Max.Value;

            BigInteger gap = (BigInteger)b - a;
            if (gap <= 1) return null;

            var mid = (BigInteger)a + (gap / 2);
            return new Position((long)mid);
        }

        public Position? Between(Position other) => Between(this, other);

        /// <summary>
        /// Evenly distributes positions for a sequence of length <paramref name="count"/>.
        /// </summary>
        public static IReadOnlyList<Position> Distribute(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return Array.Empty<Position>();

            BigInteger range = (BigInteger)Max.Value - Min.Value;
            BigInteger step = range / (count + 1);

            var result = new Position[count];
            for (int i = 0; i < count; i++)
            {
                BigInteger val = (BigInteger)Min.Value + step * (i + 1);
                result[i] = new Position((long)val);
            }
            return result;
        }
    }
}
