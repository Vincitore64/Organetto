using FluentValidation;
using Organetto.UseCases.Boards.Commands;

namespace Organetto.UseCases.Boards.Validation
{
    /// <summary>
    /// Validator for UpdateBoardMetadataCommand.
    /// </summary>
    public class UpdateBoardMetadataCommandValidator : AbstractValidator<UpdateBoardMetadataCommand>
    {
        public UpdateBoardMetadataCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .GreaterThan(0).WithMessage("BoardId must be greater than zero.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(128).WithMessage("Title must be at most 128 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must be at most 1000 characters.");
        }
    }
}
