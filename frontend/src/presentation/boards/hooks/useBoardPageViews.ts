import type { ApiClient } from '@/dataAccess/services/ApiClient'
import type { BoardPageView } from '../models'
import { defaultThumbnailHref } from '../models'
import _ from 'lodash'
import { computed, ref, type Ref } from 'vue'
import { createAdvancedSorter, ObjectSearch, type SortOrder } from '@/shared'

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
    debugger
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
