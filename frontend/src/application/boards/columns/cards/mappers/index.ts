import { createArrayMapper, createMapper } from '@/application/shared/mappers'
import type { AttachmentDto, CardDetailDto, CardDto, DueDateDto } from '@/dataAccess/cards/models'
import type { AttachmentVm, CardVm } from '../models'
import type { DueDateVm } from '../models/DueDate'
import type { CardDetailVm } from '../models/Card'

const mapCard = createMapper<CardDto, CardVm>({
  id: 'id',
  title: 'title',
  description: 'description',
  position: 'position',
  /** ISO8601 → Date | null */
  dueDate: c => (c.dueDate ? new Date(c.dueDate) : null),
  /** Helper flags resolved on the fly – not stored in the DTO */
  isOverdue: c => !!c.dueDate && new Date(c.dueDate).getTime() < Date.now(),
})

const mapAttachment = createMapper<AttachmentDto, AttachmentVm>({
  id: 'id',
  filename: 'filename',
  fileUrl: 'fileUrl',
  uploadedAt: c => new Date(c.uploadedAt),
  uploaderId: 'uploaderId',
})

const mapDueDate = createMapper<DueDateDto, DueDateVm>({
  dueAt: c => new Date(c.dueAt),
  isComplete: 'isComplete',
})

const nextDueAt = (src: CardDetailDto) => {
  const dueDates = createArrayMapper(mapDueDate)(src.dueDates)
  const incompleteDueDates = dueDates.filter(d => !d.isComplete)
  return incompleteDueDates.length > 0 ? incompleteDueDates[0].dueAt : null
}

const mapCardDetail = createMapper<CardDetailDto, CardDetailVm>({
  id: 'id',
  title: 'title',
  description: 'description',
  position: 'position',
  dueDates: (src) => createArrayMapper(mapDueDate)(src.dueDates),
  attachments: (src) => createArrayMapper(mapAttachment)(src.attachments),
  dueDate: nextDueAt,
  isOverdue: (src) => {
    const nextDueAtValue = nextDueAt(src)
    return nextDueAtValue !== null && nextDueAtValue.getTime() < Date.now()
  },
})

const mapCards = createArrayMapper(mapCard)

const mapAttachments = createArrayMapper(mapAttachment)

export { mapCard, mapCards, mapAttachments, mapCardDetail }
