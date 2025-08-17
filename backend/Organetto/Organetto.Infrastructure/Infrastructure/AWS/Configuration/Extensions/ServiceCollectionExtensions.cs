using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Organetto.Core.Shared.FileStorage.Services;
using Organetto.Infrastructure.Infrastructure.AWS.Configuration.Options;
using Organetto.Infrastructure.Infrastructure.AWS.Services;

namespace Organetto.Infrastructure.Infrastructure.AWS.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAWS(this IServiceCollection services, IConfiguration configuration)
    {
        // Регистрация конфигурации AWS Credentials
        services.Configure<Options.AWSCredentials>(configuration.GetSection("AWSCredentials"));

        // Регистрация конфигурации S3
        services.Configure<S3Config>(configuration.GetSection("S3Config"));

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var credOptions = sp.GetRequiredService<IOptions<Options.AWSCredentials>>();
            var configOptions = sp.GetRequiredService<IOptions<S3Config>>();

            var cred = credOptions.Value;
            var config = configOptions.Value;

            return new AmazonS3Client(
                new BasicAWSCredentials(cred.AccessKeyId, cred.SecretAccessKey),
                new AmazonS3Config
                {
                    ServiceURL = config.ServiceURL,
                    ForcePathStyle = bool.TryParse(config.ForcePathStyle, out var r) && r,
                    RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
                    ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED,
                    RetryMode = RequestRetryMode.Standard,
                    MaxErrorRetry = 5,
                }
            );
        });

        services.AddSingleton<IObjectStoragePort>(sp =>
        {
            return new S3ObjectStorage(
                sp.GetRequiredService<IAmazonS3>(),
                sp.GetRequiredService<IOptions<S3Config>>().Value.BucketName
            );
        });

        return services;
    }
}