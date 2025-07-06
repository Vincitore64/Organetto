import { container } from 'tsyringe'
import { SignalRClient, type Handler } from '../services'
import { getSignalRHubUrl, SignalRHubType } from '@/shared'
import { ref, onUnmounted } from 'vue'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { useAuthStore } from '@/application/authentication/stores/authStore'
import qs from 'qs'
import { useUsersComposable } from '@/application/users/hooks/useUsers'
import { useUsersStore } from '@/application/users/stores/usersStore'

/**
 * Хук для работы с SignalR клиентом
 *
 * @param hubType - Тип хаба SignalR для подключения
 * @returns Объект с методами для работы с SignalR
 */
export function useSignalRClient(hubType: SignalRHubType) {
  // const container = tryInjectServices()
  const apiClient = container.resolve(ApiClient)
  const authStore = useAuthStore(apiClient)
  const users = useUsersComposable(apiClient, useUsersStore())

  const client = ref<SignalRClient | null>(null)
  const isConnected = ref(false)
  const error = ref<Error | null>(null)

  /**
   * Инициализирует подключение к хабу
   */
  const connect = async () => {
    try {
      if (client.value) {
        await disconnect()
      }
      const currentUser = await users.getCurrentUser()
      const hubUrl = getSignalRHubUrl(hubType) + '?' + qs.stringify({ userId: currentUser.id })

      client.value = SignalRClient.create({
        url: hubUrl,
        getToken: async () => authStore.getToken(),
      })

      await client.value.start()
      isConnected.value = true
      error.value = null
    } catch (err) {
      error.value = err instanceof Error ? err : new Error(String(err))
      isConnected.value = false
    }
  }

  /**
   * Отключается от хаба
   */
  const disconnect = async () => {
    if (client.value) {
      await client.value.stop()
      client.value = null
      isConnected.value = false
    }
  }

  /**
   * Подписывается на событие от хаба
   *
   * @param method - Название метода
   * @param handler - Обработчик события
   */
  const on = <T>(method: string, handler: Handler<T>) => {
    if (client.value) {
      client.value.on(method, handler)
    } else {
      console.warn('SignalR client is not initialized. Call connect() first.')
    }
  }

  /**
   * Отправляет сообщение на хаб
   *
   * @param method - Название метода
   * @param payload - Данные для отправки
   */
  const send = async <T>(method: string, payload: T) => {
    if (client.value && isConnected.value) {
      await client.value.send(method, payload)
    } else {
      console.warn('SignalR client is not connected. Call connect() first.')
    }
  }

  // Автоматическое отключение при размонтировании компонента
  onUnmounted(() => {
    disconnect()
  })

  return {
    connect,
    disconnect,
    on,
    send,
    isConnected,
    error,
  }
}
