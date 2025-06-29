import type { HubConnection } from '@microsoft/signalr'
import { createConnection, type ConnectionOptions } from '../api/createConnection'

export type Handler<T> = (payload: T) => void

export class SignalRClient {
  constructor(private connection: HubConnection) {}

  public static create(opts: ConnectionOptions) {
    return new SignalRClient(createConnection(opts))
  }

  /** start + ensure handlers are wired before first server push */
  async start(): Promise<void> {
    await this.connection.start() // docs recommend wiring .on() before start :contentReference[oaicite:5]{index=5}
  }

  on<T>(method: string, cb: Handler<T>) {
    this.connection.on(method, cb)
  }

  async send<T>(method: string, payload: T) {
    await this.connection.send(method, payload)
  }

  async stop() {
    await this.connection.stop()
  }
}
