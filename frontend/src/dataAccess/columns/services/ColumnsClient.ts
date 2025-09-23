import type { ApiException } from '@/dataAccess/shared'
import type { AxiosError, AxiosInstance } from 'axios'
import type { ColumnDto, CreateColumnCommand, DeleteColumnCommand, MoveColumnCommand, UpdateColumnCommand } from '../models'

/**
 * Client for Columns-related endpoints.
 */
export class ColumnsClient {
  private readonly http: AxiosInstance

  /**
   * @param http - preconfigured Axios instance (baseURL + Authorization header)
   */
  constructor(http: AxiosInstance) {
    this.http = http
  }

  /**
   * GET /api/boards/${boardId}/columns
   * Retrieves all columns for the given board.
   * @param boardId - Internal board ID (int64).
   */
  public async getAll(boardId: number): Promise<ColumnDto[]> {
    try {
      const response = await this.http.get<ColumnDto[]>(`/api/boards/${boardId}/columns`)
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }

  /**
   * POST /api/boards/${boardId}/columns
   * Creates a new column on a board.
   * @param column - Payload with boardId, title, position, etc.
   */
  public async create(column: CreateColumnCommand): Promise<ColumnDto> {
    try {
      const response = await this.http.post<ColumnDto>(
        `/api/boards/${column.boardId}/columns`,
        column,
      )
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }

  /**
   * PATCH /api/Columns/{id}
   * Updates an existing column (e.g. title or position).
   * @param id     - Column identifier.
   * @param column - Partial updates (title?, position?).
   */
  public async update(column: UpdateColumnCommand): Promise<ColumnDto> {
    try {
      const response = await this.http.patch<ColumnDto>(
        `/api/boards/${column.boardId}/columns`,
        column,
      )
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }

  /**
   * PUT /api/boards/${targetBoardId}/columns/move
   * Moves a column to a new position.
   * @param payload - Payload with listId, targetBoardId, leftSiblingId, rightSiblingId.
   */
  public async move(payload: MoveColumnCommand): Promise<ColumnDto[]> {
    try {
      const response = await this.http.put<ColumnDto[]>(`/api/boards/${payload.targetBoardId}/columns/move`, payload)
      return response.data
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }

  /**
   * DELETE /api/boards/${boardId}/columns/${id}
   * Deletes the column with the given ID.
   * @param id - Column identifier.
   */
  public async delete(payload: DeleteColumnCommand): Promise<void> {
    try {
      await this.http.delete<void>(`/api/boards/${payload.boardId}/columns/${payload.id}`)
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }
}
