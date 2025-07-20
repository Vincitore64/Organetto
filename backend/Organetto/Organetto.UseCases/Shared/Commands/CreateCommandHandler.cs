using AutoMapper;
using MediatR;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Shared.Models;
using Organetto.Core.Shared.Services;

namespace Organetto.UseCases.Shared.Commands
{
    /// <summary>
    /// Base handler for "Create" commands: maps the request to an entity, persists it, and returns a DTO.
    /// </summary>
    public class CreateCommandHandler<TCommand, TEntity, TDto>
        : IRequestHandler<TCommand, TDto>
        where TCommand : IRequest<TDto>
        where TEntity : class, ICrudEntity
    {
        private readonly IMapper _mapper;
        private readonly ICreateRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCommandHandler(ICreateRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._unitOfWork = unitOfWork;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public virtual async Task<TDto> Handle(TCommand request, CancellationToken cancellationToken)
        {
            // 1. Map request to domain entity
            var entity = _mapper.Map<TEntity>(request);

            // 2. Persist
            await _repository.CreateAsync(entity, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            entity.MarkCreated();

            // 3. Map to DTO
            return _mapper.Map<TDto>(entity);
        }
    }
}
