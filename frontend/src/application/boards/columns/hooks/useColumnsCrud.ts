import { createCrudHooks } from '@/application/shared/hooks/useCrud'
import type { ColumnsClient } from '@/dataAccess/columns/services/ColumnsClient'
import type {
  CreateColumnCommand,
  ColumnDto,
  UpdateColumnCommand,
  DeleteColumnCommand,
} from '@/dataAccess/columns/models'
import type { ColumnVm } from '../models'
import { container } from 'tsyringe'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { mapColumn, mapColumns } from '../mappers'

const { useCreate: useCreateColumn, useRemove: useRemoveColumn } = createCrudHooks<
  ColumnsClient,
  [number],
  ColumnDto,
  ColumnVm,
  ColumnDto,
  ColumnVm,
  number,
  CreateColumnCommand,
  UpdateColumnCommand,
  DeleteColumnCommand,
  ColumnDto,
  ColumnVm
>({
  resourceKey: 'columns',
  client: () => container.resolve(ApiClient).columns,
  methods: {
    create: 'create',
    detail: 'getAll',
    list: 'getAll',
    update: 'update',
    remove: 'delete',
  },
  mappers: {
    list: mapColumns,
    detail: mapColumn,
    created: mapColumn,
  },
})

export { useCreateColumn, useRemoveColumn }
