using Organetto.UseCases.Boards.Data;

namespace Organetto.UseCases.Boards.Columns.Cards.Hubs
{
    public interface ICardClient
    {
        Task CardCreated(CardDto dto);
        Task CardUpdated(CardDto dto);
        Task CardDeleted(long cardId);
    }
}
