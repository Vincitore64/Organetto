using Newtonsoft.Json;
using Organetto.Infrastructure.Infrastructure.Authentication.Data;
using Organetto.Infrastructure.Infrastructure.Authentication.Exceptions;

namespace Organetto.Infrastructure.Infrastructure.Authentication.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task EnsureAuthenticationSuccessStatusCodeAsync(this HttpResponseMessage response, string errorTitle, string errorMessage)
        {
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(json) ?? ErrorResponse.Unknown;
                throw new AuthenticationException(errorResponse.Error.Code, errorTitle, errorResponse.Error.Message, errorMessage);
            }
        }
    }
}
