import type { Comparator } from './types'

function compose<T>(...comparators: Comparator<T>[]): Comparator<T> {
  return (a, b) => {
    for (const compare of comparators) {
      const result = compare(a, b)
      if (result !== 0) return result
    }
    return 0
  }
}

export { compose }
