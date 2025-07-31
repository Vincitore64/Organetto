export interface CardDto {
  id: number
  title: string
  description: string
  position: number
  dueDate?: string // ISO 8601 date-time string
}

export interface CreateCardPayload {
  columnId: number
  title: string
  description: string
  position: number
  dueDate?: string // ISO 8601 date-time string
}

export interface UpdateCardPayload {
  columnId: number
  id: number
  title: string
  description: string
  position: number
  dueDate?: string // ISO 8601 date-time string
}

export interface DeleteCardPayload {
  columnId: number
  id: number
}
