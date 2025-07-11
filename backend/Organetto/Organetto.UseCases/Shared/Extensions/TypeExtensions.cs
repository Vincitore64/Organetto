namespace Organetto.UseCases.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static T ThrowIfNull<T>(this T? v)
        {
            return v ?? throw new ArgumentNullException(nameof(v));
        }
    }
}
