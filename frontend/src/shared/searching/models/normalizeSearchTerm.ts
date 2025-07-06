import _ from 'lodash'

export function normalizeSearchTerm(term: string, caseSensitive = false): string {
  const normalizedTerm = caseSensitive ? term : _.toLower(term)
  return normalizedTerm
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // Remove accents
    // .replace(/[^a-z0-9]/g, ''); // Remove special chars
}