namespace Organetto.Core.Authentication.Ports.Data
{
    public record TokenResponse(string AccessToken, string RefreshToken, DateTimeOffset ExpiresAt, string Uuid);
}
