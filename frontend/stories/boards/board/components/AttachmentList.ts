// import type { Meta, StoryObj } from '@storybook/vue3'
// import AttachmentsList from '../../../../src/presentation/boards/board/components/AttachmentsList.vue'



// const meta: Meta<typeof AttachmentsList> = {
//   title: 'Organetto/AttachmentsList',
//   component: AttachmentsList,
//   args: {
//     items: [
//       { id: 1, filename: 'report.pdf', fileUrl: '#', uploadedAt: Date.now(), uploaderId: 1, sizeBytes: 321000 },
//       { id: 2, filename: 'photo.jpg', fileUrl: '#', uploadedAt: Date.now(), uploaderId: 1, sizeBytes: 123456 },
//       { id: 3, filename: 'archive.zip', fileUrl: '#', uploadedAt: Date.now(), uploaderId: 1 }
//     ]
//   }
// }
// export default meta
// type Story = StoryObj<typeof meta>

// export const ListDefault: Story = { args: { layout: 'list' } }
// export const Grid: Story = { args: { layout: 'grid' } }
// export const Loading: Story = { args: { loading: true, items: [] } }
// export const ErrorState: Story = { args: { error: 'Failed to load attachments', items: [] } }
// export const Empty: Story = { args: { items: [] } }
// export const Selectable: Story = { args: { selectable: true } }
// export const Virtualized: Story = {
//   args: {
//     layout: 'list',
//     virtualized: true,
//     items: Array.from({ length: 500 }, (_, i) => ({
//       id: i + 1, filename: `file-${i + 1}.txt`, fileUrl: '#', uploadedAt: Date.now(), uploaderId: 1
//     }))
//   }
// }
