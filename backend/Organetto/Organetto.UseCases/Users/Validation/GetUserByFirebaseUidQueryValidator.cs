using FluentValidation;
using Organetto.UseCases.Users.Queries;

namespace Organetto.UseCases.Users.Validation
{
    public class GetUserByFirebaseUidQueryValidator : AbstractValidator<GetUserByFirebaseUidQuery>
    {
        public GetUserByFirebaseUidQueryValidator()
        {
            RuleFor(q => q.FirebaseUid)
                .NotEmpty()
                .WithMessage("FirebaseUid must be provided.")                                       // Должен быть указан FirebaseUid.
                .Must(uid => Guid.TryParse(uid, out _))
                .WithMessage("FirebaseUid must be a valid GUID.");                                 // FirebaseUid должен быть корректным GUID.
        }
    }
}
