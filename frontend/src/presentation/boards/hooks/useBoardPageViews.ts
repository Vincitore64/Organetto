import type { ApiClient } from '@/dataAccess/services/ApiClient'
import type { BoardPageView } from '../models'
import _ from 'lodash'
import { computed, ref, type Ref } from 'vue'
import { createAdvancedSorter, ObjectSearch, type SortOrder } from '@/shared'
import { SignalRHubType } from '@/shared'
import type { BoardDto } from '@/dataAccess/boards/models'
import { useRealtimeCollection } from '@/shared'
import { boardCreated, boardDeleted } from '@/application'
import { mapToBoardPageView } from '../models/mapBoard'

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
    const rawBoards = await apiClient.boards.getAll(payload.userId)

    const allViewsList = _(rawBoards).map(mapToBoardPageView).value()
    // .map((b) => ({
    //     id: b.id,
    //     title: b.title ?? '',
    //     description: b.description ?? '',
    //     thumbnailUrl: defaultThumbnailHref,
    //     createdAt: b.createdAt,
    //     updatedAt: b.updatedAt,
    //     isArchived: b.isArchived,
    //   }))
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
    connect: () => Promise<void>
    disconnect: () => Promise<void>
    error: Ref<Error | null>
  }> {
    const result = await baseFetch(payload)
    const { items, isConnected, connect, disconnect, error } = useRealtimeCollection(
      result.allViews,
      {
        hubType: SignalRHubType.Boards,
        getItemId: (item) => item.id,
        events: {
          created: boardCreated.type,
          deleted: boardDeleted.type,
        },
        mapEventData: (data: unknown) => mapToBoardPageView(data as BoardDto),
        // mapEventData: (data: unknown) => {
        //   const board = data as BoardDto
        //   return {
        //     id: board.id,
        //     title: board.title ?? '',
        //     description: board.description ?? '',
        //     thumbnailUrl: defaultThumbnailHref,
        //     createdAt: board.createdAt,
        //     updatedAt: board.updatedAt,
        //     isArchived: board.isArchived,
        //   }
        // },
        shouldAddItem: (item) => {
          // This is a workaround to get ownerId from the event payload
          // as BoardPageView does not have it.
          const board = item as unknown as BoardDto
          return board.ownerId === payload.userId
        },
      },
    )

    result.allViews = items

    return {
      ...result,
      connect,
      disconnect,
      isConnected,
      error,
    }
  }
}
