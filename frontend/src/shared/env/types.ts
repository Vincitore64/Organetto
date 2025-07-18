/**
 * Типы для переменных окружения приложения
 */

/**
 * Интерфейс, описывающий все переменные окружения приложения
 */
export interface AppEnv {
  /** Базовый URL API */
  VITE_API_BASE_URL: string
  /** Режим работы приложения (development, production, test) */
  MODE: string
  /** Флаг, указывающий, что приложение запущено в режиме разработки */
  DEV: boolean
  /** Флаг, указывающий, что приложение запущено в режиме продакшн */
  PROD: boolean
  /** Базовый URL приложения */
  BASE_URL: string
}