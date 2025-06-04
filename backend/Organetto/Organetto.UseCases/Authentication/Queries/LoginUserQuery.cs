using MediatR;
using Organetto.Core.Authentication.Ports.Data;
using Organetto.Core.Authentication.Ports.Services;

namespace Organetto.UseCases.Authentication.Queries
{
    // -------- Login ----------------------------------------------------------
    public record LoginUserQuery(string Email, string Password) : IRequest<TokenResponse>;

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, TokenResponse>
    {
        private readonly IAuthenticationService _auth;
        public LoginUserQueryHandler(IAuthenticationService auth) => _auth = auth;

        public async Task<TokenResponse> Handle(LoginUserQuery request, CancellationToken ct)
            => await _auth.LoginUserAsync(request.Email, request.Password);
    }
}
