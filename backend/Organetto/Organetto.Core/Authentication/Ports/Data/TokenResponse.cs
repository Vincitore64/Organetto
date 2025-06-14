namespace Organetto.Core.Authentication.Ports.Data
{
    public record TokenResponse(string AccessToken, string RefreshToken, int ExpiresIn, string Uuid);
}
