import type { AxiosError, AxiosInstance } from 'axios'
import type { UserDto } from '../models'
import type { ApiException } from '@/dataAccess/shared/models/ApiException'

/**
 * Client for Users-related endpoints.
 */
export class UsersClient {
  private readonly http: AxiosInstance

  /**
   * @param baseUrl - Base URL of the Organetto backend.
   * @param token   - Optional JWT token for Authorization header.
   */
  constructor(http: AxiosInstance) {
    this.http = http
  }

  /**
   * GET /api/Users/{uid}
   * Retrieves a user by their Firebase UID.
   * @param uid - Firebase UID (string).
   */
  public async getUserByUid(uid: string): Promise<UserDto> {
    try {
      const response = await this.http.get<UserDto>(`/api/Users/${uid}`)
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response && error.response.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }
}
