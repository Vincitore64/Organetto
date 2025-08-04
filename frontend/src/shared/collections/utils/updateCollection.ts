import { type Ref } from 'vue'
import _ from 'lodash'

/**
 * Finds the first element in `collectionRef` matching `predicate`,
 * deep‐clones it, runs `updater(clone)`, and then replaces it
 * in the array, triggering Vue reactivity.
 *
 * @template T  — type of items in the array
 * @param collectionRef  — a Vue Ref holding an array of T
 * @param predicate      — identifies which item to update
 * @param updater        — mutation to perform on the cloned item
 */
function updateCollectionItem<T>(
  collectionRef: Ref<T[]>,
  predicate: (item: T) => boolean,
  updater: (clone: T) => void
): void {
  collectionRef.value = collectionRef.value.map(item => {
    if (!predicate(item)) return item
    const clone = _.cloneDeep(item)
    updater(clone)
    return clone
  })
}

export { updateCollectionItem }