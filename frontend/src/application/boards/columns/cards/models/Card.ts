import type { CardDto } from '@/dataAccess/cards/models'
import type { DueDateVm } from './DueDate'
import type { AttachmentVm } from './Attachment'

/** View‑model for a single card inside a column. */
interface CardVm extends Omit<CardDto, 'dueDate'> {
  /** Native JS date instead of ISO string (null if missing). */
  dueDate: Date | null
  /** Computed: task is overdue (dueDate < now & not completed). */
  isOverdue: boolean
}

interface CardDetailVm {
  id: number // from CardDetailDto.Id
  title: string // from CardDetailDto.Title
  description: string // from CardDetailDto.Description
  position: number // from CardDetailDto.Position
  dueDates: DueDateVm[] // from CardDetailDto.DueDates
  attachments: AttachmentVm[] // from CardDetailDto.Attachments

  // Optional convenience fields for UI:
  dueDate: Date | null;
  isOverdue: boolean;
}

export type { CardVm, CardDetailVm }
