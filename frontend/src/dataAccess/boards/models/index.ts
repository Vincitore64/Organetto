import type { ColumnDto } from '@/dataAccess/columns/models'
import type { BoardMemberDto } from '../members/models'

/** Response model for a Kanban board. */
export interface BoardDto {
  id: number // bigint -> number (int64)
  title?: string // nullable string
  description?: string // nullable string
  ownerId: number // bigint -> number (int64)
  createdAt: string // ISO 8601 date-time string
  updatedAt: string // ISO 8601 date-time string
  isArchived: boolean // boolean
}

export interface BoardDetailedDto {
  id: number // bigint -> number (int64)
  title: string // string
  description?: string // nullable string
  members: BoardMemberDto[],
  columns: ColumnDto[]
}

export interface CreateBoardCommand {
  ownerId: number
  title: string
  description?: string
}

/**
 * Command payload for PATCH /api/Boards/{id}
 * Only include the fields you want to change.
 */
export interface UpdateBoardCommand {
  /**
   * Board identifier.
   */
  boardId: number
  /**
   * New title for the board.
   * If you want to clear it, you can pass null.
   */
  title?: string | null

  /**
   * New description for the board.
   * Pass null to clear.
   */
  description?: string | null
}
