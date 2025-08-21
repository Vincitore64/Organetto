using Organetto.BuildingBlocks.Core.Models;

namespace Organetto.Attachments.Core.Attachments.Models
{
    public sealed class AttachmentLink : BaseEntity
    {
        // FK → Attachment
        public long AttachmentId { get; set; }
        public Attachment Attachment { get; set; } = null!;

        // Куда привязан файл
        public AttachmentOwnerKind OwnerKind { get; set; }
        public long OwnerId { get; set; }

        // Мета
        public int? SortOrder { get; set; }
        public bool IsDeleted { get; set; }    // soft-delete
        public DateTimeOffset CreatedAt { get; set; }
        public long CreatedByUserId { get; set; }

        private AttachmentLink() { } // EF

        private AttachmentLink(long attachmentId, AttachmentOwnerKind kind, long ownerId, long createdByUserId, int? sortOrder)
        {
            if (ownerId <= 0) throw new ArgumentOutOfRangeException(nameof(ownerId));
            AttachmentId = attachmentId;
            OwnerKind = kind;
            OwnerId = ownerId;
            CreatedByUserId = createdByUserId;
            SortOrder = sortOrder;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static AttachmentLink Create(long attachmentId, AttachmentOwnerKind kind, long ownerId, long createdByUserId, int? sortOrder = null)
            => new(attachmentId, kind, ownerId, createdByUserId, sortOrder);

        public void MarkDeleted() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
        public void Reorder(int? sortOrder) => SortOrder = sortOrder;
    }
}
