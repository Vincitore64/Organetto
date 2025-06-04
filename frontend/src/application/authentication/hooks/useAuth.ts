import { useAsyncState } from '@vueuse/core'
import { useAuthStore } from '../stores/authStore'
import type { RegisterRequest, LoginRequest } from '@/dataAccess/authentication/models'
import type { ApiClient } from '@/dataAccess/services/ApiClient'

/**
 * Wraps the registration call into a composable that exposes loading & error state.
 */
export function useRegister(apiClient: ApiClient) {
  return useAsyncState(
    async (payload: RegisterRequest) => {
      await apiClient.auth.register(payload)
      // Registration doesn’t log the user in by itself (change if backend does)
      return true
    },
    null,
    { immediate: false },
  )
}

/**
 * Wraps login so that the store is automatically updated and components can reactively
 * track `isAuthenticated`, `error`, etc.
 */
export function useLogin(apiClient: ApiClient) {
  const authStore = useAuthStore(apiClient)

  return useAsyncState(
    async (payload: LoginRequest) => {
      await authStore.login(payload)
      return true
    },
    null,
    { immediate: false },
  )
}
