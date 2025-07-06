/**
 * Structure returned by /auth/login and /auth/refresh.
 * Adjust the field names if the backend differs.
 */
export interface AuthTokens {
  accessToken: string
  refreshToken: string
  /** Date and time when the token expires (ISO string format) */
  expiresAt: string
  uuid: string
}

export interface RegisterRequest {
  email: string
  password: string
  displayName: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RefreshRequest {
  refreshToken: string
}
