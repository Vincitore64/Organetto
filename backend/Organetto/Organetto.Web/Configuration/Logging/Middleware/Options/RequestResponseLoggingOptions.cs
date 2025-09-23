namespace Organetto.Web.Configuration.Logging.Middleware.Options
{
    public sealed class RequestResponseLoggingOptions
    {
        /// <summary>Log the request body?</summary>
        public bool LogRequestBody { get; set; } = true;

        /// <summary>Log the response body?</summary>
        public bool LogResponseBody { get; set; } = true;

        /// <summary>Log only these content types (empty = any).</summary>
        public List<string> AllowedContentTypes { get; set; } = new List<string>();

        /// <summary>Skip logging for these path prefixes.</summary>
        public List<string> ExcludedPaths { get; set; } = new List<string>();

        /// <summary>Trim body to this many bytes (0 = no trim).</summary>
        public int MaxBodyBytes { get; set; } = 0;
    }
}
