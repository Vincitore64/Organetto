using Organetto.UseCases.Shared.Exceptions.Models;

namespace Organetto.Infrastructure.Infrastructure.Authentication.Exceptions
{
    public class AuthenticationException : AppException
    {
        public AuthenticationException(int status, string title, string code, string message) : base(status, title, code, message, null, null)
        {
        }
    }
}
