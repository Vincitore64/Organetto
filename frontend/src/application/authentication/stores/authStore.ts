import { defineStore } from 'pinia'
import type { AuthTokens, LoginRequest, RefreshRequest } from '@/dataAccess/authentication/models'
import type { ApiClient } from '@/dataAccess/services/ApiClient'
import { useAuthToken } from '../hooks/useAuthToken'

export const useAuthStore = (apiClient: ApiClient) =>
  defineStore('auth', () => {
    const tokenWrapper = useAuthToken()
    // const tokenWrapper.token = ref<AuthTokens | null>(
    //   localStorage.getItem('auth') ? JSON.parse(localStorage.getItem('auth') ?? 'null') : null,
    // )

    // /** Derived helper – anywhere in the app you can do: `auth.isAuthenticated` */
    // const isAuthenticated = computed(() => !!tokenWrapper.token.value?.accessToken && !isExpired(tokenWrapper.token.value))

    // function isExpired(t: AuthTokens) {
    //   return dayjs().add(10, 'minutes').isAfter(dayjs(t.expiresAt))
    // }

    function persist(t?: AuthTokens) {
      if (t) {
        tokenWrapper.token.value = t
        localStorage.setItem('auth', JSON.stringify(t))
      }
    }

    function clear() {
      tokenWrapper.token.value = null
      localStorage.removeItem('auth')
    }

    async function login(payload: LoginRequest) {
      const { data } = await apiClient.auth.login(payload)
      persist(data)
    }

    async function getToken() {
      if (!tokenWrapper.token.value) throw new Error('No tokenWrapper.token')
      if (!tokenWrapper.token.value.expiresAt) throw new Error('No expiresAt')
      // Проверяем, истекает ли токен в ближайшие 10 минут
      if (tokenWrapper.isExpired.value) await refresh()
      return tokenWrapper.token.value.accessToken
    }

    async function refresh() {
      if (!tokenWrapper.token.value) return
      const refreshPayload: RefreshRequest = { refreshToken: tokenWrapper.token.value.refreshToken }
      const { data } = await apiClient.auth.refresh(refreshPayload)
      persist(data)
    }

    function logout() {
      clear()
    }

    return {
      tokens: tokenWrapper.token,
      isAuthenticated: tokenWrapper.isAuthenticated,
      login,
      refresh,
      logout,
      getToken,
    }
  })()
