using FluentValidation;
using Organetto.UseCases.Shared.Exceptions.Models;
using System.ComponentModel.DataAnnotations;

namespace Organetto.UseCases.Shared.Validation.Extensions
{
    public static class ValidatorExtensions
    {
        public static void TryValidate<T>(this T model, bool validateAllProperties = false)
            where T : IValidatableObject
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            if (!Validator.TryValidateObject(model, context, results, validateAllProperties))
            {
                throw AppValidationException.FromMessage(string.Join('\n', results.Select(item => item.ErrorMessage)));
            }
        }

        public static string ValidationConcat(this IEnumerable<FluentValidation.Results.ValidationFailure> failures)
        {
            return string.Join("\n", failures.Select(f => f.ErrorMessage));
        }

        public static void TryValidate<T>(this IValidator<T> validator, T validatingObject)
        {
            var result = validator.Validate(validatingObject);
            if (result.IsValid) return;
            throw AppValidationException.FromMessage(result.Errors.ValidationConcat());
        }

        public static async Task TryValidateAsync<T>(this IValidator<T> validator, T validatingObject, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(validatingObject, cancellationToken);
            if (result.IsValid) return;
            throw AppValidationException.FromMessage(result.Errors.ValidationConcat());
        }
    }
}
