import { EntityType } from '@/application/shared'
import { createCrudEvent } from '@/shared/signalR'
import { CrudEventType } from '@/shared/signalR'

export const boardCreated = createCrudEvent(EntityType.Board, CrudEventType.Created)
export const boardUpdated = createCrudEvent(EntityType.Board, CrudEventType.Updated)
export const boardDeleted = createCrudEvent(EntityType.Board, CrudEventType.Deleted)
