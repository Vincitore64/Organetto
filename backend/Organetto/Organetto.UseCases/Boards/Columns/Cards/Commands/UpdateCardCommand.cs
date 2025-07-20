using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Shared.Models;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Shared.Commands;
using Organetto.UseCases.Shared.Outbox.Services;
using Organetto.UseCases.Shared.Validation.Extensions;

namespace Organetto.UseCases.Boards.Columns.Cards.Commands
{
    /// <summary>
    /// Command to update an existing card's metadata.
    /// </summary>
    public record UpdateCardCommand(
        long ColumnId,
        long Id,
        string? Title,
        string? Description,
        int? Position,
        DateTimeOffset? DueDate
    ) : IRequest<CardDto>, IHasId<long>;

    /// <summary>
    /// Handler that updates card fields (title, description, assignee, position, due date).
    /// Wrapped by TransactionDecorator and OutboxIntegrationEventDecorator.
    /// </summary>
    public class UpdateCardCommandHandler
        : IRequestHandler<UpdateCardCommand, CardDto>
    {
        private readonly ICardRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOutboxService _outboxService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCardCommand> _validator;
        private readonly ILoggerFactory _loggerFactory;

        public UpdateCardCommandHandler(
            ICardRepository repository,
            IMapper mapper,
            IOutboxService outboxService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateCardCommand> validator,
            ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _outboxService = outboxService;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _loggerFactory = loggerFactory;
        }

        public async Task<CardDto> Handle(
            UpdateCardCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Validate input
            _validator.TryValidate(request);

            // 2. Core update logic
            var businessHandler = new UpdateCommandHandler<UpdateCardCommand, Card, CardDto, long>(
                _repository,
                _mapper
            );

            // 3. Outbox for integration events
            var outboxHandler = new OutboxIntegrationEventDecorator<UpdateCardCommand, CardDto>(
                businessHandler,
                _unitOfWork,
                _outboxService,
                _loggerFactory.CreateLogger<OutboxIntegrationEventDecorator<UpdateCardCommand, CardDto>>()
            );

            // 4. Wrap in transaction
            var transactionalHandler = new TransactionDecorator<UpdateCardCommand, CardDto>(
                outboxHandler,
                _unitOfWork,
                _loggerFactory.CreateLogger<TransactionDecorator<UpdateCardCommand, CardDto>>()
            );

            // 5. Execute full pipeline
            return await transactionalHandler.Handle(request, cancellationToken);
        }
    }
}
