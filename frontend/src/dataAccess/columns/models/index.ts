import type { CardDto } from '@/dataAccess/cards/models'

export interface CreateColumnCommand {
  boardId: number
  title: string
  position?: number
}

export interface UpdateColumnCommand {
  boardId: number
  id: number
  title?: string | null
  position?: number
}

/**
 * Data Transfer Object for a board column.
 */
export interface ColumnDto {
  /**
   * Unique column identifier.
   */
  id: number

  /**
   * Column title (e.g. “To Do”, “In Progress”).
   */
  title: string

  /**
   * Zero-based ordering index for positioning columns left-to-right.
   */
  position: number
  /**
   * Cards in this column.
   */
  cards: CardDto[]
}
