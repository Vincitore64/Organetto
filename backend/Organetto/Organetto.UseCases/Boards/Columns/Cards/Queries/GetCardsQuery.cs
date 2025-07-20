using AutoMapper;
using MediatR;
using Organetto.Core.Boards.Cards.Services;
using Organetto.UseCases.Boards.Data;
using Organetto.Core.Shared.Services.Extensions;

namespace Organetto.UseCases.Boards.Columns.Cards.Queries
{
    /// <summary>
    /// Query to retrieve all cards in a specific column.
    /// </summary>
    public record GetCardsQuery(
        long ColumnId
    ) : IRequest<IEnumerable<CardDto>>;

    /// <summary>
    /// Handler for GetCardsQuery: loads cards from repository and maps to DTOs.
    /// </summary>
    public class GetCardsQueryHandler
        : IRequestHandler<GetCardsQuery, IEnumerable<CardDto>>
    {
        private readonly ICardRepository _repository;
        private readonly IMapper _mapper;

        public GetCardsQueryHandler(
            ICardRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CardDto>> Handle(
            GetCardsQuery request,
            CancellationToken cancellationToken)
        {
            // Load domain entities (including Comments, Attachments, DueDates)
            var cards = await _repository.WithoutTracking().GetByColumnIdAsync(request.ColumnId, cancellationToken);

            // Map to DTOs
            return _mapper.Map<IEnumerable<CardDto>>(cards);
        }
    }
}
