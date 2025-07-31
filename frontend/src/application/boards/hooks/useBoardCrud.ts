import { createCrudHooks } from '@/application/shared/hooks/useCrud'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { container } from 'tsyringe'
import { ref } from 'vue'

const { useDetail: useBoardDetail } = createCrudHooks({
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