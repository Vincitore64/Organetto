/**
 * @module ApiHandlers
 * @description Provides utility functions for creating API handler functions with AviscloudClient
 */

import type { ApiClient } from '@/dataAccess/services/ApiClient'
import { useAsyncState } from '@vueuse/core'

/**
 * Creates a basic API handler function that executes a client method and processes the response
 * @function useApiHandler
 * @template Response - The type of the API response
 * @template Params - The type of parameters the API method accepts (defaults to any[])
 * @param {ApiClient} client - The Aviscloud client instance
 * @param {function} method - A function that takes the client and returns an API method
 * @returns {function} A handler function that:
 *   - Takes a transformation function and method parameters
 *   - Executes the API call
 *   - Applies the transformation to the response
 * 
 * @example
 * // Basic usage
 * const handler = useApiHandler(client, c => c.someApiMethod);
 * const result = await handler(
 *   response => transformResponse(response),
 *   param1, 
 *   param2
 * );
 */
export function useApiHandler<Response, Params extends any[] = any[]>(client: ApiClient, method: (client: ApiClient) => (...args: Params) => Promise<Response>) {
  return function<Result>(next: (r: Response) => Promise<Result>, fallback?: (r: any) => Promise<void>,  ...args: Params) {
    return method(client)(...args).then(next).catch(fallback)
  }
}

/**
 * Creates a more flexible API handler with method chaining support (V2)
 * @function useApiHandlerV2
 * @template Response - The type of the API response
 * @template Params - The type of parameters the API method accepts (defaults to any[])
 * @param {ApiClient} client - The Aviscloud client instance
 * @returns {function} A curried handler creator that:
 *   1. Takes an API method
 *   2. Takes method parameters
 *   3. Takes an optional transformation function
 *   4. Executes the chain
 * 
 * @example
 * // Basic usage
 * const handler = useApiHandlerV2(client)(c => c.someApiMethod)(param1, param2)();
 * 
 * @example
 * // With transformation
 * const result = await useApiHandlerV2(client)
 *   (c => c.someApiMethod)
 *   (param1, param2)
 *   (response => transformResponse(response));
 * 
 * @example
 * // Partial application
 * const methodHandler = useApiHandlerV2(client)(c => c.someApiMethod);
 * const paramHandler = methodHandler(param1, param2);
 * const result = await paramHandler(response => transformResponse(response));
 */
export function useApiHandlerV2<Response, Params extends any[] = any[]>(client: ApiClient) {
  return function (method: (client: ApiClient) => (...args: Params) => Promise<Response>) {
    return function (...args: Params) {
      return async function<Result>(
        next?: (r: Response) => Promise<Result>,
        fallback?: (r: any) => Promise<void> | void
      ) : Promise<Result> {
        try {
          const r = await method(client)(...args)
          if (!next) return await r as Result
          return await next(r)
        } catch (ex) {
          if (!fallback) throw ex
          await fallback(ex)
          throw ex
        }
      }
    }
  }
}

export function useApiHandlerV3<TClient, Response, Params extends any[] = any[]>(client: TClient) {
  return function (method: (client: TClient) => (...args: Params) => Promise<Response>) {
    return function (...args: Params) {
      return async function<Result>(
        next?: (r: Response) => Promise<Result>,
        fallback?: (r: any) => Promise<void> | void
      ) : Promise<Result> {
        try {
          const r = await method(client)(...args)
          if (!next) return await r as Result
          return await next(r)
        } catch (ex) {
          if (!fallback) throw ex
          await fallback(ex)
          throw ex
        }
      }
    }
  }
}

export function useApiState<TClient, Response, Params extends any[] = any[]>(client: TClient) {
  return function (method: (client: TClient) => (...args: Params) => Promise<Response>) {
    return function (...args: Params) {
      return function<Result = Response>(
        next?: (r: Response) => Promise<Result>,
        fallback?: (r: any) => Promise<void> | void
      ) {
        return useAsyncState(async () => {
          try {
            const r = await method(client)(...args)
            if (!next) return r as Result
            return await next(r)
          } catch (ex) {
            debugger
            throw ex
          }
        }, null, { onError: fallback, immediate: false })
      }
    }
  }
}