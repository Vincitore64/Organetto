namespace Organetto.Core.Shared.FileStorage.Models
{
    public readonly record struct ObjectKey(string Value) : IStorageKey
    {
        public static ObjectKey For(Guid? tenantId, long attachmentId, string fileName, DateTimeOffset now)
        {
            var yyyy = now.UtcDateTime.Year;
            var mm = now.UtcDateTime.Month.ToString("D2");
            var dd = now.UtcDateTime.Day.ToString("D2");
            var slug = Slug(fileName);
            var tenant = tenantId?.ToString() ?? "public";
            return new ObjectKey($"tenant/{tenant}/attachments/{yyyy}/{mm}/{dd}/{attachmentId}-{slug}");
        }

        private static string Slug(string fileName)
        {
            var name = Path.GetFileName(fileName);
            var cleaned = new string(name.Select(ch => char.IsLetterOrDigit(ch) || ch is '.' || ch is '-' || ch is '_' ? ch : '-').ToArray());
            return cleaned.Trim('-');
        }
    }
}
