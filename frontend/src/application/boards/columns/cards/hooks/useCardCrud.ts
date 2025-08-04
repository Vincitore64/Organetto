import { createCrudHooks } from '@/application/shared/hooks/useCrud'
import type { CardsClient } from '@/dataAccess/cards/services/CardsClient'
import type { CardVm } from '../models'
import type { CardDto, CreateCardPayload, DeleteCardPayload, UpdateCardPayload } from '@/dataAccess/cards/models'
import { container } from 'tsyringe'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { mapCard, mapCards } from '../mappers'

const { useList: useGetCards, useCreate: useCreateCard, useUpdate: useUpdateCard, useRemove: useRemoveCard } = createCrudHooks<
  CardsClient,
  [number],
  CardDto,
  CardVm,
  CardDto,
  CardVm,
  number,
  CreateCardPayload,
  UpdateCardPayload,
  DeleteCardPayload,
  CardDto,
  CardVm
>({
  resourceKey: 'cards',
  client: () => container.resolve(ApiClient).cards,
  methods: {
    create: 'create',
    detail: 'getAll',
    list: 'getAll',
    update: 'update',
    remove: 'delete',
  },
  mappers: {
    list: mapCards,
    detail: mapCard,
    created: mapCard,
  },
})

export { useGetCards, useCreateCard, useUpdateCard, useRemoveCard }
