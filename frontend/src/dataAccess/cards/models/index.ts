export interface CardDto {
  id: number
  title: string
  description: string
  position: number
  dueDate?: string // ISO 8601 date-time string
}
