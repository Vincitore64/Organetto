using FluentValidation;
using Organetto.UseCases.Boards.Commands;

namespace Organetto.UseCases.Boards.Validation
{
    /// <summary>
    /// Validates DeleteBoardCommand. (Валидатор для DeleteBoardCommand.)
    /// </summary>
    public class DeleteBoardCommandValidator : AbstractValidator<DeleteBoardCommand>
    {
        public DeleteBoardCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .GreaterThan(0)
                .WithMessage("BoardId must be a positive integer.");
            // (BoardId должен быть положительным целым.)

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be a positive integer.");
            // (UserId должен быть положительным целым.)
        }
    }
}
