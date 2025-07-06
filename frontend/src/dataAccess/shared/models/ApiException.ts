/** Standard API error payload. */
export interface ApiException {
  status: number
  title?: string
  type?: string
  code?: string
  instance?: string
  errors?: Record<string, string[]>
  message?: string
  data?: Record<string, unknown>
  helpLink?: string
  source?: string
  stackTrace?: string
}
