import { createArrayMapper, createMapper } from '@/application/shared/mappers'
import type { Attachment, CardDto } from '@/dataAccess/cards/models'
import type { AttachmentVm, CardVm } from '../models'

const mapCard = createMapper<CardDto, CardVm>({
  id: 'id',
  title: 'title',
  description: 'description',
  position: 'position',
  /** ISO8601 → Date | null */
  dueDate: c => (c.dueDate ? new Date(c.dueDate) : null),
  /** Helper flags resolved on the fly – not stored in the DTO */
  isOverdue: c => !!c.dueDate && new Date(c.dueDate).getTime() < Date.now(),
  // DnD local‑state – set at runtime by UI (always false on mapping)
  isDragging: () => false,
})

const mapAttachment = createMapper<Attachment, AttachmentVm>({
  id: 'id',
  filename: 'filename',
  fileUrl: 'fileUrl',
  uploadedAt: c => new Date(c.uploadedAt),
  uploaderId: 'uploaderId',
})

const mapCards = createArrayMapper(mapCard)

const mapAttachments = createArrayMapper(mapAttachment)

export { mapCard, mapCards, mapAttachments }
