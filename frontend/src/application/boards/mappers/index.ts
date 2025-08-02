import { createArrayMapper, createMapper } from '@/application/shared/mappers'
import type { BoardDetailedDto, BoardDto } from '@/dataAccess/boards/models'
import type { BoardListItemVm, BoardVm } from '../models'
import { mapColumns } from '../columns'
import { mapBoardMembers } from '../members'

const mapBoardListItem = createMapper<BoardDto, BoardListItemVm>({
  id: 'id',
  title: src => src.title ?? 'Untitled board',
  description: src => src.description ?? '',
  isArchived: 'isArchived',
  createdAt: b => new Date(b.createdAt),
})

const mapBoardList = createArrayMapper(mapBoardListItem)

const mapBoard = createMapper<BoardDetailedDto, BoardVm>({
  id: 'id',
  title: src => src.title ?? 'Untitled board',
  description: 'description',
  columns: b => mapColumns(b.columns),
  members: b => mapBoardMembers(b.members),
  isUpdating: () => false
})

export { mapBoardList, mapBoardListItem, mapBoard }
