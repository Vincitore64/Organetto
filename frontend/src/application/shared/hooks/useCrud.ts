// src/composables/crud.ts
import { useApiQuery, useApiMutation } from './useApi'
import type { QueryKey, UseQueryOptions } from '@tanstack/vue-query'

interface CrudOptions<Client, ListArgs extends any[], DetailArg, CreateVars, UpdateVars> {
  resourceKey: string
  client: Client | (() => Client),
  methods: {
    list: keyof Client
    detail: keyof Client
    create: keyof Client
    update: keyof Client
    remove: keyof Client
  }
  /** default options for list query */
  listOptions?: UseQueryOptions<any, unknown, any, QueryKey>
  /** default options for detail query */
  detailOptions?: UseQueryOptions<any, unknown, any, QueryKey>
}

export function createCrudHooks<
  Client extends Record<string, any>,
  ListArgs extends any[] = any[],
  DetailArg = any,
  CreateVars = any,
  UpdateVars = any,
>(opts: CrudOptions<Client, ListArgs, DetailArg, CreateVars, UpdateVars>) {
  const { resourceKey, client: clientFn, methods, listOptions, detailOptions } = opts

  // Stable key generators
  const listKey = (...args: ListArgs) => [resourceKey, 'list', ...args] as const
  const defaultListKey = [resourceKey, 'list']
  const detailKey = (id: DetailArg) => [resourceKey, 'detail', id] as const
  const client = typeof clientFn === 'function' ? clientFn : () => clientFn

  function useList(...args: ListArgs) {
    return useApiQuery(
      listKey(...args),
      () => (client()[methods.list] as (...a: ListArgs) => Promise<any>)(...args),
      undefined,
      { staleTime: 1000 * 60 * 2, ...listOptions }
    )
  }

  function useDetail(id: DetailArg) {
    debugger
    return useApiQuery(
      detailKey(id),
      () => (client()[methods.detail] as (id: DetailArg) => Promise<any>)(id),
      undefined,
      { enabled: !!id, staleTime: 1000 * 60 * 2, ...detailOptions }
    )
  }

  function useCreate() {
    return useApiMutation(
      (vars: CreateVars) => (client()[methods.create] as (v: CreateVars) => Promise<any>)(vars),
      [defaultListKey]
    )
  }

  function useUpdate() {
    return useApiMutation(
      (vars: UpdateVars & { id: DetailArg }) => (client()[methods.update] as (v: UpdateVars & { id: DetailArg }) => Promise<any>)(vars),
      [defaultListKey, detailKey(({} as any as UpdateVars & { id: DetailArg }).id)]
    )
  }

  function useRemove() {
    return useApiMutation(
      (id: DetailArg) => (client()[methods.remove] as (id: DetailArg) => Promise<any>)(id),
      [defaultListKey]
    )
  }

  return {
    useList,
    useDetail,
    useCreate,
    useUpdate,
    useRemove,
  }
}