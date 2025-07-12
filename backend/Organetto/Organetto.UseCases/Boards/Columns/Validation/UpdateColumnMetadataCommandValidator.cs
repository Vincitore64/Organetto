using FluentValidation;
using Organetto.UseCases.Boards.Columns.Commands;

namespace Organetto.UseCases.Boards.Columns.Validation
{
    /// <summary>
    /// Validator for UpdateColumnMetadataCommand.
    /// </summary>
    public class UpdateColumnMetadataCommandValidator : AbstractValidator<UpdateColumnMetadataCommand>
    {
        public UpdateColumnMetadataCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .GreaterThan(0).WithMessage("BoardId must be greater than zero.");

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ColumnId must be greater than zero.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(256).WithMessage("Title must be at most 256 characters.");

            RuleFor(x => x.Position)
                .GreaterThanOrEqualTo(0).WithMessage("Position must be non-negative.");
        }
    }
}
