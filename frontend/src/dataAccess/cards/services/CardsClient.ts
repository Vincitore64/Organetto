import type { ApiException } from '@/dataAccess/shared'
import type { AxiosError, AxiosInstance } from 'axios'
import type { CardDto, CreateCardPayload, DeleteCardPayload, UpdateCardPayload } from '../models'

/**
 * Client for Columns-related endpoints.
 */
export class CardsClient {
  private readonly http: AxiosInstance

  /**
   * @param http - preconfigured Axios instance (baseURL + Authorization header)
   */
  constructor(http: AxiosInstance) {
    this.http = http
  }

  /**
   * GET /api/columns/${columnId}/cards
   * Retrieves all cards for the given column.
   * @param columnId - Internal column ID (int64).
   */
  public async getAll(columnId: number): Promise<CardDto[]> {
    try {
      const response = await this.http.get<CardDto[]>(`/api/columns/${columnId}/cards`)
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
   * POST /api/columns/${columnId}/cards
   * Creates a new card on a column.
   * @param card - Payload with columnId, title, description, position, etc.
   */
  public async create(card: CreateCardPayload): Promise<CardDto> {
    try {
      const response = await this.http.post<CardDto>(
        `/api/columns/${card.columnId}/cards`,
        card,
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
   * PATCH /api/columns/${columnId}/cards/${id}
   * Updates an existing card (e.g. title or position).
   * @param id     - Card identifier.
   * @param card - Partial updates (title?, position?).
   */
  public async update(card: UpdateCardPayload): Promise<CardDto> {
    try {
      const response = await this.http.put<CardDto>(
        `/api/columns/${card.columnId}/cards`,
        card,
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
   * DELETE /api/columns/${columnId}/cards/${id}
   * Deletes the card with the given ID.
   * @param payload - Payload with columnId and id.
   */
  public async delete(payload: DeleteCardPayload): Promise<void> {
    try {
      await this.http.delete<void>(`/api/columns/${payload.columnId}/cards/${payload.id}`)
    } catch (err) {
      const error = err as AxiosError<ApiException>
      if (error.response?.data) {
        throw new Error(error.response.data.message || `API Error: ${error.response.status}`)
      }
      throw err
    }
  }
}