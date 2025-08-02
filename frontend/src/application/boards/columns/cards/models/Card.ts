import type { CardDto } from '@/dataAccess/cards/models'

/** View‑model for a single card inside a column. */
export interface CardVm extends Omit<CardDto, 'dueDate'> {
  /** Native JS date instead of ISO string (null if missing). */
  dueDate: Date | null
  /** Computed: task is overdue (dueDate < now & not completed). */
  isOverdue: boolean
  /** UI state: true while the card is being dragged. */
  isDragging: boolean
}