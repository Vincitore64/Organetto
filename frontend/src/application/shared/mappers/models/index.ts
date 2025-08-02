export type MapperFn<TIn, TOut> = (input: TIn) => TOut

export interface IMapper<TIn, TOut> {
  map: MapperFn<TIn, TOut>
}