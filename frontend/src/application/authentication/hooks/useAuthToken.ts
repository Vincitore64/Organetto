import type { AuthTokens } from '@/dataAccess/authentication/models'
import dayjs from 'dayjs'
import { computed, ref } from 'vue'

export function useAuthToken() {
  const token = ref<AuthTokens | null>(
    localStorage.getItem('auth') ? JSON.parse(localStorage.getItem('auth') ?? 'null') : null,
  )

  /** Derived helper – anywhere in the app you can do: `auth.isAuthenticated` */
  const isAuthenticated = computed(() => !!token.value?.accessToken && !isExpiredFunc(token.value))

  function isExpiredFunc(t: AuthTokens) {
    return dayjs().add(10, 'minutes').isAfter(dayjs(t.expiresAt))
  }

  const isExpired = computed(() => (!token.value ? null : isExpiredFunc(token.value)))

  return {
    token: token,
    isExpired,
    isAuthenticated,
  }
}
