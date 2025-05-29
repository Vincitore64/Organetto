import axios, { type AxiosInstance } from 'axios'

export function createAxios(baseURL: string): AxiosInstance {
  const instance = axios.create({ baseURL })
  return instance
}

/**
 * Присоединяет request‑интерцептор, который добавляет JWT‑токен и, при необходимости,
 * может обработать 401 (здесь оставлено в качестве TODO).
 */
export function attachAuthInterceptor(instance: AxiosInstance) {
  instance.interceptors.request.use((config) => {
    const token = localStorage.getItem('accessToken')
    if (token) {
      config.headers = config.headers ?? {}
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  })

  // TODO: Добавить response‑интерцептор для silent‑refresh при 401
}
