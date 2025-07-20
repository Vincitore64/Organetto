using AutoMapper;
using Organetto.Core.Boards.Cards.Data;
using Organetto.UseCases.Boards.Columns.Cards.Commands;

namespace Organetto.UseCases.Boards.Columns.Cards.Mappers
{
    public class CardsProfile : Profile
    {
        public CardsProfile()
        {
            CreateMap<CreateCardCommand, Card>()
                // Command.ColumnId → Card.BoardListId
                .ForMember(dest => dest.BoardListId,
                           opt => opt.MapFrom(src => src.ColumnId))
                // Title and Description (null → empty string)
                .ForMember(dest => dest.Title,
                           opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description,
                           opt => opt.MapFrom(src => src.Description ?? string.Empty))
                // Position index
                .ForMember(dest => dest.Position,
                           opt => opt.MapFrom(src => src.Position))
                // Single DueDate → collection of DueDate entities
                .ForMember(dest => dest.DueDates,
                           opt => opt.MapFrom(src =>
                               src.DueDate.HasValue
                                   ? new List<DueDate> {
                                         new DueDate {
                                             DueAt      = src.DueDate.Value.UtcDateTime,
                                             IsComplete = false
                                         }
                                     }
                                   : new List<DueDate>()
                           ));
        }
    }
}
