import type { TwoWayMappers } from '../models'
import { useApiQuery, useApiMutation } from './useApi'
import type { QueryKey, UseQueryOptions } from '@tanstack/vue-query'

interface CrudOptions<Client,
  ListArgs extends any[],
  DetailArg,
  TListResp,
  TListData,
  TDetailResp,
  TDetailData,
  TCreateVars,
  TUpdateVars> {
  resourceKey: string
  client: Client | (() => Client),
  methods: {
    list: keyof Client
    detail: keyof Client
    create: keyof Client
    update: keyof Client
    remove: keyof Client
  }
  mappers?: TwoWayMappers<TListResp, TListData, TDetailResp, TDetailData, TCreateVars, TUpdateVars>
  /** default options for list query */
  listOptions?: UseQueryOptions<any, unknown, any, QueryKey>
  /** default options for detail query */
  detailOptions?: UseQueryOptions<any, unknown, any, QueryKey>
}

export function createCrudHooks<
  Client extends Record<string, any>,
  ListArgs extends any[] = any[],
  TResp = any,
  TData = any,
  TDetailResp = any,
  TDetailData = any,
  DetailArg = any,
  CreateVars = any,
  UpdateVars = any,
  DeleteVars = any,
>(opts: CrudOptions<Client, ListArgs, DetailArg, TResp, TData, TDetailResp, TDetailData, CreateVars, UpdateVars>) {
  const { resourceKey, client: clientFn, methods, mappers, listOptions, detailOptions } = opts

  // Stable key generators
  const listKey = (...args: ListArgs) => [resourceKey, 'list', ...args] as const
  const defaultListKey = [resourceKey, 'list']
  const detailKey = (id: DetailArg) => [resourceKey, 'detail', id] as const
  const client = typeof clientFn === 'function' ? clientFn : () => clientFn

  function useList(...args: ListArgs) {
    return useApiQuery(
      listKey(...args),
      () => (client()[methods.list] as (...a: ListArgs) => Promise<any>)(...args),
      mappers?.list,
      { staleTime: 1000 * 60 * 2, ...listOptions }
    )
  }

  function useDetail(id: DetailArg) {
    debugger
    return useApiQuery(
      detailKey(id),
      () => (client()[methods.detail] as (id: DetailArg) => Promise<any>)(id),
      mappers?.detail,
      { enabled: !!id, staleTime: 1000 * 60 * 2, ...detailOptions }
    )
  }

  function useCreate() {
    return useApiMutation(
      (vars: CreateVars) => (client()[methods.create] as (v: CreateVars) => Promise<any>)(
        mappers?.create ? mappers.create(vars) : vars
      ),
      [defaultListKey]
    )
  }

  function useUpdate() {
    return useApiMutation(
      (vars: UpdateVars & { id: DetailArg }) => (client()[methods.update] as (v: UpdateVars & { id: DetailArg }) => Promise<any>)(
        mappers?.update ? mappers.update(vars) : vars
      ),
      [defaultListKey, detailKey(({} as any as UpdateVars & { id: DetailArg }).id)]
    )
  }

  function useRemove() {
    return useApiMutation(
      (deleteArgs: DeleteVars) => (client()[methods.remove] as (v: DeleteVars) => Promise<any>)(deleteArgs),
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