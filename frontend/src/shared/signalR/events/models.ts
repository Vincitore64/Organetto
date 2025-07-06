export enum CrudEventType {
  Created = 'Created',
  Updated = 'Updated',
  Deleted = 'Deleted',
}

export interface SignalREvent {
  type: string
  // payload: T;
}

export interface CrudEvent extends SignalREvent {
  entity: string
  crudType: CrudEventType
}
