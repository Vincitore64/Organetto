import { createCrudHooks } from '@/application/shared/hooks/useCrud'
import type { CardsClient } from '@/dataAccess/cards/services/CardsClient'
import type { CardVm } from '../models'
import type { CardDetailDto, CardDetailPayload, CardDto, CreateCardPayload, DeleteCardPayload, UpdateCardPayload } from '@/dataAccess/cards/models'
import { container } from 'tsyringe'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { mapCard, mapCardDetail, mapCards } from '../mappers'
import type { CardDetailVm } from '../models/Card'

const { useList: useGetCards, useDetail: useGetCardDetail, useCreate: useCreateCard, useUpdate: useUpdateCard, useRemove: useRemoveCard } = createCrudHooks<

  CardsClient,
  [number],
  CardDto,
  CardVm,
  CardDetailDto,
  CardDetailVm,
  CardDetailPayload,
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
    detail: 'getById',
    list: 'getAll',
    update: 'update',
    remove: 'delete',
  },
  mappers: {
    list: mapCards,
    detail: mapCardDetail,
    created: mapCard,
  },
})

export { useGetCards, useCreateCard, useUpdateCard, useRemoveCard, useGetCardDetail }
