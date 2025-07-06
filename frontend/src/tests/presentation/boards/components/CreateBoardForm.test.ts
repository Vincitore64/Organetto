import { describe, it, expect, vi } from 'vitest'
import { mount } from '@vue/test-utils'
import CreateBoardForm from '@/presentation/boards/components/CreateBoardForm.vue'
import { createI18n } from 'vue-i18n'
import type { CreateBoardState } from '@/application'
import { Form } from 'ant-design-vue'

// Mock i18n
const i18n = createI18n({
  legacy: false,
  locale: 'en',
  messages: {
    en: {
      boards: {
        createForm: {
          nameLabel: 'Board Name',
          namePlaceholder: 'Enter board name',
          descriptionLabel: 'Description',
          descriptionPlaceholder: 'Enter board description',
          submit: 'Create Board',
        },
      },
    },
    ru: {
      boards: {
        createForm: {
          nameLabel: 'Название доски',
          namePlaceholder: 'Введите название доски',
          descriptionLabel: 'Описание',
          descriptionPlaceholder: 'Введите описание доски',
          submit: 'Создать доску',
        },
      },
    },
  },
})

// Mock Ant Design Vue components
const mockAForm = {
  template: `
    <form class="ant-form ant-form-vertical">
      <slot></slot>
    </form>
  `,
  props: ['layout'],
}

const mockAFormItem = {
  template: `
    <div class="ant-form-item" :class="{ 'ant-form-item-has-error': validateStatus === 'error' }">
      <label class="ant-form-item-label" v-if="label">{{ label }}</label>
      <div class="ant-form-item-control">
        <slot></slot>
        <div class="ant-form-item-explain" v-if="help">{{ help }}</div>
      </div>
    </div>
  `,
  props: ['label', 'name', 'validateStatus', 'help'],
}

const mockAInput = {
  template: `<input class="ant-input" :value="value" :placeholder="placeholder" @input="$emit('update:value', $event.target.value)" />`,
  props: ['value', 'placeholder'],
  emits: ['update:value'],
}

const mockATextarea = {
  template: `<textarea class="ant-input" :value="value" :placeholder="placeholder" :rows="rows" @input="$emit('update:value', $event.target.value)"></textarea>`,
  props: ['value', 'placeholder', 'rows'],
  emits: ['update:value'],
}

const mockAButton = {
  template: `
    <button class="ant-btn" :class="{ 'ant-btn-primary': type === 'primary', 'ant-btn-loading': loading, 'ant-btn-block': block }" @click="$emit('click')">
      <slot></slot>
    </button>
  `,
  props: ['type', 'loading', 'block'],
  emits: ['click'],
}

// Mock FormInstance
// const createMockFormInstance = () => ({
//   validateInfos: {
//     name: {
//       validateStatus: '',
//       help: '',
//     },
//     description: {
//       validateStatus: '',
//       help: '',
//     },
//   },
//   rulesRef: {},
// })
const createMockFormInstance = () => Form.useForm({})

// Mock CreateBoardState
const createMockBoardState = (): CreateBoardState => ({
  name: '',
  description: '',
})

const createWrapper = (props = {}) => {
  const defaultProps = {
    modelValue: createMockBoardState(),
    formInstance: createMockFormInstance(),
    loading: false,
    ...props,
  }

  return mount(CreateBoardForm, {
    props: defaultProps,
    global: {
      plugins: [i18n],
      components: {
        'a-form': mockAForm,
        'a-form-item': mockAFormItem,
        'a-input': mockAInput,
        'a-textarea': mockATextarea,
        'a-button': mockAButton,
      },
    },
  })
}

