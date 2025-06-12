/** Response model for a user. */
export interface UserDto {
  id: number // bigint -> number (int64)
  firebaseUid?: string // nullable string
  email?: string // nullable string
  name?: string // nullable string
  createdAt: string // ISO 8601 date-time string
}
