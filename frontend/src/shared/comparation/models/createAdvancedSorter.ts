import _ from 'lodash'
import type { Comparator, NestedSortConfig } from './types'

/**
 * Creates a comparator function for sorting objects based on nested property paths and custom rules
 * @template T - The type of objects being sorted
 * @function createAdvancedSorter
 * @param {NestedSortConfig} config - Configuration object defining sorting rules
 * @returns {Comparator<T>} A comparator function usable with Array.prototype.sort
 * 
 * @example
 * // Basic usage with nested properties
 * const sorter = createAdvancedSorter({
 *   'profile.age': 'desc',
 *   'name': 'asc'
 * });
 * users.sort(sorter);
 * 
 * @example
 * // With custom comparators
 * const complexSorter = createAdvancedSorter({
 *   'department': {
 *     order: 'asc',
 *     comparator: (a, b) => customDeptSort(a, b)
 *   },
 *   'hireDate': 'desc'
 * });
 * employees.sort(complexSorter);
 * 
 * @description
 * The sorter handles multiple data types automatically:
 * - Numbers: Numerical comparison
 * - Dates: Chronological comparison
 * - Strings: Locale-aware comparison
 * - Custom: User-provided comparator functions
 * 
 * Sorting behavior:
 * 1. Processes properties in config object order
 * 2. Returns first non-zero comparison result
 * 3. Considers null/undefined values
 * 4. Supports deep nested paths (using Lodash.get)
 */
function createAdvancedSorter<T>(config: NestedSortConfig): Comparator<T> {
  return (a, b) => {
    for (const key in config) {
      const keyConfig = config[key]
      if (!keyConfig) continue
      const order = typeof keyConfig === 'object' ? keyConfig.order : keyConfig
      const customComparator = typeof keyConfig === 'object' ? keyConfig.comparator : null

      const aValue = _.get(a, key)
      const bValue = _.get(b, key)

      let result = 0

      if (customComparator) {
        result = customComparator(aValue, bValue)
      } else if (typeof aValue === 'number' && typeof bValue === 'number') {
        result = aValue - bValue
      } else if (aValue instanceof Date && bValue instanceof Date) {
        result = aValue.getTime() - bValue.getTime()
      } else {
        result = _.toString(aValue).localeCompare(_.toString(bValue))
      }

      if (result !== 0) {
        return order === 'desc' ? -result : result
      }
    }
    return 0
  }
}

export { createAdvancedSorter }
