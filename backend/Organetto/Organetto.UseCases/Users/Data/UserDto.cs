namespace Organetto.UseCases.Users.Data
{
    public class UserDto
    {
        public UserDto()
        {
            FirebaseUid = string.Empty;
            Email = string.Empty;
            Name = string.Empty;
        }

        public long Id { get; set; }              // Surrogate key (суррогатный ключ)
        public string FirebaseUid { get; set; }   // Immutable UID from Firebase (неизменяемый UID из Firebase)
        public string Email { get; set; }         // User email (электронная почта)
        public string Name { get; set; }          // Display name (отображаемое имя)
        public DateTime CreatedAt { get; set; }   // Account creation timestamp (время создания аккаунта)
    }
}
