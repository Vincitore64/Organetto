using Organetto.Core.Boards.Cards.Models;
using Organetto.Core.Shared.Models;
using Organetto.Core.Users.Data;

namespace Organetto.Core.Boards.Cards.Data
{
    /// <summary>
    /// Represents an uploaded attachment for a card.
    /// </summary>
    public class Attachment : BaseEntity
    {
        public Attachment()
        {
            FileKey = string.Empty;
            FileName = string.Empty;
            ContentType = string.Empty;
            ChecksumSha256 = string.Empty;
            Links = new HashSet<AttachmentLink>();
        }

        //public long CardId { get; set; }                                // FK to Card (внешний ключ к Card)
        public long OwnerUserId { get; set; }                             // FK to User (внешний ключ к User)
        public string FileKey { get; set; }                              // URL or path to file (ссылка на файл)
        public string FileName { get; set; }                             // Original filename (оригинальное имя файла)
        public long SizeBytes { get; set; }
        public string ContentType { get; set; }
        public string ChecksumSha256 { get; set; } // base64 or hex

        // State
        public AttachmentStatus Status { get; set; }
        public int Version { get; set; }

        // Audit
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        // Arbitrary metadata
        public string MetadataJson { get; set; } = "{}";

        // Navigation properties
        //public Card? Card { get; set; }                                   // Card navigation (связь с карточкой)
        public User? OwnerUser { get; set; }                               // Uploader navigation (связь с загрузившим)
        public ICollection<AttachmentLink> Links { get; set; }

        public static Attachment Create(long id,  long uploaderId, string fileKey,
            string fileName, string contentType, long sizeBytes, string checksumSha256, DateTimeOffset now)
        {
            if (sizeBytes < 0) throw new ArgumentException("Size must be >= 0", nameof(sizeBytes));
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("Filename required", nameof(fileName));

            return new Attachment
            {
                Id = id,
                OwnerUserId = uploaderId,
                FileKey = fileKey,
                FileName = fileName,
                SizeBytes = sizeBytes,
                ContentType = contentType,
                ChecksumSha256 = checksumSha256,
                Status = AttachmentStatus.PendingUpload,
                Version = 1,
                CreatedAt = now
            };
        }

        public void MarkUploaded(DateTimeOffset now)
        {
            if (Status != AttachmentStatus.PendingUpload)
                throw new InvalidOperationException("Attachment not in PendingUpload state.");
            Status = AttachmentStatus.PendingScan;
            UpdatedAt = now;
        }

        public void Activate(DateTimeOffset now)
        {
            if (Status != AttachmentStatus.PendingScan && Status != AttachmentStatus.PendingUpload)
                throw new InvalidOperationException("Attachment not in a state that can be activated.");
            Status = AttachmentStatus.Active;
            UpdatedAt = now;
        }

        public void SoftDelete(DateTimeOffset now)
        {
            if (Status == AttachmentStatus.Deleted) return;
            Status = AttachmentStatus.Deleted;
            DeletedAt = now;
            UpdatedAt = now;
        }

        public void Restore(DateTimeOffset now)
        {
            if (Status != AttachmentStatus.Deleted)
                throw new InvalidOperationException("Attachment not deleted.");
            Status = AttachmentStatus.Active;
            DeletedAt = null;
            UpdatedAt = now;
        }

        public void UpdateMetadata(string metadataJson, DateTimeOffset now)
        {
            MetadataJson = metadataJson;
            UpdatedAt = now;
        }
    }
}
