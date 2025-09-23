import type { AxiosError } from 'axios'
import type { TwoWayMappers } from '../models'
import { useApiQuery, useApiMutation } from './useApi'
import type { QueryKey, UseQueryOptions } from '@tanstack/vue-query'
import type { ApiException } from '@/dataAccess/shared'
import { notification } from 'ant-design-vue'
import { computed, type Ref } from 'vue'
import type { IHasId } from '@/shared'
import _ from 'lodash'

interface CrudOptions<Client,
  ListArgs extends any[],
  DetailArg,
  TListResp,
  TListData,
  TDetailResp,
  TDetailData,
  TCreateVars,
  TUpdateVars,
  TCreateResp,
  TCreateData> {
  resourceKey: string
  client: Client | (() => Client),
  methods: {
    list: keyof Client
    detail: keyof Client
    create: keyof Client
    update: keyof Client
    remove: keyof Client
  }
  mappers?: TwoWayMappers<TListResp, TListData, TDetailResp, TDetailData, TCreateVars, TUpdateVars, TCreateResp, TCreateData>
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
  DetailArg extends IHasId = any,
  CreateVars = any,
  UpdateVars extends IHasId = any,
  DeleteVars = any,
  TCreateResp = any,
  TCreateData = any,
>(opts: CrudOptions<Client, ListArgs, DetailArg, TResp, TData, TDetailResp, TDetailData, CreateVars, UpdateVars, TCreateResp, TCreateData>) {
  const { resourceKey, client: clientFn, methods, mappers, listOptions, detailOptions } = opts

  // Stable key generators
  const listKey = (...args: ListArgs) => [resourceKey, 'list', ...args] as const
  const defaultListKey = [resourceKey, 'list']
  const detailKey = (id: number) => [resourceKey, 'detail', id] as const
  const client = typeof clientFn === 'function' ? clientFn : () => clientFn

  function useList(...args: ListArgs) {
    const argsRef: Ref<ListArgs> = computed(() => args)
    return useApiQuery(
      computed(() => listKey(...argsRef.value)),
      () => (client()[methods.list] as (...a: ListArgs) => Promise<any>)(...argsRef.value),
      mappers?.list,
      { staleTime: 1000 * 60 * 2, ...listOptions }
    )
  }

  function useDetail(args: DetailArg | Ref<DetailArg>) {
    // debugger
    const argsRef: Ref<DetailArg> = _.isObject(args) && 'value' in args ? args : computed(() => args)
    return useApiQuery(
      computed(() => detailKey(argsRef.value.id)),
      () => (client()[methods.detail] as (payload: DetailArg) => Promise<any>)(argsRef.value),
      mappers?.detail,
      { enabled: !!args, staleTime: 1000 * 60 * 2, ...detailOptions }
    )
  }

  function useCreate() {
    return useApiMutation(
      (vars: CreateVars) => (client()[methods.create] as (v: CreateVars) => Promise<TCreateResp>)(
        mappers?.create ? mappers.create(vars) : vars
      ).then(r => mappers?.created ? mappers.created(r) : r),
      [defaultListKey]
    )
  }

  function useUpdate() {
    return useApiMutation(
      async (vars: UpdateVars) => {
        try {
          return await (client()[methods.update] as (v: UpdateVars) => Promise<any>)(
            mappers?.update ? mappers.update(vars) : vars
          )
        } catch (ex) { // TODO: Rework to notification provider
          const error = ex as AxiosError<ApiException>
          if (error.response?.data) {
            notification.error({
              message: error.response.data.title ?? 'Unknown error',
              description: error.response.data.detail,
            })
          }
          throw ex
        }
      },
      [defaultListKey, detailKey(({} as any as UpdateVars).id)]
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