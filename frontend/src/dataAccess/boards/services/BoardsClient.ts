import type { AxiosError, AxiosInstance } from 'axios'
import type { BoardDto } from '../models'
import type { ApiException } from '@/dataAccess/shared/models/ApiException'

/**
 * Client for Boards-related endpoints.
 */
export class BoardsClient {
  private readonly http: AxiosInstance

  /**
   * @param baseUrl - Base URL of the Organetto backend (e.g. 'https://api.organetto.com').
   * @param token   - Optional JWT token for Authorization header.
   */
  constructor(http: AxiosInstance) {
    this.http = http
  }

  /**
   * GET /api/Boards/{userId}
   * Retrieves all boards where the given user is owner or member.
   * @param userId - Internal user ID (int64).
   */
  public async getBoards(userId: number): Promise<BoardDto[]> {
    try {
      const response = await this.http.get<BoardDto[]>(`/api/Boards/${userId}`)
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
