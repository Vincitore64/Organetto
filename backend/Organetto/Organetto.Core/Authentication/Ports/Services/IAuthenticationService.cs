using Organetto.Core.Authentication.Ports.Data;

namespace Organetto.Core.Authentication.Ports.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Creates a new Firebase user and returns its UID.
        /// </summary>
        Task<string> RegisterUserAsync(string email, string password, string? displayName = null, CancellationToken ct = default);
        Task<TokenResponse> LoginUserAsync(string email, string password);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
        Task SetCustomUserClaimsAsync(string uid, IDictionary<string, object> claims);
    }
}
