import type { AxiosInstance } from 'axios'
import { AuthenticationClient } from '../authentication/services/AuthenticationClient'
import { inject, injectable } from 'tsyringe'
import { ProvidedService } from '@/shared'
import { UsersClient } from '../users/services/UsersClient'
import { BoardsClient } from '../boards/services/BoardsClient'

@injectable()
export class ApiClient {
  private _auth: AuthenticationClient
  private _users: UsersClient
  private _boards: BoardsClient

  constructor(@inject(ProvidedService.AxiosInstance) private readonly http: AxiosInstance) {
    this._auth = new AuthenticationClient(this.http)
    this._users = new UsersClient(this.http)
    this._boards = new BoardsClient(this.http)
  }

  /** Lazy getter so we don't construct the client if it isn’t needed. */
  get auth() {
    return this._auth
  }

  /** Lazy getter so we don't construct the client if it isn’t needed. */
  get users() {
    return this._users
  }

  /** Lazy getter so we don't construct the client if it isn’t needed. */
  get boards() {
    return this._boards
  }
}
