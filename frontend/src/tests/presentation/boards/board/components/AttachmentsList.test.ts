import { mount } from '@vue/test-utils'
import { describe, it, expect, vi } from 'vitest'
import AttachmentsList from '@/presentation/boards/board/components/AttachmentsList.vue'

const items = [
  { id: 1, filename: 'report.pdf', fileUrl: '/f/1', uploadedAt: new Date(1721000000000), uploaderId: 7 },
  { id: 2, filename: 'photo.jpg', fileUrl: '/f/2', uploadedAt: new Date(1721001000000), uploaderId: 7, sizeBytes: 123456 }
]

describe('AttachmentsList', () => {
  it('renders list items', () => {
    const w = mount(AttachmentsList, { props: { items } })
    const rows = w.findAll('.attachments__item')
    expect(rows.length).toBe(2)
    expect(rows[0].text()).toContain('report.pdf')
  })

  it('emits item-click on click', async () => {
    const w = mount(AttachmentsList, { props: { items } })
    await w.findAll('.attachments__item')[0].trigger('click')
    expect(w.emitted<any>('item-click')?.[0]?.[0]?.id).toBe(1)
  })

  it('icon category mapping (pdf, image)', () => {
    const w = mount(AttachmentsList, { props: { items } })
    const svgs = w.findAll('.attachments__icon-svg')
    expect(svgs.length).toBe(2)
    // Smoke: both icons rendered
    expect(svgs[0].exists()).toBe(true)
    expect(svgs[1].exists()).toBe(true)
  })

  it('a11y roles & labels present', () => {
    const w = mount(AttachmentsList, { props: { items, ariaLabel: 'Attachments' } })
    const root = w.find('[role="list"]')
    expect(root.exists()).toBe(true)
    expect(root.attributes('aria-label')).toBe('Attachments')
  })

  it('selection emits selection-change', async () => {
    const w = mount(AttachmentsList, { props: { items, selectable: true } })
    await w.findAll('.attachments__item')[1].trigger('click')
    expect((w.emitted('selection-change')![0][0] as number[])).toEqual([2])
  })
})
