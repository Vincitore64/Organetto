using Microsoft.AspNetCore.StaticFiles;

namespace Organetto.Core.Shared.FileStorage.Models
{
    /// <summary>
    /// Value object для MIME-типа контента.
    /// Гарантирует валидность формата "type/subtype".
    /// </summary>
    public sealed class ContentType : IContentType, IEquatable<ContentType>
    {
        // Простая проверка: "type/subtype", где type и subtype состоят из букв, цифр, -, + или .
        //private static readonly Regex ValidPattern = new(
        //    @"^[a-zA-Z0-9]+\/[a-zA-Z0-9\-\.\+]+$",
        //    RegexOptions.Compiled
        //);

        /// <summary>
        /// Строковое представление контент-типа.
        /// </summary>
        public string Value { get; }

        private ContentType(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Фабрика с валидацией:
        /// - не null/пустой
        /// - соответствует простому MIME-паттерну "type/subtype"
        /// </summary>
        public static ContentType Create(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath не может быть пустым или содержать только пробелы.", nameof(filePath));

            string defaultContentType = "application/octet-stream";
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string? contentType))
            {
                contentType = defaultContentType;
            }

            //if (!ValidPattern.IsMatch(filePath))
            //    throw new ArgumentException($"\"{filePath}\" не соответствует формату MIME-типа (type/subtype).", nameof(filePath));

            return new ContentType(contentType);
        }

        public override string ToString() => Value;

        #region Equality members

        public bool Equals(ContentType? other) =>
            other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) =>
            obj is ContentType other && Equals(other);

        public override int GetHashCode() =>
            StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

        public static bool operator ==(ContentType? left, ContentType? right) =>
            Equals(left, right);

        public static bool operator !=(ContentType? left, ContentType? right) =>
            !Equals(left, right);

        #endregion

        #region Conversions

        /// <summary>
        /// Неявное приведение к string.
        /// </summary>
        public static implicit operator string(ContentType ct) =>
            ct?.Value ?? throw new ArgumentNullException(nameof(ct));

        /// <summary>
        /// Явное создание из string: (ContentType)"asdf.pdf"
        /// </summary>
        public static explicit operator ContentType(string value) =>
            Create(value);

        #endregion
    }

}
