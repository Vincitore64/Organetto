using MediatR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Shared.Databases;
using Organetto.UseCases.Shared.IntegrationEvents.Factories;
using Organetto.UseCases.Shared.Outbox.Services;

namespace Organetto.UseCases.Shared.Commands
{
    /// <summary>
    /// Decorator that executes the inner handler, then enqueues integration events
    /// produced by <see cref="IIntegrationEventFactory{TRequest,TResponse}"/> into <see cref="IOutboxService"/>.
    /// </summary>
    public sealed class OutboxIntegrationEventDecorator<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOutboxService _outbox;
        private readonly IIntegrationEventFactory<TRequest, TResponse> _factory;
        private readonly ILogger _logger;

        public OutboxIntegrationEventDecorator(
            IRequestHandler<TRequest, TResponse> inner,
            IUnitOfWork unitOfWork,
            IOutboxService outbox,
            IIntegrationEventFactory<TRequest, TResponse> factory,
            ILogger<OutboxIntegrationEventDecorator<TRequest, TResponse>> logger)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this._unitOfWork = unitOfWork;
            _outbox = outbox ?? throw new ArgumentNullException(nameof(outbox));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            // 1. Execute core business logic
            var response = await _inner.Handle(request, cancellationToken);

            // 2. Build & persist integration events
            foreach (var evt in _factory.Create(request, response))
            {
                await _outbox.AddAsync(evt, cancellationToken);
                _logger.LogInformation("Added integration event {EventType}", evt.GetType().Name);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
