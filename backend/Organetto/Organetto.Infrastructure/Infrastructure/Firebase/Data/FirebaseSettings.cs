namespace Organetto.Infrastructure.Infrastructure.Firebase.Data
{
    public class FirebaseSettings
    {
        public const string SectionName = "Firebase";
        public string ProjectId { get; set; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty; // For REST login / refresh
        public string CredentialsPath { get; set; } = string.Empty;
        public string ServiceAccountPath { get; init; } = string.Empty; // JSON file or base64
    }
}
