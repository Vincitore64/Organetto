using Newtonsoft.Json;

namespace Organetto.Infrastructure.Infrastructure.Authentication.Data
{
    public record ErrorResponse(
        [property: JsonProperty("error")] ErrorDetail Error
    )
    {
        public static ErrorResponse Unknown => new(new ErrorDetail(500, "UNKNOWN"));
    }

    public record ErrorDetail(
        [property: JsonProperty("code")] int Code,

        [property: JsonProperty("message")] string Message
    );

    public record ErrorItem(
        [property: JsonProperty("message")] string Message,

        [property: JsonProperty("domain")] string Domain,

        [property: JsonProperty("reason")] string Reason
    );

}
