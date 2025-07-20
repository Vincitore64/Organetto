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
                            ? new List<DueDate>
                            {
                                new DueDate
                                {
                                    DueAt      = src.DueDate.Value.UtcDateTime,
                                    IsComplete = false
                                }
                            }
                            : new List<DueDate>()
                    ));

            // MemberList.None lets us explicitly map only those members we care about.
            CreateMap<UpdateCardCommand, Card>()
                // Always update the column (BoardList) FK
                .ForMember(dest => dest.BoardListId,
                           opt => opt.MapFrom(src => src.ColumnId))

                // Title → Card.Title, only if Title was supplied
                .ForMember(dest => dest.Title, opt =>
                {
                    opt.PreCondition(src => src.Title != null);
                    opt.MapFrom(src => src.Title!);
                })

                // Description → Card.Description, only if supplied
                .ForMember(dest => dest.Description, opt =>
                {
                    opt.PreCondition(src => src.Description != null);
                    opt.MapFrom(src => src.Description!);
                })

                // Position → Card.Position, only if supplied
                .ForMember(dest => dest.Position, opt =>
                {
                    opt.PreCondition(src => src.Position.HasValue);
                    opt.MapFrom(src => src.Position!.Value);
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())

                // DueDate: handled in AfterMap to clear/add a single DueDate entry
                .AfterMap((src, dest) =>
                {
                    if (src.DueDate.HasValue)
                    {
                        dest.DueDates.Clear();
                        dest.DueDates.Add(new DueDate
                        {
                            DueAt = src.DueDate.Value.UtcDateTime,
                            IsComplete = false
                        });
                    }
                });

        }
    }
}
