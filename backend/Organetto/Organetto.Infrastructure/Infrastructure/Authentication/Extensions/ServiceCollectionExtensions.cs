using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Organetto.Core.Authentication.Ports.Services;
using Organetto.Infrastructure.Infrastructure.Authentication.Services;
using Organetto.Infrastructure.Infrastructure.Firebase.Data;

namespace Organetto.Infrastructure.Infrastructure.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="AuthenticationService"/> and bootstraps <see cref="FirebaseApp"/>.
        /// </summary>
        public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FirebaseOptions>(configuration.GetSection("Firebase"));

            // FirebaseApp as singleton
            services.AddSingleton(provider =>
            {
                var opt = provider.GetRequiredService<IOptions<FirebaseOptions>>().Value;
                var cred = GoogleCredential.FromFile(opt.CredentialsPath);

                return FirebaseApp.DefaultInstance ?? FirebaseApp.Create(new AppOptions
                {
                    Credential = cred,
                    ProjectId = opt.ProjectId
                });
            });

            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
