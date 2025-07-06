import { useAsyncState } from '@vueuse/core'
import type { ApiClient } from '@/dataAccess/services/ApiClient'
import type { CreateBoardState } from '../models'
import { useUsersComposable } from '@/application/users/hooks/useUsers'

/**
 * Wraps the board creation call into a composable that exposes loading & error state.
 */
export function useCreateBoard(apiClient: ApiClient) {
  const users = useUsersComposable(apiClient)
  return useAsyncState(
    async ({ name, description }: CreateBoardState) => {
      debugger
      const currentUser = await users.getCurrentUser()
      return await apiClient.boards.create({ ownerId: currentUser.id, title: name, description })
    },
    null,
    { immediate: false },
  )
}
