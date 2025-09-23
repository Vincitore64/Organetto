using AutoMapper;
using MediatR;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Core.Shared.Services.Extensions;
using Organetto.UseCases.Boards.Columns.Cards.Data;

namespace Organetto.UseCases.Boards.Columns.Cards.Queries
{
    public sealed record GetCardDetailQuery(long CardId) : IRequest<CardDetailDto>;

    public sealed class GetCardDetailQueryHandler : IRequestHandler<GetCardDetailQuery, CardDetailDto>
    {
        private readonly ICardRepository _repository;
        private readonly IMapper _mapper;

        public GetCardDetailQueryHandler(ICardRepository repository, IMapper mapper)
        {
            this._repository = repository;
            _mapper = mapper;
        }

        public async Task<CardDetailDto> Handle(GetCardDetailQuery request, CancellationToken ct)
        {
            var item = await _repository.WithoutTracking().GetByIdAsync(request.CardId, ct);

            var result = _mapper.Map<CardDetailDto>(item);
            return result;
        }
    }

}
