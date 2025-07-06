import {
  HttpTransportType,
  HubConnectionBuilder,
  LogLevel,
  type HubConnection,
} from '@microsoft/signalr'

/**
 * Options for configuring the SignalR connection.
 */
export interface ConnectionOptions {
  /** The absolute URL of the hub, e.g., `https://api.organetto.com/hubs/boards` */
  url: string
  /**
   * An optional function that returns an access token. This is used for authentication.
   * If provided, the token will be included in the connection request.
   */
  getToken?: () => Promise<string>
}

/**
 * Creates a new SignalR HubConnection with the specified options.
 *
 * @param opt - The connection options, including the hub URL and an optional token provider.
 * @returns A Promise that resolves to a configured `HubConnection` instance.
 */
export function createConnection(opt: ConnectionOptions): HubConnection {
  const conn = new HubConnectionBuilder()
    .withUrl(opt.url, {
      accessTokenFactory: opt.getToken,
      transport: HttpTransportType.WebSockets,
      withCredentials: false,
      // queryParams: opt.qs,
    })
    .configureLogging(LogLevel.Warning)
    .withAutomaticReconnect([0, 2_000, 5_000, 10_000])
    .build()

  // await conn.start()
  return conn
}
