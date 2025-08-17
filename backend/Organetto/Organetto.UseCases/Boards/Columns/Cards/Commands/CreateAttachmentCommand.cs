using AutoMapper;
using MediatR;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Cards.Models;
using Organetto.Core.Boards.Cards.Services;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Shared.FileStorage.Models;
using Organetto.Core.Shared.FileStorage.Services;
using Organetto.Core.Shared.IO.Services;
using Organetto.UseCases.Boards.Columns.Cards.Data;

namespace Organetto.UseCases.Boards.Columns.Cards.Commands
{
    public sealed record CreateAttachmentCommand(
        UploadFile File,
        long CurrentUserId,
        AttachmentOwnerKind? OwnerKind = null,
        long? OwnerId = null,
        string? MetadataJson = null
    ) : IRequest<AttachmentDto>;

    public sealed class CreateAttachmentHandler : IRequestHandler<CreateAttachmentCommand, AttachmentDto>
    {
        private readonly IAttachmentRepository _repo;
        private readonly IObjectStoragePort _storage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStreamHasher _streamHasher;

        public CreateAttachmentHandler(IAttachmentRepository repo, IObjectStoragePort storage, IUnitOfWork unitOfWork, IMapper mapper, IStreamHasher streamHasher)
        {
            _repo = repo;
            _storage = storage;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._streamHasher = streamHasher;
        }

        public async Task<AttachmentDto> Handle(CreateAttachmentCommand c, CancellationToken ct)
        {
            if (c.File is null || c.File.Length <= 0) throw new ArgumentException("Empty file.", nameof(c.File));

            var now = DateTimeOffset.UtcNow;
            var ctpe = ContentType.Create(c.File.FileName);

            // 1) Создадим доменную сущность
            var a = Attachment.Create(
                ownerUserId: c.CurrentUserId,
                fileName: c.File.FileName,
                contentType: ctpe,
                sizeBytes: c.File.Length,
                now: now
            );

            a = await _repo.CreateAsync(a, ct);

            var key = ObjectKey.For(null, a.Id, c.File.FileName, now);



            // 2) Посчитаем SHA-256 и зальём в хранилище (если поток не seekable — в буфер)
            string sha256;
            if (c.File.Content.CanSeek)
            {
                sha256 = await _streamHasher.ComputeSha256Base64Async(c.File.Content, ct);
                c.File.Content.Position = 0;
                await _storage.PutAsync(key, c.File.Content, ctpe, ct);
            }
            else
            {
                using var buf = new MemoryStream(capacity: (int)Math.Min(c.File.Length, 64 * 1024 * 1024)); // защитный cap
                await c.File.Content.CopyToAsync(buf, ct);
                buf.Position = 0;
                sha256 = await _streamHasher.ComputeSha256Base64Async(buf, ct);
                buf.Position = 0;
                await _storage.PutAsync(key, buf, ctpe, ct);
            }

            // 3) Привязка к владельцу (необязательно)
            var link = c.OwnerKind.HasValue && c.OwnerId.HasValue
                ? AttachmentLink.Create(a.Id, c.OwnerKind.Value, c.OwnerId.Value, c.CurrentUserId)
                : null;

            a.Activate(now, key.Value, sha256, link);

            if (!string.IsNullOrWhiteSpace(c.MetadataJson))
                a.UpdateMetadata(c.MetadataJson!, now);

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<AttachmentDto>(a);
        }
    }
}
