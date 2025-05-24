using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Organetto.Core.Authentication.Ports.Services;

namespace Organetto.Infrastructure.Infrastructure.Authentication.Services
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly FirebaseAuth _auth;

        public AuthenticationService(FirebaseApp app)
        {
            _auth = FirebaseAuth.GetAuth(app);
        }

        public async Task<string> RegisterUserAsync(string email, string password, string? displayName = null, CancellationToken ct = default)
        {
            var args = new UserRecordArgs
            {
                Email = email,
                EmailVerified = false,
                Password = password,
                DisplayName = displayName,
                Disabled = false
            };

            var user = await _auth.CreateUserAsync(args, ct);
            return user.Uid; // we return Firebase UID so domain can map profile if needed
        }
    }
}
