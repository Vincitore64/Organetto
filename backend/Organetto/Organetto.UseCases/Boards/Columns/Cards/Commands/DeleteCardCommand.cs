using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Shared.Models;
using Organetto.UseCases.Shared.Commands;
using Organetto.UseCases.Shared.Outbox.Services;
using Organetto.UseCases.Shared.Validation.Extensions;

namespace Organetto.UseCases.Boards.Columns.Cards.Commands
{
    /// <summary>
    /// Command to delete an existing card.
    /// </summary>
    public record DeleteCardCommand(
        long ColumnId,
        long Id
    ) : IRequest<Unit>, IHasId<long>;

    /// <summary>
    /// Handler that deletes a card by Id.
    /// Wrapped by TransactionDecorator and OutboxIntegrationEventDecorator.
    /// </summary>
    public class DeleteCardCommandHandler
        : IRequestHandler<DeleteCardCommand, Unit>
    {
        private readonly ICardRepository _repository;
        private readonly IOutboxService _outboxService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteCardCommand> _validator;
        private readonly ILoggerFactory _loggerFactory;

        public DeleteCardCommandHandler(
            ICardRepository repository,
            IOutboxService outboxService,
            IUnitOfWork unitOfWork,
            IValidator<DeleteCardCommand> validator,
            ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _outboxService = outboxService;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _loggerFactory = loggerFactory;
        }

        public async Task<Unit> Handle(
            DeleteCardCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Validate input
            _validator.TryValidate(request);

            // 2. Core delete logic
            var businessHandler = new DeleteCommandHandler<DeleteCardCommand, Card, long>(
                _repository
            );

            // 3. Enqueue any domain/integration events
            var outboxHandler = new OutboxIntegrationEventDecorator<DeleteCardCommand, Unit>(
                businessHandler,
                _unitOfWork,
                _outboxService,
                _loggerFactory.CreateLogger<OutboxIntegrationEventDecorator<DeleteCardCommand, Unit>>()
            );

            // 4. Wrap in a DB transaction
            var transactionalHandler = new TransactionDecorator<DeleteCardCommand, Unit>(
                outboxHandler,
                _unitOfWork,
                _loggerFactory.CreateLogger<TransactionDecorator<DeleteCardCommand, Unit>>()
            );

            // 5. Execute full pipeline
            return await transactionalHandler.Handle(request, cancellationToken);
        }
    }

}
