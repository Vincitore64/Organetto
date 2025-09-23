using Organetto.Core.Boards.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Organetto.Infrastructure.Infrastructure.Boards.Shared.Services
{
    // Optional: JSON converter if you ever expose Position directly in API contracts
    public sealed class PositionJsonConverter : JsonConverter<Position>
    {
        public override Position Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetInt64());

        public override void Write(Utf8JsonWriter writer, Position value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value.Value);
    }
}
