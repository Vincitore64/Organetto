import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import type { AuthTokens, LoginRequest, RefreshRequest } from '@/dataAccess/authentication/models'
import type { ApiClient } from '@/dataAccess/services/ApiClient'
import dayjs from 'dayjs'

export const useAuthStore = (apiClient: ApiClient) =>
  defineStore('auth', () => {
    const tokens = ref<AuthTokens | null>(
      localStorage.getItem('auth') ? JSON.parse(localStorage.getItem('auth') ?? 'null') : null,
    )

    /** Derived helper – anywhere in the app you can do: `auth.isAuthenticated` */
    const isAuthenticated = computed(() => !!tokens.value?.accessToken)

    function persist(t?: AuthTokens) {
      if (t) {
        tokens.value = t
        localStorage.setItem('auth', JSON.stringify(t))
      }
    }

    function clear() {
      tokens.value = null
      localStorage.removeItem('auth')
    }

    async function login(payload: LoginRequest) {
      const { data } = await apiClient.auth.login(payload)
      persist(data)
    }

    async function getToken() {
      debugger
      if (!tokens.value) throw new Error('No tokens')
      if (!tokens.value.expiresAt) throw new Error('No expiresAt')
      // Проверяем, истекает ли токен в ближайшие 10 минут
      if (dayjs().add(10, 'minutes').isAfter(dayjs(tokens.value.expiresAt))) await refresh()
      return tokens.value.accessToken
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

    return { tokens, isAuthenticated, login, refresh, logout, getToken }
  })()
