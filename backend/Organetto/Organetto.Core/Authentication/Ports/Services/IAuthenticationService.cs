namespace Organetto.Core.Authentication.Ports.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Creates a new Firebase user and returns its UID.
        /// </summary>
        Task<string> RegisterUserAsync(string email, string password, string? displayName = null, CancellationToken ct = default);
    }
}
