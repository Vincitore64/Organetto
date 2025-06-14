import _ from 'lodash'
import { normalizeSearchTerm } from './normalizeSearchTerm'

interface SearchConfig<T> {
  keys?: Array<keyof T | string>
  threshold?: number
  caseSensitive?: boolean
}

export class ObjectSearch<T extends object> {
  private items: T[]
  private config: Required<SearchConfig<T>>

  constructor(items: T[], config: SearchConfig<T> = {}) {
    this.items = items
    this.config = {
      keys: _.keys(items[0] || []) as Array<keyof T>,
      threshold: 0.3,
      caseSensitive: false,
      ...config,
    }
  }

  search(term: string): T[] {
    const normalizedTerm = normalizeSearchTerm(term, this.config.caseSensitive)

    return _(this.items).filter((item) => {
      return _(this.config.keys).some((key) => {
        const value = _.toString(_.get(item, key))
        const normalizedValue = normalizeSearchTerm(value, this.config.caseSensitive)
        
        return normalizedValue.includes(normalizedTerm)
      })
    }).value()
  }
}
