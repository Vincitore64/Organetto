namespace Organetto.UseCases.Shared.Exceptions.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<Exception> InnerExceptions(this Exception ex)
        {

            var inner = ex.InnerException;
            while (inner != null)
            {
                yield return inner;
                inner = inner.InnerException;
            }
        }

        public static string InnerMessage(this Exception ex)
        {
            var messages = ex.InnerExceptions().Select(ex => ex.Message).ToArray();
            return string.Join('\n', messages);
        }

        public static string FullMessage(this Exception ex)
        {
            var innerMessage = ex.InnerMessage();
            return string.Join('\n', ex.Message, innerMessage);
        }
    }
}
