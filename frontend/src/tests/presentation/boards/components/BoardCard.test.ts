import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import BoardCard from '@/presentation/boards/components/BoardCard.vue'
import type { BoardPageView } from '@/presentation/boards/models'
// import { defaultThumbnailHref } from '../models'

// Mock Ant Design Vue icons
const mockIcons = {
  EyeOutlined: { template: '<span class="eye-icon">👁</span>' },
  ClockCircleOutlined: { template: '<span class="clock-icon">🕐</span>' },
  UserOutlined: { template: '<span class="user-icon">👤</span>' },
}

const createMockBoard = (overrides: Partial<BoardPageView> = {}): BoardPageView => ({
  id: 1,
  title: 'Test Board',
  thumbnailUrl: '/test-image.png',
  ...overrides,
})

describe('BoardCard.vue', () => {
  // 1. Rendering tests
  describe('Rendering', () => {
    it('renders the board title', () => {
      const board = createMockBoard({ title: 'My Board' })
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })
      expect(wrapper.text()).toContain('My Board')
    })

    it('renders the board thumbnail with correct src', () => {
      const board = createMockBoard({ thumbnailUrl: '/custom-image.png' })
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })
      const img = wrapper.find('.board-thumbnail')
      expect(img.attributes('src')).toBe('/custom-image.png')
    })

    it('renders the board thumbnail with correct alt text', () => {
      const board = createMockBoard({ title: 'My Board Title' })
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })
      const img = wrapper.find('.board-thumbnail')
      expect(img.attributes('alt')).toBe('My Board Title')
    })

    it('renders the overlay with view text', () => {
      const board = createMockBoard()
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })
      expect(wrapper.find('.overlay').exists()).toBe(true)
      expect(wrapper.find('.view-text').text()).toBe('Открыть доску')
    })

    it('renders meta information', () => {
      const board = createMockBoard()
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })
      expect(wrapper.find('.board-meta').exists()).toBe(true)
      expect(wrapper.text()).toContain('Недавно')
      expect(wrapper.text()).toContain('Личная')
    })
  })

  // 2. Interaction tests
  describe('Interactions', () => {
    it('emits "open" event with board id when card is clicked', async () => {
      const board = createMockBoard({ id: 42 })
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      await wrapper.find('.board-card').trigger('click')

      expect(wrapper.emitted()).toHaveProperty('open')
      expect(wrapper.emitted('open')![0]).toEqual([42])
    })

    it('shows overlay on hover (CSS behavior)', () => {
      const board = createMockBoard()
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      const overlay = wrapper.find('.overlay')
      expect(overlay.exists()).toBe(true)
      // Note: CSS hover effects can't be directly tested in unit tests
      // but we can verify the overlay element exists
    })
  })

  // 3. Props validation
  describe('Props', () => {
    it('handles board prop correctly', () => {
      const board = createMockBoard({
        id: 123,
        title: 'Custom Title',
        thumbnailUrl: '/custom.jpg',
      })

      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      expect(wrapper.find('.board-title').text()).toBe('Custom Title')
      expect(wrapper.find('.board-thumbnail').attributes('src')).toBe('/custom.jpg')
    })

    it('truncates long titles with CSS ellipsis', () => {
      const longTitle = 'This is a very long board title that should be truncated with ellipsis'
      const board = createMockBoard({ title: longTitle })

      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      const titleElement = wrapper.find('.board-title')
      expect(titleElement.text()).toBe(longTitle)
      // CSS truncation is handled by white-space: nowrap and text-overflow: ellipsis
      expect(titleElement.classes()).not.toContain('truncated')
    })
  })

  // 4. Edge cases
  describe('Edge Cases', () => {
    it('handles empty thumbnail URL gracefully', () => {
      const board = createMockBoard({ thumbnailUrl: '' })
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      const img = wrapper.find('.board-thumbnail')
      expect(img.attributes('src')).toBe('')
    })

    it('handles special characters in title', () => {
      const board = createMockBoard({ title: 'Board with <script>alert("xss")</script> & symbols' })
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      expect(wrapper.find('.board-title').text()).toContain('Board with')
      expect(wrapper.find('.board-title').text()).toContain('& symbols')
    })

    it('handles zero or negative board id', async () => {
      const board = createMockBoard({ id: -1 })
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      await wrapper.find('.board-card').trigger('click')
      expect(wrapper.emitted('open')![0]).toEqual([-1])
    })
  })

  // 5. Component structure
  describe('Component Structure', () => {
    it('has correct CSS classes', () => {
      const board = createMockBoard()
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      expect(wrapper.find('.board-card').exists()).toBe(true)
      expect(wrapper.find('.card-cover').exists()).toBe(true)
      expect(wrapper.find('.card-content').exists()).toBe(true)
      expect(wrapper.find('.board-title').exists()).toBe(true)
      expect(wrapper.find('.board-meta').exists()).toBe(true)
    })

    it('uses Ant Design Card component', () => {
      const board = createMockBoard()
      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: {
            ...mockIcons,
            'a-card': {
              template: '<div class="ant-card"><slot name="cover"></slot><slot></slot></div>',
              props: ['hoverable', 'bodyStyle'],
            },
          },
        },
      })

      expect(wrapper.find('.ant-card').exists()).toBe(true)
    })
  })

  // 6. Snapshot test
  describe('Snapshot', () => {
    it('matches snapshot for a normal board', () => {
      const board = createMockBoard({
        id: 1,
        title: 'Sample Board',
        thumbnailUrl: '/sample.png',
      })

      const wrapper = mount(BoardCard, {
        props: { board },
        global: {
          components: mockIcons,
        },
      })

      expect(wrapper.html()).toMatchSnapshot()
    })
  })
})
