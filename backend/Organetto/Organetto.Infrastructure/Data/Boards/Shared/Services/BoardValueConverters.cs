using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Organetto.Core.Boards.Shared.Models;

namespace Organetto.Infrastructure.Data.Boards.Shared.Services
{
    public class BoardValueConverters
    {
        private readonly static ValueConverter<Position, long> _positionConverter = new ValueConverter<Position, long>(
            p => p.Value,
            v => new Position(v)
        );

        private readonly static ValueComparer<Position> _positionComparer = new ValueComparer<Position>(
            (a, b) => a.Value == b.Value,
            p => p.Value.GetHashCode(),
            p => new Position(p.Value)
        );

        public static ValueConverter<Position, long> PositionConverter => _positionConverter;

        public static ValueComparer<Position> PositionComparer => _positionComparer;
    }
}
