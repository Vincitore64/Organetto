using Polly;
using Polly.Extensions.Http;

namespace Organetto.Web.Configuration.Http.Extensions
{
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Добавляет политику повторной попытки (retry) для HttpClient с параметрами из конфигурации.
        /// </summary>
        /// <param name="builder">IHttpClientBuilder, к которому добавляется политика.</param>
        /// <returns>IHttpClientBuilder с добавленной политикой retry.</returns>
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder builder)
        {
            // Читаем из конфигурации число попыток
            var retryCount = 3;
            // Читаем базовую задержку между попытками в секундах
            var baseDelaySeconds = 3;

            return builder.AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult((r) => r.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    .WaitAndRetryAsync(
                        retryCount,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(baseDelaySeconds, retryAttempt)),
                        onRetry: (outcome, timespan, retryNumber, context) =>
                        {
                            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<HttpClient>>();
                            var responseContent = outcome.Result.Content.ReadAsStringAsync();
                            Task.WaitAll(responseContent);
                            logger.LogWarning(
                                $"Polly retry {retryNumber} after {timespan.TotalSeconds}s due to {outcome.Exception?.Message ?? outcome.Result.ReasonPhrase}." +
                                $"Response content: {responseContent.Result}."
                            );
                        }
                    )
            );
        }
    }
}
