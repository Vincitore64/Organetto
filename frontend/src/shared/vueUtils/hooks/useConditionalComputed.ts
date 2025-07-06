import { computed, type Ref } from 'vue'

/**
 * Creates a computed property with a conditional setter.
 * @param getter - function returning the current value
 * @param setter - function to call with the new value
 * @param predicate - function which must return true for the setter to run
 */
export function useConditionalComputed<T>(
  getter: () => T,
  setter: (value: T) => void,
  predicate: (value: T) => boolean,
): Ref<T> {
  return computed<T>({
    get() {
      return getter()
    },
    set(value: T) {
      if (predicate(value)) {
        setter(value)
      }
    },
  })
}
