import { computed, type ComputedRef } from 'vue'
import { useVModel } from '@vueuse/core'
import _ from 'lodash'

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function useVModelFields<TModel extends Record<string, any>, K extends keyof TModel>(
  props: { modelValue: TModel },
  emit: (e: 'update:modelValue', value: TModel) => void,
  keys?: K[],
): { [P in K]: ComputedRef<TModel[P]> } {
  const model = useVModel(props, 'modelValue', emit)

  const fields = {} as { [P in K]: ComputedRef<TModel[P]> }
  ;(keys ?? (_.keys(props.modelValue) as K[])).forEach((key) => {
    fields[key] = computed<TModel[typeof key]>({
      get: () => model.value[key],
      set: (val) => {
        model.value = { ...model.value, [key]: val }
      },
    })
  })

  return fields
}
