using AutoMapper;
using MediatR;
using Organetto.Core.Shared.Models;
using Organetto.Core.Shared.Services;

namespace Organetto.UseCases.Shared.Commands
{
    /// <summary>
    /// Base handler for "Update" commands: loads by Id, maps updates, persists, and returns a DTO.
    /// </summary>
    public class UpdateCommandHandler<TCommand, TEntity, TDto, TKey>
        : IRequestHandler<TCommand, TDto>
        where TCommand : IRequest<TDto>, IHasId<TKey>
        where TEntity : class
    {
        private readonly IReadByIdAndUpdateRepository<TEntity, TKey> _repository;
        private readonly IMapper _mapper;

        public UpdateCommandHandler(IReadByIdAndUpdateRepository<TEntity, TKey> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public virtual async Task<TDto> Handle(TCommand request, CancellationToken cancellationToken)
        {
            // 1. Load existing entity
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            // 2. Map modifications onto entity
            _mapper.Map(request, entity);

            // 3. Persist
            await _repository.UpdateAsync(entity, cancellationToken);

            // 4. Map to DTO and return
            return _mapper.Map<TDto>(entity);
        }
    }
}
