# AttachmentsList

A reusable Vue 3 + TS component for rendering a list/grid of attachments with proper icons, keyboard navigation, and robust states.

## Props
| Name | Type | Default | Description |
|---|---|---|---|
| items | AttachmentVm[] | [] | Data source |
| layout | 'list' \| 'grid' | 'list' | Rendering mode |
| showSize | boolean | false | Show formatted size |
| showActions | boolean | true | Show action buttons |
| iconMap | Record<string, Component> | {} | Override icons by category or `ext:<ext>` |
| truncate | number | 28 | Trim long names |
| loading | boolean | false | Loading skeletons |
| error | string\|null | null | Error banner |
| emptyState | string | 'No attachments yet' | Message for empty lists |
| selectable | boolean | false | Enables selection |
| disabled | boolean | false | Greys out & blocks interactions |
| ariaLabel | string | 'Attachments' | Wrapper aria-label |
| download | boolean | true | Anchor `download` attribute |
| openInNewTab | boolean | true | Safe new tab |
| virtualized | boolean | false | List windowing (fixed row height) |

## Events
- `item-click(item)`
- `download(item)`
- `remove(item)`
- `selection-change(selectedIds: number[])`
- `retry()`

## Slots
- `icon` (item), `item` (item), `actions` (item), `empty`, `error`
