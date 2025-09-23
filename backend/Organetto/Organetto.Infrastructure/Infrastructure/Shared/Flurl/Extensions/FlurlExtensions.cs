using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Http.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Organetto.Infrastructure.Infrastructure.Shared.Flurl.Extensions;
using Organetto.Infrastructure.Infrastructure.Shared.Http.Exceptions.Models;
using Organetto.Infrastructure.Infrastructure.Shared.Json.Services;
using Organetto.UseCases.Shared.Exceptions.Extensions;
using System.Collections;
using System.Net;
using System.Reflection;

namespace Organetto.Infrastructure.Infrastructure.Shared.Flurl.Extensions;

public static class FlurlExtensions
{
    public static Uri ToUriWithSlashDoubleEncode(this Url url)
    {
        var uri = url.ToUri();
        return new Uri(uri.AbsoluteUri.Replace("%2F", "%252F"));
    }

    /// <summary>
    /// Like SetQueryParams, but renders arrays as indexed brackets (starting at 1)
    /// and nests object properties in bracket notation.
    /// </summary>
    public static Url SetIndexedQueryParams(this Url url, object? queryParams)
    {
        if (queryParams == null)
            return url;
        var dict = new Dictionary<string, object>();

        void Flatten(string prefix, object? value)
        {
            if (value == null)
                return;

            var type = value.GetType();

            // simple value
            if (type.IsValueType || value is string)
            {
                dict[prefix] = value;
            }
            // IDictionary<string, object>
            else if (value is IDictionary<string, object> kvs)
            {
                foreach (var kv in kvs)
                    Flatten($"{prefix}[{kv.Key}]", kv.Value);
            }
            // any other IEnumerable (arrays, lists, etc.)
            else if (value is IEnumerable enumerable)
            {
                int index = 1; // start at 1
                foreach (var item in enumerable)
                {
                    Flatten($"{prefix}[{index}]", item);
                    index++;
                }
            }
            // any other POCO -> reflect its public properties
            else
            {
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var val = prop.GetValue(value);
                    Flatten($"{prefix}[{prop.Name}]", val);
                }
            }
        }

        if (queryParams != null)
        {
            foreach (var prop in queryParams.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Flatten(prop.Name, prop.GetValue(queryParams));
            }
        }

        return url.SetQueryParams(dict);
    }

    public static FlurlClient CreateFlurlClient(this System.Net.Http.IHttpClientFactory httpClientFactory)
    {
        return new FlurlClient(httpClientFactory.CreateClient());
    }

    public static async Task<T> ThrowIfError<T>(this Task<T> responseTask, CancellationToken cancellationToken)
        where T : IFlurlResponse
    {
        try
        {
            return await responseTask;
        }
        catch (FlurlHttpException ex)
        {
            var responseContent = await ex.ResponseContentAsStringAsync(cancellationToken);
            var errorMessage = $"Exception: {ex.FullMessage()}.\nResponse: {responseContent}.";
            throw new HttpException(errorMessage, ex, (HttpStatusCode)ex.Call.Response.StatusCode);
        }
    }

    public static IFlurlRequest WithSerializerSettings(this IFlurlRequest request, JsonSerializerSettings jsonSerializerSettings)
    {
        request.Settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSerializerSettings);
        return request;
    }

    public static IFlurlRequest WithShakeCase(this IFlurlRequest request)
    {
        return WithSerializerSettings(request, JsonUtils.JsonSerializerSettings(JsonUtils.SnakeCaseNamingStrategy));
    }

    public static IFlurlRequest WithCamelCase(this IFlurlRequest request)
    {
        return WithSerializerSettings(request, JsonUtils.JsonSerializerSettings(JsonUtils.CamelCaseNamingStrategy));
    }

    public static Task<T?> GetJsonAsync<T>(this IFlurlRequest request, JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken = default,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
    {
        request.Settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSerializerSettings);
        return request.GetJsonAsync<T?>(cancellationToken: cancellationToken, completionOption: completionOption);
    }

    public static Task<T?> GetJsonAsync<T>(this IFlurlRequest request, NamingStrategy namingStrategy, CancellationToken cancellationToken = default,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
    {
        return request.GetJsonAsync<T?>(JsonUtils.JsonSerializerSettings(namingStrategy), cancellationToken, completionOption);
    }


    public static async Task<IFlurlResponse> PostJsonAsync<T>(this IFlurlRequest request, T data, JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
    {
        request.Settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSerializerSettings);
        return await request.PostJsonAsync(data, cancellationToken);
    }

    public static Task<IFlurlResponse> PostCamelCaseJsonAsync<T>(this IFlurlRequest request, T data, CancellationToken cancellationToken)
    {
        return PostJsonAsync(request, data, JsonUtils.JsonSerializerSettings(JsonUtils.CamelCaseNamingStrategy), cancellationToken);
    }

    public static CapturedMultipartContent AddMvcForm<T>(this CapturedMultipartContent content, T formData)
        where T : class
    {
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var propertyAttributes = property.GetCustomAttributes(true);

            var bindPropertyAttr = propertyAttributes.OfType<BindPropertyAttribute>().FirstOrDefault();
            var bindRequiredAttr = propertyAttributes.OfType<BindRequiredAttribute>().FirstOrDefault();

            var propertyValue = property.GetValue(formData);

            if (bindRequiredAttr != null && (propertyValue == null || string.IsNullOrEmpty(propertyValue.ToString())))
                throw new InvalidDataException($"The {property.Name} field is required");
            if (propertyValue == null) continue;

            content.AddString(bindPropertyAttr?.Name ?? property.Name, propertyValue.ToString());
        }
        return content;
    }

    public static Task<string> ResponseContentAsStringAsync(this FlurlHttpException ex, CancellationToken cancellationToken)
    {
        return ex.Call.HttpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
    }
}
