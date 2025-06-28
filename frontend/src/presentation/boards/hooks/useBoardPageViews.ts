import type { ApiClient } from '@/dataAccess/services/ApiClient'
import type { BoardPageView } from '../models'
import { defaultThumbnailHref } from '../models'
import _ from 'lodash'
import { computed, ref, type Ref } from 'vue'
import { createAdvancedSorter, ObjectSearch, type SortOrder } from '@/shared'
import { useSignalRClient } from '@/application/shared'
import { SignalRHubType } from '@/shared/env'
import type { BoardDto } from '@/dataAccess/boards/models'

/**
 * Higher-order composable for fetching BoardPageView data.
 * @param apiClient - injected API client from dataAccess
 * @returns fetchBoardPageViews function
 */
export function useBoardPageViews(apiClient: ApiClient) {
  /**
   * Fetches a list of BoardPageView objects for rendering the Boards page.
   * @param payload.userId - current user's internal ID
   * @param payload.sortBy  - optional sort key, e.g. 'updatedAt' | 'title'
   * @param payload.search  - optional search query to filter boards by title
   */
  return async function fetchBoardPageViews(payload: { userId: number }): Promise<{
    allViews: Ref<BoardPageView[]>
    searchTerms: Ref<string>
    sortBy: Ref<string>
    sortDirection: Ref<SortOrder>
    views: Ref<BoardPageView[]>
  }> {
    const rawBoards = await apiClient.boards.getBoards(payload.userId)

    const allViewsList = _(rawBoards)
      .map((b) => ({
        id: b.id,
        title: b.title ?? '',
        description: b.description ?? '',
        thumbnailUrl: defaultThumbnailHref,
        createdAt: b.createdAt,
        updatedAt: b.updatedAt,
        isArchived: b.isArchived,
      }))
      .value()
    const allViews = ref(allViewsList)
    const searchTerms = ref<string>('')
    const sortBy = ref<string>('title')
    const sortDirection = ref<SortOrder>('asc')

    return {
      allViews: allViews,
      searchTerms,
      sortBy,
      sortDirection,
      views: computed(() => {
        const searchObject = new ObjectSearch(allViews.value, {
          keys: ['title'],
        })
        const searched = searchObject.search(searchTerms.value)
        const sorter = createAdvancedSorter<BoardPageView>({
          [sortBy.value]: sortDirection.value,
        })
        const sorted = searched.sort(sorter)
        return sorted
      }),
    }
  }
}

/**
 * Декоратор для useBoardPageViews, добавляющий функционал realtime обновления досок.
 * @param apiClient - injected API client from dataAccess
 * @returns fetchBoardPageViews function с поддержкой realtime обновлений
 */
export function useRealtimeBoardPageViews(apiClient: ApiClient) {
  const baseFetch = useBoardPageViews(apiClient)

  /**
   * Fetches a list of BoardPageView objects and subscribes to realtime updates.
   * @param payload.userId - current user's internal ID
   */
  return async function fetchBoardPageViewsWithRealtime(payload: { userId: number }): Promise<{
    allViews: Ref<BoardPageView[]>
    searchTerms: Ref<string>
    sortBy: Ref<string>
    sortDirection: Ref<SortOrder>
    views: Ref<BoardPageView[]>
    isConnected: Ref<boolean>
    error: Ref<Error | null>
  }> {
    // Получаем базовый результат
    const result = await baseFetch(payload)

    // Инициализируем SignalR клиент для хаба досок
    const { connect, on, isConnected, error } = useSignalRClient(SignalRHubType.Boards)

    // Подключаемся к хабу
    await connect()

    // Обработчик создания новой доски
    on<BoardDto>('BoardCreated', (board) => {
      if (board.ownerId === payload.userId) {
        const newBoard: BoardPageView = {
          id: board.id,
          title: board.title ?? '',
          thumbnailUrl: defaultThumbnailHref,
        }
        result.allViews.value = [...result.allViews.value, newBoard]
      }
    })

    // Обработчик обновления доски
    on<BoardDto>('BoardUpdated', (board) => {
      const index = result.allViews.value.findIndex((b) => b.id === board.id)
      if (index !== -1) {
        const updatedBoard: BoardPageView = {
          id: board.id,
          title: board.title ?? '',
          thumbnailUrl: result.allViews.value[index].thumbnailUrl,
        }
        result.allViews.value = [
          ...result.allViews.value.slice(0, index),
          updatedBoard,
          ...result.allViews.value.slice(index + 1),
        ]
      }
    })

    // Обработчик удаления доски
    on<number>('BoardDeleted', (boardId) => {
      result.allViews.value = result.allViews.value.filter((b) => b.id !== boardId)
    })

    // Обработчик архивации доски
    on<BoardDto>('BoardArchived', (board) => {
      const index = result.allViews.value.findIndex((b) => b.id === board.id)
      if (index !== -1) {
        const archivedBoard = { ...result.allViews.value[index], isArchived: true }
        result.allViews.value = [
          ...result.allViews.value.slice(0, index),
          archivedBoard,
          ...result.allViews.value.slice(index + 1),
        ]
      }
    })

    return {
      ...result,
      isConnected,
      error,
    }
  }
}
