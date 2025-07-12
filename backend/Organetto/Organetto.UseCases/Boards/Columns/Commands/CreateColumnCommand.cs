using AutoMapper;
using FluentValidation;
using MediatR;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Boards.Columns.IntergationEvents;
using Organetto.UseCases.Shared.Outbox.Services;
using Organetto.UseCases.Shared.Validation.Extensions;

namespace Organetto.UseCases.Boards.Columns.Commands
{
    /// <summary>
    /// Command to create a new column (list) under a specific board.
    /// </summary>
    public record CreateColumnCommand(long BoardId, string Title, int Position)
        : IRequest<BoardListDto>;

    /// <summary>
    /// Handler for CreateColumnCommand.
    /// </summary>
    public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, BoardListDto>
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOutboxService _outboxService;
        private readonly IValidator<CreateColumnCommand> _validator;

        public CreateColumnCommandHandler(
            IColumnRepository columnRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOutboxService outboxService,
            IValidator<CreateColumnCommand> validator)
        {
            _columnRepository = columnRepository ?? throw new ArgumentNullException(nameof(columnRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._unitOfWork = unitOfWork;
            this._outboxService = outboxService;
            this._validator = validator;
        }

        public async Task<BoardListDto> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
        {
            _validator.TryValidate(request);

            var dbTransaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Create the domain entity
            var newColumn = new BoardList
            {
                BoardId = request.BoardId,
                Title = request.Title,
                Position = request.Position
            };

            // Persist via repository
            var created = await _columnRepository.CreateAsync(newColumn, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _outboxService.AddAsync(new ColumnCreatedIntegrationEvent(created.Id, created.BoardId), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await dbTransaction.CommitAsync(cancellationToken);
            // Return the mapped DTO
            var dto = _mapper.Map<BoardListDto>(created);
            return dto;
        }
    }
}
