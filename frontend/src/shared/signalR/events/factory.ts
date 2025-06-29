import { type CrudEvent, CrudEventType, type SignalREvent } from './models'

export function createCrudEvent(entity: string, crudType: CrudEventType): CrudEvent {
  return {
    type: `${entity}${crudType}`,
    entity,
    crudType,
  }
}

export function createSignalREvent(type: string): SignalREvent {
  return {
    type,
  }
}
