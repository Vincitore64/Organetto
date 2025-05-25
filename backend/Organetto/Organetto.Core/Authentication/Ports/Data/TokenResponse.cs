namespace Organetto.Core.Authentication.Ports.Data
{
    public record TokenResponse(string IdToken, string RefreshToken, int ExpiresIn);
}
