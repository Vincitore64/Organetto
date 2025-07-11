using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Organetto.Core.Authentication.Ports.Data;
using Organetto.Core.Authentication.Ports.Services;
using Organetto.Infrastructure.Infrastructure.Authentication.Exceptions;
using Organetto.Infrastructure.Infrastructure.Authentication.Extensions;
using Organetto.Infrastructure.Infrastructure.Firebase.Data;
using Organetto.UseCases.Shared.Extensions;
using System.Text;
using System.Text.Json;

namespace Organetto.Infrastructure.Infrastructure.Authentication.Services
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly FirebaseAuth _auth;
        private readonly FirebaseSettings _settings;
        private readonly HttpClient _http;

        public AuthenticationService(FirebaseApp app, IOptions<FirebaseSettings> options, HttpClient httpClient)
        {
            _auth = FirebaseAuth.GetAuth(app);
            _http = httpClient;
            _settings = options.Value;
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

            try
            {
                var user = await _auth.CreateUserAsync(args, ct);
                return user.Uid; // we return Firebase UID so domain can map profile if needed
            }
            catch (FirebaseAuthException ex)
            {
                throw new AuthenticationException(
                    (int)(ex.HttpResponse?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError),
                    "Register failed",
                    ex.AuthErrorCode.HasValue ? ex.AuthErrorCode.Value.ToString() : "UNKNOWN",
                    "Register failed on Google"
                );
            }
        }

        public async Task<TokenResponse> LoginUserAsync(string email, string password)
        {
            var requestBody = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_settings.ApiKey}";
            using var response = await _http.PostAsync(url, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));

            await response.EnsureAuthenticationSuccessStatusCodeAsync("Login failed", "Login failed on Google");

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json).RootElement;
            var expiresInSeconds = int.Parse(doc.GetProperty("expiresIn").GetString()!);
            var expiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds);
            
            return new TokenResponse(
                AccessToken: doc.GetProperty("idToken").GetString()!,
                RefreshToken: doc.GetProperty("refreshToken").GetString()!,
                ExpiresAt: expiresAt,
                Uuid: doc.GetProperty("localId").GetString().ThrowIfNull()
            );
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var body = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };

            var url = $"https://securetoken.googleapis.com/v1/token?key={_settings.ApiKey}";
            using var response = await _http.PostAsync(url, new FormUrlEncodedContent(body));
            await response.EnsureAuthenticationSuccessStatusCodeAsync("Refresh token failed", "Refresh token on Google");
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json).RootElement;
            var expiresInSeconds = int.Parse(doc.GetProperty("expires_in").GetString()!);
            var expiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds);
            var uuid = doc.TryGetProperty("user_id", out var userIdProp)
                ? userIdProp.GetString().ThrowIfNull()
                : doc.TryGetProperty("localId", out var localIdProp)
                    ? localIdProp.GetString().ThrowIfNull()
                    : throw new AuthenticationException(500, "Refresh token failed", "USER_UUID_NOT_FOUND", "User ID not found in response");
            
            return new TokenResponse(
                AccessToken: doc.GetProperty("id_token").GetString()!,
                RefreshToken: doc.GetProperty("refresh_token").GetString()!,
                ExpiresAt: expiresAt,
                Uuid: uuid
            );
        }

        public Task SetCustomUserClaimsAsync(string uid, IDictionary<string, object> claims) =>
            FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims.AsReadOnly());
    }
}
