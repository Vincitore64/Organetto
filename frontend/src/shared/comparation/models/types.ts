/**
 * @module SortingTypes
 * @description Type definitions for advanced sorting utilities
 */

/**
 * Represents the direction of sorting
 * @typedef {'asc' | 'desc'} SortOrder
 * @property {'asc'} asc - Ascending order (A-Z, 0-9)
 * @property {'desc'} desc - Descending order (Z-A, 9-0)
 * @example
 * const order: SortOrder = 'asc';
 */
type SortOrder = 'asc' | 'desc'

/**
 * Comparator function type for sorting
 * @template T
 * @callback Comparator
 * @param {T} a - First item to compare
 * @param {T} b - Second item to compare
 * @returns {number} Comparison result:
 *   - Negative if a < b
 *   - Positive if a > b
 *   - Zero if equal
 * @example
 * const numberComparator: Comparator<number> = (a, b) => a - b;
 */
type Comparator<T> = (a: T, b: T) => number

/**
 * Configuration for sorting by object properties
 * @template T
 * @typedef {Object} SortConfig
 * @property {SortOrder | Object} [keyof T] - Sorting configuration for each property
 * @property {SortOrder} [key.order] - Sort direction for the property
 * @property {Comparator<T[keyof T]>} [key.comparator] - Custom comparator for the property
 * @example
 * const config: SortConfig<User> = {
 *   age: 'desc',
 *   name: {
 *     order: 'asc',
 *     comparator: (a, b) => a.localeCompare(b)
 *   }
 * };
 */
type SortConfig<T> = {
  [K in keyof T]?:
    | SortOrder
    | {
        order?: SortOrder
        comparator?: (a: T[K], b: T[K]) => number
      }
}

/**
 * Configuration for nested property sorting
 * @typedef {Object} NestedSortConfig
 * @property {SortOrder | Object | null} [string] - Sorting config for nested paths
 * @property {SortOrder} [key.order] - Sort direction for nested property
 * @property {Comparator<unknown>} [key.comparator] - Custom comparator for nested values
 * @example
 * const nestedConfig: NestedSortConfig = {
 *   'profile.age': 'asc',
 *   'department.name': {
 *     order: 'desc',
 *     comparator: (a, b) => customSort(a, b)
 *   }
 * };
 */
type NestedSortConfig = Record<
  string,
  | SortOrder
  | {
      order?: SortOrder
      comparator?: (a: unknown, b: unknown) => number
    }
  | null
>

export { type SortOrder, type Comparator, type SortConfig, type NestedSortConfig }
