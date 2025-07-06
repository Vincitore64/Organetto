using AutoMapper;
using Organetto.Core.Boards.Data;
using Organetto.UseCases.Boards.Data;

namespace Organetto.UseCases.Boards.Services
{
    /// <summary>
    /// AutoMapper profile for Board mappings. (Профиль AutoMapper для маппинга досок.)
    /// </summary>
    public class BoardMappingProfile : Profile
    {
        public BoardMappingProfile()
        {
            // Map Board entity → BoardDto. (Маппим Board → BoardDto.)
            CreateMap<Board, BoardDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => src.IsArchived));
        }
    }
}
