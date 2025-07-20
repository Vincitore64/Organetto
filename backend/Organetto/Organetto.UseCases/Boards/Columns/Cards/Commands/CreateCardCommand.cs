using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Shared.Commands;
using Organetto.UseCases.Shared.Outbox.Services;
using Organetto.UseCases.Shared.Validation.Extensions;

namespace Organetto.UseCases.Boards.Columns.Cards.Commands
{
    /// <summary>
    /// Command to create a new card in a given board and column.
    /// </summary>
    public record CreateCardCommand(
        long BoardId,
        long ColumnId,
        string Title,
        string? Description,
        long Position,
        DateTimeOffset? DueDate
    ) : IRequest<CardDto>;


    /// <summary>
    /// Handler that creates a new card.
    /// Wrapped by TransactionDecorator and OutboxIntegrationEventDecorator.
    /// </summary>
    public class CreateCardCommandHandler
        : IRequestHandler<CreateCardCommand, CardDto>
    {
        private readonly ICardRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOutboxService _outboxService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCardCommand> _validator;
        private readonly ILoggerFactory _loggerFactory;

        public CreateCardCommandHandler(
            ICardRepository repository,
            IMapper mapper,
            IOutboxService outboxService,
            IUnitOfWork unitOfWork,
            IValidator<CreateCardCommand> validator,
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
            CreateCardCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Validate
            _validator.TryValidate(request);

            // 2. Core business logic: map & persist
            var businessHandler = new CreateCommandHandler<CreateCardCommand, Card, CardDto>(
                _repository,
                _unitOfWork,
                _mapper
            );

            // 3. Outbox decorator: enqueue any integration events
            var outboxHandler = new OutboxIntegrationEventDecorator<CreateCardCommand, CardDto>(
                businessHandler,
                _unitOfWork,
                _outboxService,
                _loggerFactory.CreateLogger<OutboxIntegrationEventDecorator<CreateCardCommand, CardDto>>()
            );

            // 4. Transaction decorator: wrap in DB transaction
            var transactionalHandler = new TransactionDecorator<CreateCardCommand, CardDto>(
                outboxHandler,
                _unitOfWork,
                _loggerFactory.CreateLogger<TransactionDecorator<CreateCardCommand, CardDto>>()
            );

            // 5. Execute the full pipeline
            return await transactionalHandler.Handle(request, cancellationToken);
        }
    }
}
