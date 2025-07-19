using MediatR;
using Organetto.Core.Shared.Models;
using Organetto.Core.Shared.Services;

namespace Organetto.UseCases.Shared.Commands
{
    /// <summary>
    /// Base handler for "Delete" commands: loads by Id (optional), deletes, and returns Unit.
    /// </summary>
    public class DeleteCommandHandler<TCommand, TEntity, TKey>
        : IRequestHandler<TCommand, Unit>
        where TCommand : IRequest<Unit>, IHasId<TKey>
        where TEntity : class, ICrudEntity
    {
        private readonly IReadByIdAndDeleteRepository<TEntity, TKey> _repository;

        public DeleteCommandHandler(
            IReadByIdAndDeleteRepository<TEntity, TKey> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public virtual async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            // 1. Optionally verify existence
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            // 2. Delete
            await _repository.DeleteAsync(request.Id, cancellationToken);

            entity.MarkDeleted();

            return Unit.Value;
        }
    }
}
