using FluentValidation;
using Organetto.UseCases.Boards.Columns.Commands;

namespace Organetto.UseCases.Boards.Columns.Validation
{
    /// <summary>
    /// Validator for DeleteColumnCommand.
    /// </summary>
    public class DeleteColumnCommandValidator : AbstractValidator<DeleteColumnCommand>
    {
        public DeleteColumnCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .GreaterThan(0).WithMessage("BoardId must be greater than zero.");
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ColumnId must be greater than zero.");
        }
    }
}
