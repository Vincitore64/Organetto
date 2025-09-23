namespace Organetto.UseCases.Shared.Exceptions.Models
{
    // define an interface for exceptions that carry a custom code
    public interface IHasErrorCode
    {
        string Code { get; }
    }
}
