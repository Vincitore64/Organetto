using AutoMapper;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Cards.Data;
using Organetto.UseCases.Boards.Data;
using System.Linq;

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

            // Map Board entity → BoardDetailDto. (Маппим Board → BoardDetailDto.)
            CreateMap<Board, BoardDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members))
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Lists));

            // Map Core.BoardMember → UseCases.BoardMember. (Маппим Core.BoardMember → UseCases.BoardMember.)
            CreateMap<Core.Boards.Data.BoardMember, UseCases.Boards.Data.BoardMember>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            // Map BoardList → BoardListDto. (Маппим BoardList → BoardListDto.)
            CreateMap<BoardList, BoardListDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Cards, opt => opt.MapFrom(src => src.Cards));

            // Map Card → CardDto. (Маппим Card → CardDto.)
            CreateMap<Card, CardDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDates.FirstOrDefault() != null ? src.DueDates.FirstOrDefault()!.DueAt : (DateTime?)null));
        }
    }
}
