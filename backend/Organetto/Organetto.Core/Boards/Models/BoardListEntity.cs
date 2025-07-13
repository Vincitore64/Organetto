using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Shared.Exceptions;
using Organetto.Core.Shared.Models;

namespace Organetto.Core.Boards.Models
{
    ///// <summary>
    ///// Сущность-колонка в составе агрегата Board.
    ///// </summary>
    //public class BoardListEntity : BaseEntity<long>
    //{
    //    private readonly List<CardEntity> _cards = new();

    //    public long BoardId { get; private set; }
    //    public string Title { get; private set; }
    //    public int Position { get; private set; }

    //    public IReadOnlyCollection<CardEntity> Cards => _cards.AsReadOnly();

    //    // Для EF Core
    //    private BoardListEntity(): base(0)
    //    {

    //    }

    //    /// <summary>
    //    /// Создаёт новую колонку в доске.
    //    /// </summary>
    //    public BoardListEntity(long boardId, string title, int position) : base(0)
    //    {
    //        if (string.IsNullOrWhiteSpace(title))
    //            throw new DomainException("Title cannot be empty.");

    //        BoardId = boardId;
    //        Title = title;
    //        Position = position;
    //    }

    //    // Unified metadata update
    //    public void UpdateMetadata(string title, int position)
    //    {
    //        if (string.IsNullOrWhiteSpace(title))
    //            throw new DomainException("List title cannot be empty.");
    //        if (position < 0)
    //            throw new DomainException("Position must be non-negative.");
    //        Title = title;
    //        Position = position;
    //    }

    //    /// <summary>
    //    /// Добавляет карточку в эту колонку.
    //    /// </summary>
    //    public CardEntity AddCard(string title, string? description, int order)
    //    {
    //        // Пример простого инварианта: уникальный порядок внутри колонки
    //        if (_cards.Any(c => c.Position == order))
    //            throw new DomainException("A card at this position already exists.");

    //        var card = new CardEntity(Id, title, description, order);
    //        _cards.Add(card);
    //        return card;
    //    }

    //    /// <summary>
    //    /// Удаляет карточку из колонки.
    //    /// </summary>
    //    public void RemoveCard(long cardId)
    //    {
    //        var card = _cards.FirstOrDefault(c => c.Id == cardId);
    //        if (card == null) return;

    //        _cards.Remove(card);
    //    }

    //    /// <summary>
    //    /// Перемещает карточку внутри колонки.
    //    /// </summary>
    //    public void MoveCard(long cardId, int newOrder)
    //    {
    //        var card = _cards.FirstOrDefault(c => c.Id == cardId);
    //        if (card == null)
    //            throw new DomainException($"Card {cardId} not found.");

    //        // Можно расширить проверку: сдвинуть остальные карточки и т.д.
    //        card.UpdateOrder(newOrder);
    //    }
    //}
}
