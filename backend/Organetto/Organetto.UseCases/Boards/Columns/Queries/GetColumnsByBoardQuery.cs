using AutoMapper;
using FluentValidation;
using MediatR;
using Organetto.Core.Boards.Services;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Shared.Validation.Extensions;

namespace Organetto.UseCases.Boards.Columns.Queries
{
    /// <summary>
    /// Query to retrieve all columns (lists) for a specific board, including their cards.
    /// </summary>
    public record GetColumnsByBoardQuery(long BoardId) : IRequest<IEnumerable<BoardListDto>>;

    /// <summary>
    /// Handler for GetColumnsByBoardQuery.
    /// </summary>
    public class GetColumnsByBoardQueryHandler
        : IRequestHandler<GetColumnsByBoardQuery, IEnumerable<BoardListDto>>
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetColumnsByBoardQuery> _validator;

        public GetColumnsByBoardQueryHandler(
            IColumnRepository columnRepository,
            IMapper mapper,
            IValidator<GetColumnsByBoardQuery> validator)
        {
            _columnRepository = columnRepository ?? throw new ArgumentNullException(nameof(columnRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._validator = validator;
        }

        public async Task<IEnumerable<BoardListDto>> Handle(
            GetColumnsByBoardQuery request,
            CancellationToken cancellationToken)
        {
            _validator.TryValidate(request);

            // Fetch columns and their cards from repository
            var columns = await _columnRepository.GetByBoardIdAsync(
                request.BoardId,
                cancellationToken);

            // Map domain entities (BoardList) to DTOs
            var dtos = _mapper.Map<IEnumerable<BoardListDto>>(columns);
            return dtos;
        }
    }
}
