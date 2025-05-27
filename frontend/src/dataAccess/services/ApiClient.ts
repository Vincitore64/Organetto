import type { AxiosInstance } from 'axios'
import { AuthenticationClient } from '../authentication/services/AuthenticationClient'
import axios from 'axios'

export class ApiClient {
  private _auth: AuthenticationClient
  private readonly http: AxiosInstance

  constructor(baseURL: string = import.meta.env.VITE_API_BASE_URL) {
    this.http = axios.create({ baseURL })

    // Attach token to every request automatically (reads from localStorage)
    this.http.interceptors.request.use((config) => {
      const token = localStorage.getItem('accessToken')
      if (token) {
        config.headers = config.headers ?? {}
        config.headers['Authorization'] = `Bearer ${token}`
      }
      return config
    })
    this._auth = new AuthenticationClient(this.http)
  }

  /** Lazy getter so we don't construct the client if it isn’t needed. */
  get auth() {
    return this._auth
  }
}
