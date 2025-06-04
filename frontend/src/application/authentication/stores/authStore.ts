import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import type { AuthTokens, LoginRequest, RefreshRequest } from '@/dataAccess/authentication/models'
import type { ApiClient } from '@/dataAccess/services/ApiClient'

export const useAuthStore = (apiClient: ApiClient) =>
  defineStore('auth', () => {
    const tokens = ref<AuthTokens | null>(
      localStorage.getItem('accessToken') && localStorage.getItem('refreshToken')
        ? {
            accessToken: localStorage.getItem('accessToken')!,
            refreshToken: localStorage.getItem('refreshToken')!,
          }
        : null,
    )

    /** Derived helper – anywhere in the app you can do: `auth.isAuthenticated` */
    const isAuthenticated = computed(() => !!tokens.value?.accessToken)

    function persist(t: AuthTokens) {
      tokens.value = t
      localStorage.setItem('accessToken', t.accessToken)
      localStorage.setItem('refreshToken', t.refreshToken)
    }

    function clear() {
      tokens.value = null
      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
    }

    async function login(payload: LoginRequest) {
      const { data } = await apiClient.auth.login(payload)
      persist(data)
    }

    async function refresh() {
      if (!tokens.value) return
      const refreshPayload: RefreshRequest = { refreshToken: tokens.value.refreshToken }
      const { data } = await apiClient.auth.refresh(refreshPayload)
      persist(data)
    }

    function logout() {
      clear()
    }

    return { tokens, isAuthenticated, login, refresh, logout }
  })()
