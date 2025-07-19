using Organetto.UseCases.Boards.Columns.Commands;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Shared.IntegrationEvents.Factories;
using Organetto.UseCases.Shared.IntegrationEvents.Models;

namespace Organetto.UseCases.Boards.Columns.IntergationEvents.Factories
{
    public class ColumnMetadataUpdatedEventFactory
    : IIntegrationEventFactory<UpdateColumnMetadataCommand, BoardListDto>
    {
        public IEnumerable<IntegrationEvent> Create(
            UpdateColumnMetadataCommand request,
            BoardListDto response)
        {
            yield return new BoardListUpdatedIntegrationEvent(response.Id, request.BoardId);
        }
    }
}
