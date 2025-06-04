using MediatR;
using Organetto.Core.Authentication.Ports.Services;

namespace Organetto.UseCases.Authentication.Commands
{
    /// <summary>
    /// Command to create a new Firebase user.
    /// </summary>
    public sealed record RegisterUserCommand(string Email, string Password, string? DisplayName) : IRequest<string>;

    internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IAuthenticationService _auth;
        public RegisterUserCommandHandler(IAuthenticationService auth) => _auth = auth;

        public Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken) =>
            _auth.RegisterUserAsync(request.Email, request.Password, request.DisplayName, cancellationToken);
    }
}
