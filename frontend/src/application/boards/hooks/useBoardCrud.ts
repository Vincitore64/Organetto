import { createCrudHooks } from '@/application/shared/hooks/useCrud'
import type { BoardDetailedDto, BoardDto } from '@/dataAccess/boards/models'
import type { BoardsClient } from '@/dataAccess/boards/services/BoardsClient'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { container } from 'tsyringe'

const { useDetail: useBoardDetail } = createCrudHooks<BoardsClient, [], BoardDto, BoardDto, BoardDetailedDto, BoardDetailedDto>({
  resourceKey: 'boards',
  client: () => container.resolve(ApiClient).boards,
  methods: {
    list: 'getAll',
    detail: 'getById',
    create: 'create',
    update: 'update',
    remove: 'delete',
  },
})

export { useBoardDetail }