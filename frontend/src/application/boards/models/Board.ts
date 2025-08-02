import type { BoardDetailedDto, BoardDto } from '@/dataAccess/boards/models'
import type { ColumnVm } from '../columns'
import type { BoardMemberVm } from '../members'

/** Minimal board item for lists & sidebar nav. */
export interface BoardListItemVm extends Pick<BoardDto, 'id' | 'title' | 'description' | 'isArchived'> {
  /** Display‑ready created date. */
  createdAt: Date
}

/** Full board view‑model shown on the Kanban page. */
export interface BoardVm extends Omit<BoardDetailedDto, 'columns' | 'members'> {
  columns: ColumnVm[]
  members: BoardMemberVm[]
  /** True while an optimistic UI mutation is pending server confirm. */
  isUpdating: boolean
}
