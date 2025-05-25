using MediatR;
using Organetto.Core.Authentication.Ports.Data;
using Organetto.Core.Authentication.Ports.Services;

namespace Organetto.UseCases.Authentication.Commands
{
    // -------- Refresh Token --------------------------------------------------
    public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponse>;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
    {
        private readonly IAuthenticationService _auth;
        public RefreshTokenCommandHandler(IAuthenticationService auth) => _auth = auth;

        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
            => await _auth.RefreshTokenAsync(request.RefreshToken);
    }
}
