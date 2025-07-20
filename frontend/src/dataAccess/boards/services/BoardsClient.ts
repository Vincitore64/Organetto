import type { AxiosError, AxiosInstance } from 'axios'
import type { BoardDto, CreateBoardCommand, UpdateBoardCommand } from '../models'
import type { ApiException } from '@/dataAccess/shared/models/ApiException'
import qs from 'qs'

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
  public async getAll(userId: number): Promise<BoardDto[]> {
    try {
      const response = await this.http.get<BoardDto[]>(
        `/api/Boards?${qs.stringify({ userId: userId })}`,
      )
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response && error.response.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }

  /**
   * POST /api/Boards
   * Creates a new board.
   */
  public async create(board: CreateBoardCommand): Promise<BoardDto> {
    try {
      const response = await this.http.post<BoardDto>(`/api/Boards`, board)
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response && error.response.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }

  /**
   * PATCH /api/Boards/{id}
   * Updates an existing board.
   * @param id - Board identifier
   * @param board - Partial updates (e.g. title, description)
   */
  public async update(board: UpdateBoardCommand): Promise<BoardDto> {
    try {
      const response = await this.http.patch<BoardDto>(`/api/Boards`, board)
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message ?? `API Error: ${error.response.status}`)
      }
      throw err
    }
  }

  /**
   * DELETE /api/Boards/{id}
   * Deletes the board with the given id.
   * @param id - Board identifier
   */
  public async delete(id: number): Promise<void> {
    try {
      await this.http.delete<void>(`/api/Boards/${id}`)
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message ?? `API Error: ${error.response.status}`)
      }
      throw err
    }
  }
}
