using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Shared.Models;
using Organetto.UseCases.Shared.Commands;
using Organetto.UseCases.Shared.Outbox.Services;
using Organetto.UseCases.Shared.Validation.Extensions;

namespace Organetto.UseCases.Boards.Columns.Commands
{
    /// <summary>
    /// Command to delete a column (BoardList) from a board.
    /// </summary>
    public record DeleteColumnCommand(long BoardId, long Id) : IRequest<Unit>, IHasId<long>;

    /// <summary>
    /// Handles the deletion of a column by delegating to the Board aggregate.
    /// </summary>
    public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, Unit>
    {
        private readonly IColumnRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOutboxService _outboxService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteColumnCommand> _validator;
        private readonly ILoggerFactory _loggerFactory;

        public DeleteColumnCommandHandler(IColumnRepository repository,
            IMapper mapper,
            IOutboxService outboxService,
            IUnitOfWork unitOfWork,
            IValidator<DeleteColumnCommand> validator,
            ILoggerFactory loggerFactory)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._outboxService = outboxService;
            this._unitOfWork = unitOfWork;
            this._validator = validator;
            this._loggerFactory = loggerFactory;
        }

        public async Task<Unit> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
        {
            _validator.TryValidate(request);
            var bussinessHandler = new DeleteCommandHandler<DeleteColumnCommand, BoardList, long>(_repository);
            var outboxHandler = new OutboxIntegrationEventDecorator<DeleteColumnCommand, Unit>(
                bussinessHandler,
                _unitOfWork,
                _outboxService,
                _loggerFactory.CreateLogger<OutboxIntegrationEventDecorator<DeleteColumnCommand, Unit>>()
            );
            var transactionalHandler = new TransactionDecorator<DeleteColumnCommand, Unit>(
                outboxHandler,
                _unitOfWork,
                _loggerFactory.CreateLogger<TransactionDecorator<DeleteColumnCommand, Unit>>()
            );

            return await transactionalHandler.Handle(request, cancellationToken);
        }
    }

}
