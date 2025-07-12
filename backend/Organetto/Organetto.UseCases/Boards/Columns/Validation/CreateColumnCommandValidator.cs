using FluentValidation;
using Organetto.UseCases.Boards.Columns.Commands;

namespace Organetto.UseCases.Boards.Columns.Validation
{
    /// <summary>
    /// Validator for CreateColumnCommand.
    /// </summary>
    public class CreateColumnCommandValidator : AbstractValidator<CreateColumnCommand>
    {
        public CreateColumnCommandValidator()
        {
            RuleFor(c => c.BoardId)
                .GreaterThan(0).WithMessage("BoardId must be greater than zero.");
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Column title cannot be empty.")
                .MaximumLength(256).WithMessage("Column title must be at most 256 characters.");
            RuleFor(c => c.Position)
                .GreaterThanOrEqualTo(0).WithMessage("Position must be non-negative.");
        }
    }
}
