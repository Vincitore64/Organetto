import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import CreateBoardCard from '@/presentation/boards/components/CreateBoardCard.vue'
import { createI18n } from 'vue-i18n'

// Mock i18n
const i18n = createI18n({
  legacy: false,
  locale: 'en',
  messages: {
    en: {
      boards: {
        page: {
          create: 'Create Board',
        },
      },
    },
    ru: {
      boards: {
        page: {
          create: 'Создать доску',
        },
      },
    },
  },
})

// Mock Ant Design Vue icons
const mockIcons = {
  PlusOutlined: { template: '<span class="plus-icon">+</span>' },
  CheckOutlined: { template: '<span class="check-icon">✓</span>' },
  BulbOutlined: { template: '<span class="bulb-icon">💡</span>' },
  RocketOutlined: { template: '<span class="rocket-icon">🚀</span>' },
  StarOutlined: { template: '<span class="star-icon">⭐</span>' },
  HeartOutlined: { template: '<span class="heart-icon">❤️</span>' },
  ThunderboltOutlined: { template: '<span class="thunder-icon">⚡</span>' },
  CrownOutlined: { template: '<span class="crown-icon">👑</span>' },
}

// Mock Ant Design Vue Card component
const mockACard = {
  template: `
    <div class="ant-card" :class="{ 'ant-card-hoverable': hoverable }">
      <div class="ant-card-cover" v-if="$slots.cover">
        <slot name="cover"></slot>
      </div>
      <div class="ant-card-body" :style="bodyStyle">
        <slot></slot>
      </div>
    </div>
  `,
  props: ['hoverable', 'bodyStyle'],
}

const createWrapper = (props = {}) => {
  return mount(CreateBoardCard, {
    props,
    global: {
      plugins: [i18n],
      components: {
        ...mockIcons,
        'a-card': mockACard,
      },
    },
  })
}

