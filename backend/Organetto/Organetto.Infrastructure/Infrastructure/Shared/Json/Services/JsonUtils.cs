using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Organetto.Infrastructure.Infrastructure.Shared.Json.Services
{
    public static class JsonUtils
    {
        public static readonly SnakeCaseNamingStrategy SnakeCaseNamingStrategy = new SnakeCaseNamingStrategy();
        public static readonly CamelCaseNamingStrategy CamelCaseNamingStrategy = new CamelCaseNamingStrategy();

        public static JsonSerializerSettings JsonSerializerSettings(NamingStrategy namingStrategy)
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = namingStrategy,
                }
            };
        }

        public static string ToSnakeCase<T>(this T instance)
        {
            return JsonConvert.SerializeObject(instance, JsonSerializerSettings(SnakeCaseNamingStrategy));
        }

        //public static string ToSnakeCase(this string @string)
        //{
        //    return SnakeCaseNamingStrategy.GetPropertyName(@string, false);
        //}

        public static StringContent JsonContent<T>(T value)
        {
            return JsonContent(value, (JsonSerializerSettings?)null);
        }

        public static StringContent JsonContent<T>(T value, NamingStrategy namingStrategy)
        {
            return JsonContent(value, JsonSerializerSettings(namingStrategy));
        }

        public static StringContent SnakeJsonContent<T>(T value)
        {
            return JsonContent(value, JsonSerializerSettings(SnakeCaseNamingStrategy));
        }

        public static StringContent CamelJsonContent<T>(T value)
        {
            return JsonContent(value, JsonSerializerSettings(CamelCaseNamingStrategy));
        }

        public static StringContent JsonContent<T>(T value, JsonSerializerSettings? jsonSerializerSettings)
        {
            var jsonContent = JsonConvert.SerializeObject(value, jsonSerializerSettings);
            var requestContent = new StringContent(jsonContent, null, "application/json");
            return requestContent;
        }

        public static T DeserializeObjectFromSnake<T>(this string json)
        {
            return json.DeserializeObject<T>(SnakeCaseNamingStrategy);
        }

        public static T DeserializeObjectFromCamel<T>(this string json)
        {
            return json.DeserializeObject<T>(CamelCaseNamingStrategy);
        }

        public static T DeserializeObject<T>(this string json, NamingStrategy namingStrategy)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings(namingStrategy)) ??
                throw new InvalidOperationException($"Не удалось десериализовать объект - {json}");
        }

        public static T DeserializeObject<T>(string json, string objectName)
        {
            return JsonConvert.DeserializeObject<T>(json)
                ?? throw new InvalidOperationException($"Не удалось десериализовать объект {objectName}");
        }
    }
}
