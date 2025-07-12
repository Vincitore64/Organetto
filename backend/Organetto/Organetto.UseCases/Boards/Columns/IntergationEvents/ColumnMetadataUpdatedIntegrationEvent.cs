using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.Columns.IntergationEvents
{
    public record ColumnMetadataUpdatedIntegrationEvent(long EntityId, long BoardId) : IntegrationEvent, IEntityIntegrationEvent<long>;
}
