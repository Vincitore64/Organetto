import type { MapperFn } from '../mappers'

export interface TwoWayMappers<TListResp, TListData, TDetailResp, TDetailData, TCreateVars = any, TUpdateVars = any> {
  /** transform response array to UI data array */
  list?: MapperFn<TListResp[], TListData[]>
  /** transform response object to UI data */
  detail?: MapperFn<TDetailResp, TDetailData>
  /** transform UI data to request payload */
  create?: MapperFn<TCreateVars, any>
  /** transform UI update data to request payload */
  update?: MapperFn<TUpdateVars, any>
}