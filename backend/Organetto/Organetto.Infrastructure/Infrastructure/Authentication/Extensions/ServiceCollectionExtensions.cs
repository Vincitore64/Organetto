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
            services.Configure<FirebaseSettings>(configuration.GetSection(FirebaseSettings.SectionName));

            // FirebaseApp as singleton
            services.AddSingleton(provider =>
            {
                var opt = provider.GetRequiredService<IOptions<FirebaseSettings>>().Value;
                var cred = GoogleCredential.FromFile(opt.CredentialsPath);

                //if (FirebaseApp.DefaultInstance == null)
                //{
                //    var credential = GoogleCredential.FromFile(_settings.ServiceAccountPath);
                //    FirebaseApp.Create(new AppOptions { Credential = credential, ProjectId = _settings.ProjectId });
                //}

                return FirebaseApp.DefaultInstance ?? FirebaseApp.Create(new AppOptions
                {
                    Credential = cred,
                    ProjectId = opt.ProjectId
                });
            });
            services.AddHttpClient<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
