namespace Organetto.Infrastructure.Infrastructure.AWS.Configuration.Options;

public class S3Config
{
    public string ServiceURL { get; set; } = string.Empty;
    public string ForcePathStyle { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}