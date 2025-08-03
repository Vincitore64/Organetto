// src/composables/useApi.ts
import {
  useQuery,
  useMutation,
  type UseQueryOptions,
  type UseMutationOptions,
  type QueryKey,
  QueryClient,
} from '@tanstack/vue-query'
import { container } from 'tsyringe'

/**
 * Generic wrapper for GET-style calls with explicit mapping
 */
export function useApiQuery<TResp, TData = TResp>(
  key: QueryKey,
  fetcher: () => Promise<TResp>,
  mapper?: (data: TResp) => TData,
  options?: Omit<UseQueryOptions<TData, unknown, TData, QueryKey>, 'queryKey' | 'queryFn'>
) {
  return useQuery<TData, unknown>({
    ...(options as any ?? {}),
    queryKey: key,
    queryFn: async () => {
      const resp = await fetcher()
      return mapper ? mapper(resp) : (resp as unknown as TData)
    },
  }, container.resolve(QueryClient))
}

/**
 * Generic wrapper for mutations with targeted invalidation
 */
export function useApiMutation<TResp, TVars = void>(
  mutationFn: (vars: TVars) => Promise<TResp>,
  invalidateKeys: QueryKey[],
  options?: UseMutationOptions<TResp, unknown, TVars, unknown>
) {
  const qc = container.resolve(QueryClient)

  return useMutation<TResp, unknown, TVars>(
    {
      ...options,
      mutationFn,
      async onSuccess(data, vars, ctx) {
        for (const key of invalidateKeys) {
          qc.invalidateQueries({
            queryKey: key
          })
        }
        (options as any)?.onSuccess?.(data, vars, ctx)
      },
    },
    qc
  )
}
