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

export interface CreateBoardCommand {
  ownerId: number
  title: string
  description?: string
}
