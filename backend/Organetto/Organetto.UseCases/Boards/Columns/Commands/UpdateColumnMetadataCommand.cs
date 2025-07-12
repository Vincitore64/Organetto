using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Shared.Models;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Boards.Columns.IntergationEvents.Factories;
using Organetto.UseCases.Shared.Commands;
using Organetto.UseCases.Shared.Outbox.Services;

namespace Organetto.UseCases.Boards.Columns.Commands
{
    /// <summary>
    /// Command to update an existing column's metadata (title and position).
    /// </summary>
    public record UpdateColumnMetadataCommand(
        long BoardId,
        long Id,
        string Title,
        int Position
    ) : IRequest<BoardListDto>, IHasId<long>;

    /// <summary>
    /// Handler that updates a column's Title and Position.
    /// Wrapped by TransactionDecorator and OutboxIntegrationEventDecorator.
    /// </summary>
    public class UpdateColumnMetadataCommandHandler
        : IRequestHandler<UpdateColumnMetadataCommand, BoardListDto>
    {
        private readonly IColumnRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOutboxService _outboxService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerFactory _loggerFactory;

        public UpdateColumnMetadataCommandHandler(
            IColumnRepository repository,
            IMapper mapper,
            IOutboxService outboxService,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._outboxService = outboxService;
            this._unitOfWork = unitOfWork;
            this._loggerFactory = loggerFactory;
        }

        public async Task<BoardListDto> Handle(
            UpdateColumnMetadataCommand request,
            CancellationToken cancellationToken)
        {
            var bussinessHandler = new UpdateCommandHandler<UpdateColumnMetadataCommand, BoardList, BoardListDto, long>(_repository, _mapper);
            var outboxHandler = new OutboxIntegrationEventDecorator<UpdateColumnMetadataCommand, BoardListDto>(
                bussinessHandler,
                _unitOfWork,
                _outboxService,
                new ColumnMetadataUpdatedEventFactory(),
                _loggerFactory.CreateLogger<OutboxIntegrationEventDecorator<UpdateColumnMetadataCommand, BoardListDto>>()
            );
            var transactionalHandler = new TransactionDecorator<UpdateColumnMetadataCommand, BoardListDto>(
                outboxHandler,
                _unitOfWork,
                _loggerFactory.CreateLogger<TransactionDecorator<UpdateColumnMetadataCommand, BoardListDto>>()
            );
            return await transactionalHandler.Handle(request, cancellationToken);
        }
    }

}
