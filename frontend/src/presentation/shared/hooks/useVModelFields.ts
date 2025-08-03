import { computed, type WritableComputedRef } from 'vue'
import { useVModel } from '@vueuse/core'
import _ from 'lodash'

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function useVModelFields<TModel extends Record<string, any>, K extends keyof TModel, TName extends string, TProps extends object>(
  props: TProps,
  key: keyof TProps,
  emit: (name: TName, ...args: any[]) => void,
  keys?: K[],
): { [P in K]: WritableComputedRef<TModel[P]> } {
  const model = useVModel(props, key, emit)

  const fields = {} as { [P in K]: WritableComputedRef<TModel[P]> }
  ;(keys ?? (_.keys(props[key]) as K[])).forEach((key) => {
    fields[key] = computed<TModel[typeof key]>({
      get: () => (model.value as TModel)[key],
      set: (val) => {
        model.value = { ...model.value, [key]: val }
      },
    })
  })

  return fields
}
