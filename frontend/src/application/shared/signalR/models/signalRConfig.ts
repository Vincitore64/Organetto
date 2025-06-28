import { getApiUrl } from '@/shared'

/**
 * Типы хабов SignalR в приложении
 */
export enum SignalRHubType {
  /** Хаб для работы с досками */
  Boards = 'boards',
  /** Хаб для работы с уведомлениями */
  Notifications = 'notifications',
  /** Хаб для работы с чатом */
  Chat = 'chat',
}

/**
 * Получает URL для подключения к хабу SignalR
 *
 * @param hubType - Тип хаба SignalR
 * @returns URL для подключения к хабу
 */
export function getSignalRHubUrl(hubType: SignalRHubType): string {
  return getApiUrl(`hub/${hubType}`)
}

/**
 * Получает URL для подключения к хабу досок
 *
 * @returns URL для подключения к хабу досок
 */
export function getBoardsHubUrl(): string {
  return getSignalRHubUrl(SignalRHubType.Boards)
}

/**
 * Получает URL для подключения к хабу уведомлений
 *
 * @returns URL для подключения к хабу уведомлений
 */
export function getNotificationsHubUrl(): string {
  return getSignalRHubUrl(SignalRHubType.Notifications)
}

/**
 * Получает URL для подключения к хабу чата
 *
 * @returns URL для подключения к хабу чата
 */
export function getChatHubUrl(): string {
  return getSignalRHubUrl(SignalRHubType.Chat)
}
