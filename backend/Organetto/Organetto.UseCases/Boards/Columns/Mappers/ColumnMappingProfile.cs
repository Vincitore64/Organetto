using AutoMapper;
using Organetto.Core.Boards.Data;
using Organetto.Core.Boards.Events;
using Organetto.UseCases.Boards.Columns.Commands;
using Organetto.UseCases.Boards.Columns.IntergationEvents;

namespace Organetto.UseCases.Boards.Columns.Mappers
{
    /// <summary>
    /// AutoMapper profile for mapping column-related commands to domain entities.
    /// </summary>
    public class ColumnMappingProfile : Profile
    {
        public ColumnMappingProfile()
        {
            // Map only Title and Position from UpdateColumnMetadataCommand to BoardList
            CreateMap<UpdateColumnMetadataCommand, BoardList>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                // Ignore all other properties to prevent accidental overwrites
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BoardId, opt => opt.Ignore())
                .ForMember(dest => dest.Cards, opt => opt.Ignore())
                .ForMember(dest => dest.Board, opt => opt.Ignore());

            CreateMap<BoardListUpdatedDomainEvent, BoardListUpdatedIntegrationEvent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.BoardListId));

            // Optionally, if you have a CreateColumnCommand, map its properties as well:
            // CreateMap<CreateColumnCommand, BoardList>()
            //     .ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.BoardId))
            //     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //     .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
            //     .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
