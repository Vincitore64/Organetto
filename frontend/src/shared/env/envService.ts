import type { AppEnv } from './types'

/**
 * Получает значение переменной окружения по ключу
 * 
 * @param key - Ключ переменной окружения
 * @param defaultValue - Значение по умолчанию, если переменная не найдена
 * @returns Значение переменной окружения или значение по умолчанию
 */
export function getEnv<K extends keyof AppEnv>(key: K, defaultValue?: AppEnv[K]): AppEnv[K] {
  const env = import.meta.env as unknown as AppEnv
  return env[key] !== undefined ? env[key] : (defaultValue as AppEnv[K])
}

/**
 * Проверяет, определена ли переменная окружения
 * 
 * @param key - Ключ переменной окружения
 * @returns true, если переменная определена, иначе false
 */
export function hasEnv<K extends keyof AppEnv>(key: K): boolean {
  const env = import.meta.env as unknown as AppEnv
  return env[key] !== undefined
}

/**
 * Получает базовый URL API
 * 
 * @param path - Дополнительный путь, который будет добавлен к базовому URL
 * @returns Полный URL API
 */
export function getApiUrl(path: string = ''): string {
  const baseUrl = getEnv('VITE_API_BASE_URL', 'https://localhost:44322')
  return `${baseUrl}${path.startsWith('/') ? path : `/${path}`}`
}

/**
 * Проверяет, запущено ли приложение в режиме разработки
 * 
 * @returns true, если приложение в режиме разработки, иначе false
 */
export function isDevelopment(): boolean {
  return getEnv('DEV', false)
}

/**
 * Проверяет, запущено ли приложение в режиме продакшн
 * 
 * @returns true, если приложение в режиме продакшн, иначе false
 */
export function isProduction(): boolean {
  return getEnv('PROD', false)
}