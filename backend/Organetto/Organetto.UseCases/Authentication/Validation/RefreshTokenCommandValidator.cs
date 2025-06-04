using FluentValidation;
using Organetto.UseCases.Authentication.Commands;

namespace Organetto.UseCases.Authentication.Validation
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
