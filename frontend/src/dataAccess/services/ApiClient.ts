import type { AxiosInstance } from 'axios'
import { AuthenticationClient } from '../authentication/services/AuthenticationClient'
import { inject, injectable } from 'tsyringe'
import { ProvidedService } from '@/shared'

@injectable()
export class ApiClient {
  private _auth: AuthenticationClient

  constructor(@inject(ProvidedService.AxiosInstance) private readonly http: AxiosInstance) {
    this._auth = new AuthenticationClient(this.http)
  }

  /** Lazy getter so we don't construct the client if it isn’t needed. */
  get auth() {
    return this._auth
  }
}
