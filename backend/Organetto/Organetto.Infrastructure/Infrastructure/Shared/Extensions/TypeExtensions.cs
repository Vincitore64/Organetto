namespace Organetto.Infrastructure.Infrastructure.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static T ThrowIfNull<T>(this T? v)
        {
            return v ?? throw new ArgumentNullException(nameof(v));
        }
    }
}