describe('CreateBoardForm.vue', () => {
  // 1. Rendering tests
  describe('Rendering', () => {
    it('renders the form structure correctly', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('.ant-form').exists()).toBe(true)
      expect(wrapper.find('.ant-form-vertical').exists()).toBe(true)
      expect(wrapper.findAll('.ant-form-item')).toHaveLength(3)
    })

    it('renders name input field with correct props', () => {
      const wrapper = createWrapper()

      const nameFormItem = wrapper.findAll('.ant-form-item')[0]
      const nameInput = nameFormItem.find('.ant-input')

      expect(nameFormItem.find('label').text()).toBe('Board Name')
      expect(nameInput.attributes('placeholder')).toBe('Enter board name')
    })

    it('renders description textarea field with correct props', () => {
      const wrapper = createWrapper()

      const descriptionFormItem = wrapper.findAll('.ant-form-item')[1]
      const descriptionTextarea = descriptionFormItem.find('.ant-input')

      expect(descriptionFormItem.find('label').text()).toBe('Description')
      expect(descriptionTextarea.attributes('placeholder')).toBe('Enter board description')
      expect(descriptionTextarea.attributes('rows')).toBe('4')
    })

    it('renders submit button with correct text and properties', () => {
      const wrapper = createWrapper()

      const submitButton = wrapper.find('.ant-btn')
      expect(submitButton.text()).toBe('Create Board')
      expect(submitButton.classes()).toContain('ant-btn-primary')
      expect(submitButton.classes()).toContain('ant-btn-block')
    })

    it('shows loading state on submit button when loading prop is true', () => {
      const wrapper = createWrapper({ loading: true })

      const submitButton = wrapper.find('.ant-btn')
      expect(submitButton.classes()).toContain('ant-btn-loading')
    })
  })

  // 2. Props and v-model tests
  describe('Props and V-Model', () => {
    it('displays initial values from modelValue prop', () => {
      const initialState: CreateBoardState = {
        name: 'Test Board',
        description: 'Test Description',
      }
      const wrapper = createWrapper({ modelValue: initialState })

      const nameInput = wrapper.findAll('.ant-form-item')[0].find('.ant-input')
      const descriptionTextarea = wrapper.findAll('.ant-form-item')[1].find('.ant-input')

      expect(nameInput.element.value).toBe('Test Board')
      expect(descriptionTextarea.element.value).toBe('Test Description')
    })

    it('emits update:modelValue when name input changes', async () => {
      const wrapper = createWrapper()
      const nameInput = wrapper.findAll('.ant-form-item')[0].find('.ant-input')

      await nameInput.setValue('New Board Name')

      expect(wrapper.emitted('update:modelValue')).toBeTruthy()
      const emittedValue = wrapper.emitted('update:modelValue')?.[0]?.[0] as CreateBoardState
      expect(emittedValue.name).toBe('New Board Name')
    })

    it('emits update:modelValue when description textarea changes', async () => {
      const wrapper = createWrapper()
      const descriptionTextarea = wrapper.findAll('.ant-form-item')[1].find('.ant-input')

      await descriptionTextarea.setValue('New Description')

      expect(wrapper.emitted('update:modelValue')).toBeTruthy()
      const emittedValue = wrapper.emitted('update:modelValue')?.[0]?.[0] as CreateBoardState
      expect(emittedValue.description).toBe('New Description')
    })

    it('preserves other fields when updating one field', async () => {
      const initialState: CreateBoardState = {
        name: 'Initial Name',
        description: 'Initial Description',
      }
      const wrapper = createWrapper({ modelValue: initialState })
      const nameInput = wrapper.findAll('.ant-form-item')[0].find('.ant-input')

      await nameInput.setValue('Updated Name')

      const emittedValue = wrapper.emitted('update:modelValue')?.[0]?.[0] as CreateBoardState
      expect(emittedValue.name).toBe('Updated Name')
      expect(emittedValue.description).toBe('Initial Description')
    })
  })

  // 3. Form submission tests
  describe('Form Submission', () => {
    it('emits submit event when submit button is clicked', async () => {
      const wrapper = createWrapper()
      const submitButton = wrapper.find('.ant-btn')

      await submitButton.trigger('click')

      expect(wrapper.emitted('submit')).toBeTruthy()
      expect(wrapper.emitted('submit')).toHaveLength(1)
    })

    it('emits submit event with current form values', async () => {
      const initialState: CreateBoardState = {
        name: 'Test Board',
        description: 'Test Description',
      }
      const wrapper = createWrapper({ modelValue: initialState })
      const submitButton = wrapper.find('.ant-btn')

      await submitButton.trigger('click')

      const emittedValue = wrapper.emitted('submit')?.[0]?.[0] as CreateBoardState
      expect(emittedValue).toEqual(initialState)
    })

    it('prevents default form submission behavior', async () => {
      const wrapper = createWrapper()
      const submitButton = wrapper.find('.ant-btn')
      const clickEvent = new Event('click')
      const preventDefaultSpy = vi.spyOn(clickEvent, 'preventDefault')

      await submitButton.element.dispatchEvent(clickEvent)

      expect(preventDefaultSpy).toHaveBeenCalled()
    })
  })

  // 4. Form validation integration
  describe('Form Validation Integration', () => {
    it('applies validation info to name field', () => {
      const formInstance = createMockFormInstance()
      formInstance.validateInfos.name = {
        validateStatus: 'error',
        help: 'Name is required',
      }
      const wrapper = createWrapper({ formInstance })

      const nameFormItem = wrapper.findAll('.ant-form-item')[0]
      expect(nameFormItem.classes()).toContain('ant-form-item-has-error')
    })

    it('applies validation info to description field', () => {
      const formInstance = createMockFormInstance()
      formInstance.validateInfos.description = {
        validateStatus: 'error',
        help: 'Description is too long',
      }
      const wrapper = createWrapper({ formInstance })

      const descriptionFormItem = wrapper.findAll('.ant-form-item')[1]
      expect(descriptionFormItem.classes()).toContain('ant-form-item-has-error')
    })
  })

  // 5. Internationalization tests
  describe('Internationalization', () => {
    it('displays correct labels and placeholders in English', () => {
      i18n.global.locale.value = 'en'
      const wrapper = createWrapper()

      const formItems = wrapper.findAll('.ant-form-item')
      expect(formItems[0].find('label').text()).toBe('Board Name')
      expect(formItems[1].find('label').text()).toBe('Description')
      expect(wrapper.find('.ant-btn').text()).toBe('Create Board')
    })

    it('displays correct labels and placeholders in Russian', async () => {
      i18n.global.locale.value = 'ru'
      const wrapper = createWrapper()
      await wrapper.vm.$nextTick()

      const formItems = wrapper.findAll('.ant-form-item')
      expect(formItems[0].find('label').text()).toBe('Название доски')
      expect(formItems[1].find('label').text()).toBe('Описание')
      expect(wrapper.find('.ant-btn').text()).toBe('Создать доску')
    })
  })

  // 6. Edge cases and error handling
  describe('Edge Cases', () => {
    it('handles empty modelValue gracefully', () => {
      const wrapper = createWrapper({ modelValue: { name: '', description: '' } })

      expect(() => wrapper.vm).not.toThrow()
      expect(wrapper.find('.ant-form').exists()).toBe(true)
    })

    it('handles missing formInstance properties gracefully', () => {
      const incompleteFormInstance = {
        validateInfos: {
          name: {},
          description: {},
        },
      }
      const wrapper = createWrapper({ formInstance: incompleteFormInstance })

      expect(() => wrapper.vm).not.toThrow()
      expect(wrapper.find('.ant-form').exists()).toBe(true)
    })

    it('handles undefined loading prop', () => {
      const wrapper = createWrapper({ loading: undefined })
      const submitButton = wrapper.find('.ant-btn')

      expect(submitButton.classes()).not.toContain('ant-btn-loading')
    })

    it('maintains component structure with minimal props', () => {
      const minimalProps = {
        modelValue: { name: '', description: '' },
        formInstance: { validateInfos: { name: {}, description: {} } },
      }
      const wrapper = createWrapper(minimalProps)

      expect(wrapper.find('.ant-form').exists()).toBe(true)
      expect(wrapper.findAll('.ant-form-item')).toHaveLength(3)
      expect(wrapper.find('.ant-btn').exists()).toBe(true)
    })
  })

  // 7. Component behavior tests
  describe('Component Behavior', () => {
    it('maintains reactive updates between parent and child', async () => {
      const wrapper = createWrapper()
      const nameInput = wrapper.findAll('.ant-form-item')[0].find('.ant-input')

      // Simulate typing in input
      await nameInput.setValue('Dynamic Update')

      // Check that the component emitted the update
      expect(wrapper.emitted('update:modelValue')).toBeTruthy()
      const lastEmittedValue = wrapper
        .emitted('update:modelValue')
        ?.slice(-1)[0]?.[0] as CreateBoardState
      expect(lastEmittedValue.name).toBe('Dynamic Update')
    })

    it('handles rapid input changes correctly', async () => {
      const wrapper = createWrapper()
      const nameInput = wrapper.findAll('.ant-form-item')[0].find('.ant-input')

      // Simulate rapid typing
      await nameInput.setValue('A')
      await nameInput.setValue('AB')
      await nameInput.setValue('ABC')

      expect(wrapper.emitted('update:modelValue')).toHaveLength(3)
      const lastEmittedValue = wrapper
        .emitted('update:modelValue')
        ?.slice(-1)[0]?.[0] as CreateBoardState
      expect(lastEmittedValue.name).toBe('ABC')
    })
  })

  // 8. Snapshot test
  describe('Snapshot', () => {
    it('matches snapshot for create board form', () => {
      const wrapper = createWrapper()
      expect(wrapper.html()).toMatchSnapshot()
    })

    it('matches snapshot with loading state', () => {
      const wrapper = createWrapper({ loading: true })
      expect(wrapper.html()).toMatchSnapshot()
    })

    it('matches snapshot with validation errors', () => {
      const formInstance = createMockFormInstance()
      formInstance.validateInfos.name = {
        validateStatus: 'error',
        help: 'Name is required',
      }
      const wrapper = createWrapper({ formInstance })
      expect(wrapper.html()).toMatchSnapshot()
    })
  })
})
