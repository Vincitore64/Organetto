using Microsoft.Extensions.Options;
using Organetto.Web.Configuration.Logging.Middleware.Options;

namespace Organetto.Web.Configuration.Logging.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private static readonly string gRPCContentTypeName = "application/grpc";
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> logger;
        private readonly IOptions<RequestResponseLoggingOptions> options;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger,
            IOptions<RequestResponseLoggingOptions> options)
        {
            _next = next;
            this.logger = logger;
            this.options = options;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (options.Value.ExcludedPaths.Any(p => httpContext.Request.Path.StartsWithSegments(p)) ||
                httpContext.Request.ContentType == gRPCContentTypeName)
            {
                await _next(httpContext);
                return;
            }

            if (options.Value.LogRequestBody)
            {
                try
                {
                    logger.LogInformation($"HTTP request information:\n" +
                        $"\tMethod: {httpContext.Request.Method}\n" +
                        $"\tPath: {httpContext.Request.Path}\n" +
                        $"\tQueryString: {httpContext.Request.QueryString}\n" +
                        $"\tHeaders: {FormatHeaders(httpContext.Request.Headers)}\n" +
                        $"\tSchema: {httpContext.Request.Scheme}\n" +
                        $"\tHost: {httpContext.Request.Host}\n" +
                        $"\tBody: {await ReadBodyFromRequest(httpContext.Request, options.Value.MaxBodyBytes)}");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to log HTTP request information.");
                }
            }

            // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.
            var originalResponseBody = httpContext.Response.Body;
            using var newResponseBody = new MemoryStream();
            httpContext.Response.Body = newResponseBody;

            // Call the next middleware in the pipeline
            await _next(httpContext);

            if (options.Value.LogResponseBody && IsContentTypeAllowed(httpContext.Response.ContentType))
            {
                var responseBodyText = await ReadBodyAsync(newResponseBody, options.Value.MaxBodyBytes);

                logger.LogInformation($"HTTP response information:\n" +
                    $"\tStatusCode: {httpContext.Response.StatusCode}\n" +
                    $"\tContentType: {httpContext.Response.ContentType}\n" +
                    $"\tHeaders: {FormatHeaders(httpContext.Response.Headers)}\n" +
                    $"\tBody: {responseBodyText}");
            }

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
        }

        private bool IsContentTypeAllowed(string? contentType) =>
            options.Value.AllowedContentTypes.Count == 0 ||
            options.Value.AllowedContentTypes.Any(t =>
                contentType?.StartsWith(t, StringComparison.OrdinalIgnoreCase) == true);

        private static string FormatHeaders(IHeaderDictionary headers) => string
            .Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value.ToString())}}}"));

        private static async Task<string> ReadBodyAsync(Stream stream, int maxBytes)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, leaveOpen: true);
            var text = await reader.ReadToEndAsync();
            //stream.Seek(0, SeekOrigin.Begin);
            return maxBytes > 0 && text.Length > maxBytes
                   ? text.Substring(0, maxBytes) + "…(trimmed)"
                   : text;
        }

        private static async Task<string> ReadBodyFromRequest(HttpRequest request, int maxBytes)
        {
            // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.
            request.Body.Position = 0;
            return maxBytes > 0 && requestBody.Length > maxBytes
               ? requestBody.Substring(0, maxBytes) + "…(trimmed)"
               : requestBody;
        }
    }


    //public class RequestResponseLoggerMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly ILogger<RequestResponseLoggerMiddleware> logger;

    //    public RequestResponseLoggerMiddleware(RequestDelegate next, ILogger<RequestResponseLoggerMiddleware> logger)
    //    {
    //        _next = next;
    //        this.logger = logger;
    //    }

    //    public async Task InvokeAsync(HttpContext httpContext)
    //    {
    //        try
    //        {
    //            logger.LogInformation($"HTTP request information:\n" +
    //                $"\tMethod: {httpContext.Request.Method}\n" +
    //                $"\tPath: {httpContext.Request.Path}\n" +
    //                $"\tQueryString: {httpContext.Request.QueryString}\n" +
    //                $"\tHeaders: {FormatHeaders(httpContext.Request.Headers)}\n" +
    //                $"\tSchema: {httpContext.Request.Scheme}\n" +
    //                $"\tHost: {httpContext.Request.Host}\n" +
    //                $"\tBody: {await ReadBodyFromRequest(httpContext.Request)}");
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.LogWarning(ex, "Failed to log HTTP request information.");
    //        }

    //        // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.
    //        //var originalResponseBody = httpContext.Response.Body;
    //        //using var newResponseBody = new MemoryStream();
    //        //httpContext.Response.Body = newResponseBody;

    //        // Call the next middleware in the pipeline
    //        await _next(httpContext);

    //        //newResponseBody.Seek(0, SeekOrigin.Begin);
    //        //var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

    //        //Console.WriteLine($"HTTP request information:\n" +
    //        //    $"\tStatusCode: {httpContext.Response.StatusCode}\n" +
    //        //    $"\tContentType: {httpContext.Response.ContentType}\n" +
    //        //    $"\tHeaders: {FormatHeaders(httpContext.Response.Headers)}\n" +
    //        //    $"\tBody: {responseBodyText}");

    //        //newResponseBody.Seek(0, SeekOrigin.Begin);
    //        //await newResponseBody.CopyToAsync(originalResponseBody);
    //    }

    //    private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));

    //    private static async Task<string> ReadBodyFromRequest(HttpRequest request)
    //    {
    //        // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
    //        request.EnableBuffering();

    //        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
    //        var requestBody = await streamReader.ReadToEndAsync();

    //        // Reset the request's body stream position for next middleware in the pipeline.
    //        request.Body.Position = 0;
    //        return requestBody;
    //    }
    //}

}
