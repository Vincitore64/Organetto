using FluentValidation;
using Organetto.UseCases.Boards.Columns.Cards.Commands;

namespace Organetto.UseCases.Boards.Columns.Cards.Validation
{
    public class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
    {
        public CreateCardCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .GreaterThan(0)
                .WithMessage("BoardId must be a positive number.");

            RuleFor(x => x.ColumnId)
                .GreaterThan(0)
                .WithMessage("ColumnId must be a positive number.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(255)
                .WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Position)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Position must be zero or greater.");

            RuleFor(x => x.DueDate)
                .Must(due => !due.HasValue || due.Value >= DateTimeOffset.UtcNow)
                .WithMessage("Due date, if specified, must be in the future.");
        }
    }
}
