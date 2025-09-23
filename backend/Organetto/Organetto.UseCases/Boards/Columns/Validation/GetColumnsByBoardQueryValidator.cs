using FluentValidation;
using Organetto.UseCases.Boards.Columns.Queries;

namespace Organetto.UseCases.Boards.Columns.Validation
{
    /// <summary>
    /// Ensures BoardId is valid.
    /// </summary>
    public class GetColumnsByBoardQueryValidator : AbstractValidator<GetColumnsByBoardQuery>
    {
        public GetColumnsByBoardQueryValidator()
        {
            RuleFor(x => x.BoardId)
                .GreaterThan(0).WithMessage("BoardId must be greater than zero.");
        }
    }
}
