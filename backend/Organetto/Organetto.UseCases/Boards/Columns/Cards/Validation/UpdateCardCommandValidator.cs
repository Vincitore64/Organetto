using FluentValidation;
using Organetto.UseCases.Boards.Columns.Cards.Commands;

namespace Organetto.UseCases.Boards.Columns.Cards.Validation
{
    public class UpdateCardCommandValidator : AbstractValidator<UpdateCardCommand>
    {
        public UpdateCardCommandValidator()
        {
            // Ensure we know which column and card we're updating
            RuleFor(x => x.ColumnId)
                .GreaterThan(0)
                .WithMessage("ColumnId must be a positive number.");

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Card Id must be a positive number.");

            // Title: if provided, it cannot be empty or too long
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title, if specified, cannot be empty.")
                .MaximumLength(255)
                .WithMessage("Title must not exceed 255 characters.")
                .When(x => x.Title != null);

            // Description: if provided, cap length
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description must not exceed 1000 characters.")
                .When(x => x.Description != null);

            //// AssigneeId: if provided, must be positive
            //RuleFor(x => x.AssigneeId)
            //    .GreaterThan(0)
            //    .WithMessage("AssigneeId, if specified, must be a positive number.")
            //    .When(x => x.AssigneeId.HasValue);

            // Position: if provided, must be zero or greater
            RuleFor(x => x.Position)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Position, if specified, must be zero or greater.")
                .When(x => x.Position.HasValue);

            // DueDate: if provided, must be in the future
            RuleFor(x => x.DueDate)
                .Must(due => !due.HasValue || due.Value >= DateTimeOffset.UtcNow)
                .WithMessage("DueDate, if specified, must be in the future.");
        }
    }
}
