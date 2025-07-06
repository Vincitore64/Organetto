namespace Organetto.Web.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IConfigurationBuilder AddConfigurationFile(this IConfigurationBuilder builder, string fileName, string? environmentName)
        {
            //var filePath = Path.Combine(AppContext.BaseDirectory, hostEnvironment.IsDevelopment()
            //    ? $"{fileName}.Development.json"
            //    : hostEnvironment.IsStaging()
            //        ? $"{fileName}.Staging.json"
            //        : $"{fileName}.json");
            var filePath = Path.Combine(AppContext.BaseDirectory, environmentName == null ? $"{fileName}.json" : $"{fileName}.{environmentName}.json");
            return builder.AddJsonFile(filePath);
        }

        public static IConfigurationBuilder AddAppSettingsServerConfigurationFile(this IConfigurationBuilder builder)
        {
            return AddConfigurationFile(builder, "appsettings-server", null);
        }

        public static IConfigurationBuilder AddAppSettingsServerConfigurationFile(this IConfigurationBuilder builder, string? environmentName)
        {
            return AddConfigurationFile(builder, "appsettings-server", environmentName);
        }

        public static void UseAppCors(this WebApplication app)
        {
            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        }
    }
}
