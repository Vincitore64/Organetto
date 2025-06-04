namespace Organetto.Web.Authentication.Data
{
    //public sealed class RegisterRequest
    //{
    //    public string Email { get; set; } = string.Empty;
    //    public string Password { get; set; } = string.Empty;
    //    public string? DisplayName { get; set; }
    //}
    public record RegisterRequest(string Email, string Password, string? DisplayName);
    public record LoginRequest(string Email, string Password);
    public record RefreshRequest(string RefreshToken);
}
