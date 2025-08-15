import type { Attachment } from '@/dataAccess/cards/models'

interface AttachmentVm extends Omit<Attachment, 'uploadedAt'> {

  uploadedAt: Date
}

export type { AttachmentVm }
