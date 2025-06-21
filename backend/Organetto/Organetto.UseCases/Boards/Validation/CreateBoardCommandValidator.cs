using FluentValidation;
using Organetto.UseCases.Boards.Commands;

namespace Organetto.UseCases.Boards.Validation
{
    public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
    {
        public CreateBoardCommandValidator()
        {
            RuleFor(x => x.OwnerId)
                .GreaterThan(0)
                .WithMessage("OwnerId must be a positive integer.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title must be at most 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description must be at most 1000 characters.")
                .When(x => x.Description != null);
        }
    }
}
