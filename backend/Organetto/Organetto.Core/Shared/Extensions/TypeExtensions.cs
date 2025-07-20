namespace Organetto.Core.Shared.Extensions
{
    public static class TypeExtensions
    {

        public static T ThrowIfNull<T>(this T? v, Exception exception)
        {
            return v ?? throw exception;
        }

        public static T ThrowIfNull<T>(this T? v)
        {
            return v ?? throw new ArgumentNullException(nameof(v));
        }
    }
}
