namespace Organetto.Web.Authentication.Data
{
    public sealed class RegisterUserData
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }
}
