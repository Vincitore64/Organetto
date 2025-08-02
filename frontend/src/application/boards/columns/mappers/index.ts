import { createArrayMapper, createMapper } from '@/application/shared/mappers'
import type { ColumnDto } from '@/dataAccess/columns/models'
import type { ColumnVm } from '../models'
import { mapCards } from '../cards'

const mapColumn = createMapper<ColumnDto, ColumnVm>({
  id: 'id',
  title: 'title',
  position: 'position',
  cards: col => mapCards(col.cards),
  collapsed: () => false,
})

const mapColumns = createArrayMapper(mapColumn)

export { mapColumn, mapColumns }
