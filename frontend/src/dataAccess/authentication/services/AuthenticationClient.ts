import type { AxiosInstance } from 'axios'
import type { AuthTokens, LoginRequest, RefreshRequest, RegisterRequest } from '../models'

export class AuthenticationClient {
  constructor(private readonly http: AxiosInstance) {}

  /** POST /auth/register */
  register(payload: RegisterRequest) {
    return this.http.post<void>('/auth/register', payload)
  }

  /** POST /auth/login */
  login(payload: LoginRequest) {
    return this.http.post<AuthTokens>('/auth/login', payload)
  }

  /** POST /auth/refresh */
  refresh(payload: RefreshRequest) {
    return this.http.post<AuthTokens>('/auth/refresh', payload)
  }
}
