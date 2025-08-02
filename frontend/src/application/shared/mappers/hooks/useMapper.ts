import { mapValues, isFunction, get } from 'lodash'
import type { MapperFn } from '../models'

/**
 * Mapping rule: either the key/path of a DTO property or a custom transform fn.
 * You may use dot‑notation paths (`'author.name'`) thanks to lodash `get()`.
 */
export type FieldRule<TIn, TOutField> = keyof TIn | string | ((src: TIn) => TOutField)

export type FieldMap<TIn, TOut> = {
  [K in keyof TOut]: FieldRule<TIn, TOut[K]>
}

/**
 * createMapper ⇒ returns a memo‑free mapping function
 *
 * ```ts
 * const mapBoard = createMapper<BoardDTO, BoardVM>({
 *   id:        'id',
 *   title:     'title',
 *   createdAt: d => new Date(d.created_at),
 *   public:    'is_public',
 * })
 * ```
 */
export function createMapper<TIn, TOut>(config: FieldMap<TIn, TOut>) : MapperFn<TIn, TOut> {
  return (src: TIn): TOut =>
    mapValues(config, rule =>
      isFunction(rule)
        ? rule(src)
        : (get(src as any, rule as string | keyof TIn) as unknown)
    ) as TOut
}

/**
 * mapArray helper – validates & maps array payloads in one call.
 */
export const createArrayMapper =
  <TIn, TOut>(mapper: MapperFn<TIn, TOut>) : MapperFn<TIn[], TOut[]> =>
  (items: TIn[]): TOut[] => items.map(mapper)
