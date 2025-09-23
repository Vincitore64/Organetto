import { createCrudHooks } from '@/application/shared/hooks/useCrud'
import type { BoardDetailedDto, BoardDto } from '@/dataAccess/boards/models'
import type { BoardsClient } from '@/dataAccess/boards/services/BoardsClient'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { container } from 'tsyringe'
import { mapBoard, mapBoardList } from '../mappers'
import type { BoardListItemVm, BoardVm } from '../models'

const { useDetail: useBoardDetail } = createCrudHooks<BoardsClient, [], BoardDto, BoardListItemVm, BoardDetailedDto, BoardVm>({
  resourceKey: 'boards',
  client: () => container.resolve(ApiClient).boards,
  methods: {
    list: 'getAll',
    detail: 'getById',
    create: 'create',
    update: 'update',
    remove: 'delete',
  },
  mappers: {
    list: mapBoardList,
    detail: mapBoard,
  }
})

export { useBoardDetail }