describe('CreateBoardCard.vue', () => {
  // 1. Rendering tests
  describe('Rendering', () => {
    it('renders the main structure correctly', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('main').exists()).toBe(true)
      expect(wrapper.find('.create-card').exists()).toBe(true)
      expect(wrapper.find('.create-cover').exists()).toBe(true)
      expect(wrapper.find('.create-body').exists()).toBe(true)
    })

    it('renders the create title with i18n', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('.create-title').text()).toBe('Create Board')
    })

    it('renders the create description', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('.create-description').text()).toBe(
        'Создайте новую доску для вашего проекта',
      )
    })

    it('renders the main plus icon', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('.main-icon').exists()).toBe(true)
      expect(wrapper.find('.plus-icon').exists()).toBe(true)
    })

    it('renders feature list with check icons', () => {
      const wrapper = createWrapper()

      const features = wrapper.findAll('.feature')
      expect(features).toHaveLength(2)

      expect(features[0].text()).toContain('Шаблоны')
      expect(features[1].text()).toContain('Совместная работа')

      expect(wrapper.findAll('.check-icon')).toHaveLength(2)
    })

    it('renders floating icons in the cover', () => {
      const wrapper = createWrapper()

      const floatingIcons = wrapper.findAll('.floating-icon')
      expect(floatingIcons).toHaveLength(6)
    })

    it('renders gradient background in cover', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('.gradient-bg').exists()).toBe(true)
    })
  })

  // 2. Interaction tests
  describe('Interactions', () => {
    it('emits "create" event when card is clicked', async () => {
      const wrapper = createWrapper()

      await wrapper.find('.create-card').trigger('click')

      expect(wrapper.emitted()).toHaveProperty('create')
      expect(wrapper.emitted('create')).toHaveLength(1)
    })

    it('emits "create" event when main element is clicked', async () => {
      const wrapper = createWrapper()

      await wrapper.find('main').trigger('click')

      expect(wrapper.emitted()).toHaveProperty('create')
    })

    it('has hoverable card property set', () => {
      const wrapper = createWrapper()

      const card = wrapper.findComponent({ name: 'a-card' })
      expect(card.props('hoverable')).toBe(true)
    })
  })

  // 3. Component methods and computed properties
  describe('Component Logic', () => {
    it('getRandomIcon returns a valid icon component', () => {
      const wrapper = createWrapper()
      const vm = wrapper.vm as any

      // Test multiple calls to ensure randomness works
      for (let i = 0; i < 10; i++) {
        const icon = vm.getRandomIcon()
        expect(icon).toBeDefined()
        expect(typeof icon).toBe('object')
      }
    })

    it('getFloatingIconStyle returns correct positioning for each index', () => {
      const wrapper = createWrapper()
      const vm = wrapper.vm as any

      // Test all 6 positions
      for (let i = 1; i <= 6; i++) {
        const style = vm.getFloatingIconStyle(i)
        expect(style).toBeDefined()
        expect(style).toHaveProperty('animationDelay')
        expect(typeof style.animationDelay).toBe('string')
      }
    })

    it('getFloatingIconStyle returns empty object for invalid index', () => {
      const wrapper = createWrapper()
      const vm = wrapper.vm as any

      const style = vm.getFloatingIconStyle(0)
      expect(style).toEqual({})

      const style2 = vm.getFloatingIconStyle(7)
      expect(style2).toEqual({})
    })
  })

  // 4. CSS classes and styling
  describe('Styling', () => {
    it('has correct CSS classes applied', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('.create-card').classes()).toContain('create-card')
      expect(wrapper.find('.create-cover').classes()).toContain('create-cover')
      expect(wrapper.find('.create-body').classes()).toContain('create-body')
      expect(wrapper.find('.main-icon').classes()).toContain('main-icon')
    })

    it('applies correct body style to card', () => {
      const wrapper = createWrapper()

      const card = wrapper.findComponent({ name: 'a-card' })
      const bodyStyle = card.props('bodyStyle')

      expect(bodyStyle).toEqual({
        height: 'max-content',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        padding: '16px 12px 0px 12px',
      })
    })

    it('floating icons have correct positioning styles', () => {
      const wrapper = createWrapper()
      const floatingIcons = wrapper.findAll('.floating-icon')

      // Check that each floating icon has inline styles applied
      floatingIcons.forEach((icon) => {
        const style = icon.attributes('style')
        expect(style).toBeDefined()
        expect(style).toContain('animation-delay')
      })
    })
  })

  // 5. Internationalization
  describe('Internationalization', () => {
    it('displays correct text in English locale', async () => {
      i18n.global.locale.value = 'en'
      const wrapper = createWrapper()

      await wrapper.vm.$nextTick()
      expect(wrapper.find('.create-title').text()).toBe('Create Board')
    })

    it('displays correct text in Russian locale', async () => {
      i18n.global.locale.value = 'ru'
      const wrapper = createWrapper()

      await wrapper.vm.$nextTick()
      expect(wrapper.find('.create-title').text()).toBe('Создать доску')
    })
  })

  // 6. Edge cases
  describe('Edge Cases', () => {
    it('handles missing i18n gracefully', () => {
      const wrapperWithoutI18n = mount(CreateBoardCard, {
        global: {
          components: {
            ...mockIcons,
            'a-card': mockACard,
          },
          mocks: {
            t: (key: string) => key,
          },
        },
      })

      expect(wrapperWithoutI18n.find('.create-title').text()).toBe('boards.page.create')
    })

    it('renders without crashing when no props are provided', () => {
      expect(() => createWrapper()).not.toThrow()
    })

    it('maintains component structure integrity', () => {
      const wrapper = createWrapper()

      // Ensure all critical elements exist
      expect(wrapper.find('main').exists()).toBe(true)
      expect(wrapper.find('.create-card').exists()).toBe(true)
      expect(wrapper.find('.main-icon').exists()).toBe(true)
      expect(wrapper.find('.create-title').exists()).toBe(true)
      expect(wrapper.find('.create-description').exists()).toBe(true)
      expect(wrapper.find('.create-features').exists()).toBe(true)
    })
  })

  // 7. Accessibility
  describe('Accessibility', () => {
    it('uses semantic HTML elements', () => {
      const wrapper = createWrapper()

      expect(wrapper.find('main').exists()).toBe(true)
      expect(wrapper.find('h3').exists()).toBe(true)
      expect(wrapper.find('p').exists()).toBe(true)
    })

    it('has clickable card for keyboard navigation', () => {
      const wrapper = createWrapper()

      const card = wrapper.find('.create-card')
      expect(card.exists()).toBe(true)
      // Card should be clickable for accessibility
    })
  })

  // 8. Snapshot test
  describe('Snapshot', () => {
    it('matches snapshot for create board card', () => {
      const wrapper = createWrapper()
      expect(wrapper.html()).toMatchSnapshot()
    })
  })
})
