import type { AttachmentDto } from '@/dataAccess/cards/models'

interface AttachmentVm extends Omit<AttachmentDto, 'uploadedAt'> {
  uploadedAt: Date
}

export type { AttachmentVm }
