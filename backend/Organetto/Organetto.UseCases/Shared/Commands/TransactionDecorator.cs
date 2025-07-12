using MediatR;
using Microsoft.Extensions.Logging;
using Organetto.Core.Shared.Databases;

namespace Organetto.UseCases.Shared.Commands
{
    public sealed class TransactionDecorator<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public TransactionDecorator(
            IRequestHandler<TRequest, TResponse> inner,
            IUnitOfWork unitOfWork,
            ILogger<TransactionDecorator<TRequest, TResponse>> logger)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await _inner.Handle(request, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await tx.CommitAsync(cancellationToken);
                _logger.LogInformation("Transaction committed for {Command}", typeof(TRequest).Name);

                return response;
            }
            catch
            {
                _logger.LogWarning("Transaction rolling back for {Command}", typeof(TRequest).Name);
                await tx.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
