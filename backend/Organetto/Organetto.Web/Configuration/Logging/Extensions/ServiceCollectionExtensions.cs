using Newtonsoft.Json;
using Organetto.Web.Configuration.Logging.Middleware;
using Organetto.Web.Configuration.Logging.Middleware.Options;
using Serilog;
using Serilog.Events;
using System.Net.Mime;

namespace Organetto.Web.Configuration.Logging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
            //builder.Services.AddSerilog();
        }

        public static void DefaultRequestResponseLoggingOptionsConfigure(RequestResponseLoggingOptions options)
        {
            options.ExcludedPaths.Add("/swagger/");
            options.AllowedContentTypes.Add(MediaTypeNames.Application.Json);
        }

        public static IServiceCollection AddRequestResponseLogging(
            this IServiceCollection services,
            Action<RequestResponseLoggingOptions>? configure = null)
        {
            if (configure is not null)
                services.Configure(configure);
            else
            {
                services.Configure<RequestResponseLoggingOptions>((opts) =>
                {
                });
            }
            return services;
        }

        public static IServiceCollection AddPreConfiguredRequestResponseLogging(
            this IServiceCollection services,
            Action<RequestResponseLoggingOptions>? configure = null)
        {
            if (configure is not null)
                services.Configure<RequestResponseLoggingOptions>((opts) =>
                {
                    DefaultRequestResponseLoggingOptionsConfigure(opts);
                    configure(opts);
                });
            else
            {
                services.Configure<RequestResponseLoggingOptions>(DefaultRequestResponseLoggingOptionsConfigure);
            }
            return services;
        }

        public static IServiceCollection AddRequestResponseLogging(
            this IServiceCollection services, string apiPathPrefix)
        {
            return services.AddRequestResponseLogging(options =>
            {
                DefaultRequestResponseLoggingOptionsConfigure(options);
                options.ExcludedPaths.Add($"/{apiPathPrefix}/swagger/");
            });
        }

        public static IServiceCollection AddRequestResponseLogging(
            this IServiceCollection services)
        {
            return services.AddRequestResponseLogging(DefaultRequestResponseLoggingOptionsConfigure);
        }

        public static void UseRequestResponseLogging(this WebApplication app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.IncludeQueryInRequestPath = true;
                options.GetMessageTemplateProperties = GetMessageTemplateProperties;
            });
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }

        private static IEnumerable<LogEventProperty> GetMessageTemplateProperties(HttpContext httpContext, string requestPath, double elapsedMs, int statusCode)
        {
            var headers = httpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            var rawHeaders = JsonConvert.SerializeObject(headers);

            return new LogEventProperty[]
            {
                new LogEventProperty("RequestMethod", new ScalarValue(httpContext.Request.Method)),
                new LogEventProperty("RequestPath", new ScalarValue(requestPath)),
                new LogEventProperty("StatusCode", new ScalarValue(statusCode)),
                new LogEventProperty("ElapsedInMs", new ScalarValue(elapsedMs)),
                new LogEventProperty("RawHeaders", new ScalarValue(rawHeaders)),
            };
        }
    }
}
