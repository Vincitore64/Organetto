namespace Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Models
{
    // abstract base exception carrying all ProblemDetails properties
    public abstract class ApiException : Exception, IHasErrorCode
    {
        public int Status { get; }
        public string Title { get; }
        public string Type => GetType().Name;
        public string Code { get; }
        public string? Instance { get; }
        public IDictionary<string, string[]>? Errors { get; }

        protected ApiException(
            int status,
            string title,
            string code,
            string message,
            IDictionary<string, string[]>? errors = null,
            string? instance = null
        ) : base(message)
        {
            Status = status;
            Title = title;
            Code = code;
            Errors = errors ?? new Dictionary<string, string[]>();
            Instance = instance;
        }
    }

}
