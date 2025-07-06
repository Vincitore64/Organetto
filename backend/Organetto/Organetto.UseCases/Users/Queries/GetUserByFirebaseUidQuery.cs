using AutoMapper;
using MediatR;
using Organetto.Core.Users.Services;
using Organetto.UseCases.Users.Data;

namespace Organetto.UseCases.Users.Queries
{
    public record GetUserByFirebaseUidQuery(string FirebaseUid) : IRequest<UserDto>
    {
    }

    public class GetUserByFirebaseUidQueryHandler : IRequestHandler<GetUserByFirebaseUidQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByFirebaseUidQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByFirebaseUidQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByFirebaseUidAsync(request.FirebaseUid);

            return _mapper.Map<UserDto>(user);  // Map entity to DTO (Маппим сущность в DTO)
        }
    }
}
