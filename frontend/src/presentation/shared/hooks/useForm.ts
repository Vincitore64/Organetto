import { reactive, ref, type Ref } from 'vue'
import { merge, cloneDeep, debounce, assign } from 'lodash'
import { Form } from 'ant-design-vue'
// import type { FormInstance } from 'ant-design-vue'
import type { Rule as FormRule } from 'ant-design-vue/lib/form/interface'
import { computedAsync } from '@vueuse/core'

export type FormInstance = ReturnType<typeof Form.useForm>

/**
 * Universal form hook with Ant Design Vue integration:
 * - Deep defaults and resets via Lodash
 * - Optional debouncing
 * - Optional Ant Design Form instance and validation rules
 */
export interface UseFormOptions<
  TParams extends object,
  TReturn,
  TActionParams extends object = TParams,
> {
  initialValues: TParams
  action: (params: TActionParams) => Promise<TReturn>
  validate?: ((values: TParams) => Promise<boolean>) | ((values: TParams) => boolean)
  transform?: ((values: TParams) => Promise<TActionParams>) | ((values: TParams) => TActionParams)
  debounceMs?: number
  /** Ant Design Vue form validation rules */
  rules?: Record<keyof TParams, FormRule[]>
  /** Enable Ant Design Vue form instance and validation */
  useAntd?: boolean
  onBefore?: (values: TParams) => void
  onSuccess?: (response: TReturn, values: TParams) => void
  onError?: (error: unknown, values: TParams) => void
  onFinally?: (values: TParams) => void
}

export interface UseFormReturn<TParams extends object, TReturn> {
  form: Ref<TParams>
  isValid: Ref<boolean>
  reset: () => void
  submit: (overrideValues?: Partial<TParams>) => Promise<TReturn>
  /** Ant Design Vue Form instance, if useAntd is true */
  antdForm?: FormInstance
  /** Pass these rules into <a-form-model> */
  rules?: Ref<Record<keyof TParams, FormRule[]>>
}

export function useForm<TParams extends object, TReturn = unknown>(
  options: UseFormOptions<TParams, TReturn>,
): UseFormReturn<TParams, TReturn> {
  const {
    initialValues,
    action,
    validate,
    transform,
    debounceMs,
    rules,
    useAntd,
    onBefore,
    onSuccess,
    onError,
    onFinally,
  } = options

  // Reactive form state
  const formParams = reactive(cloneDeep(initialValues)) as TParams
  const form = ref(formParams)
  const reactiveRules = rules
    ? ref(reactive(rules) as Record<keyof TParams, FormRule[]>)
    : undefined
  let merged = cloneDeep(initialValues)

  // Optional Ant Design Vue Form instance
  let antdForm: FormInstance | undefined
  if (useAntd) {
    const formInstance = Form.useForm(form, reactiveRules)
    antdForm = formInstance
  }

  const isValid = computedAsync(() => (validate ? validate(form.value) : true), true)

  const reset = () => {
    const fresh = cloneDeep(initialValues)
    form.value = assign(form.value, fresh)
    merged = fresh
    if (useAntd && antdForm) {
      antdForm.resetFields()
      antdForm.clearValidate()
    }
  }

  // Debounced state updates
  const updateForm = debounce((values: Partial<TParams>) => {
    merged = merge({}, merged, values) as TParams
    form.value = assign(form.value, merged)
    // if (useAntd && antdForm) {
    //   antdForm.setFieldsValue(merged)
    // }
  }, debounceMs ?? 0)

  const submit = async (overrideValues?: Partial<TParams>): Promise<TReturn> => {
    debugger
    if (overrideValues) {
      if (debounceMs) updateForm(overrideValues)
      else {
        merged = merge({}, merged, overrideValues) as TParams
        form.value = assign(form.value, merged)
      }
      // if (useAntd && antdForm) {
      //   antdForm.setFieldsValue(merged)
      // }
    }

    if (!isValid.value) {
      return Promise.reject(new Error('Validation failed')) as Promise<TReturn>
    }

    try {
      onBefore?.(form.value)
      const params = transform
        ? await transform(debounceMs ? merged : form.value)
        : debounceMs
          ? merged
          : form.value
      if (useAntd && antdForm) {
        // Run Ant Design Vue validation
        await antdForm.validate()
      }
      const response = await action(params)
      onSuccess?.(response, form.value)

      return response
    } catch (error) {
      onError?.(error, form.value)
      throw error
    } finally {
      onFinally?.(form.value)
    }
  }

  return {
    form: form as Ref<TParams>,
    isValid,
    reset,
    submit,
    antdForm,
    rules: reactiveRules as Ref<Record<keyof TParams, FormRule[]>>,
  }
}
