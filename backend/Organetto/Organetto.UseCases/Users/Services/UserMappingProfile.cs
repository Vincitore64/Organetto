using AutoMapper;
using Organetto.Core.Users.Data;
using Organetto.UseCases.Users.Data;

namespace Organetto.UseCases.Users.Services
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
