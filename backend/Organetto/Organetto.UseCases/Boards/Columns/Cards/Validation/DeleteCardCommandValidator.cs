using FluentValidation;
using Organetto.UseCases.Boards.Columns.Cards.Commands;

namespace Organetto.UseCases.Boards.Columns.Cards.Validation
{
    /// <summary>
    /// Validator for DeleteCardCommand.
    /// </summary>
    public class DeleteCardCommandValidator : AbstractValidator<DeleteCardCommand>
    {
        public DeleteCardCommandValidator()
        {
            RuleFor(x => x.ColumnId)
                .GreaterThan(0)
                .WithMessage("ColumnId must be a positive number.");

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Card Id must be a positive number.");
        }
    }
}
