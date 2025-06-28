import { ref, type Ref } from 'vue'
import { useSignalRClient } from '@/application/shared'
import { SignalRHubType } from '@/shared/env'

/** NEW ➊ — optional async loader */
type FetchItemById<T, K> = (id: K) => Promise<T>

/**
 * Опции для хука useRealtimeCollection
 */
export interface RealtimeCollectionOptions<T, K> {
  hubType: SignalRHubType
  getItemId: (item: T) => K
  events: { created?: string; updated?: string; deleted?: string; archived?: string }
  mapEventData?: (d: unknown) => T
  shouldAddItem?: (item: T) => boolean
  updateItem?: (oldI: T, newI: T) => T
  archiveField?: keyof T

  /** NEW ➊ — how to load item if Hub sends only an ID */
  fetchItem?: FetchItemById<T, K>
}

/**
 * Универсальный хук ...
 */
export function useRealtimeCollection<T, K = number | string>(
  initialItems: T[],
  options: RealtimeCollectionOptions<T, K>,
) {
  const items = ref<T[]>([...initialItems]) as Ref<T[]>

  const {
    connect: connectSignalR,
    disconnect: disconnectSignalR,
    on,
    isConnected,
    error,
  } = useSignalRClient(options.hubType)

  /** ───────────────── helpers ───────────────── */

  const addOrUpdate = (item: T) => {
    const itemId = options.getItemId(item)
    const idx = items.value.findIndex((i) => options.getItemId(i) === itemId)

    if (idx === -1) {
      // options.shouldAddItem?.(item) === false ? null : (items.value = [...items.value, item])
      if (options.shouldAddItem?.(item)) {
        items.value = [...items.value, item]
      }
    } else {
      const next = options.updateItem?.(items.value[idx], item) ?? item
      items.value.splice(idx, 1, next)
    }
  }

  /** NEW ➋ — resolve raw payload that could be id or object */
  const ensureItem = async (payload: unknown): Promise<T | null> => {
    // id?
    if (typeof payload === 'number' || typeof payload === 'string') {
      if (!options.fetchItem) return null
      try {
        return await options.fetchItem(payload as K)
      } catch (e) {
        console.error('[useRealtimeCollection] fetchItem failed', e)
        return null
      }
    }
    // Object with id
    if (options.getItemId(payload as T)) {
      if (!options.fetchItem) return null
      try {
        return await options.fetchItem(options.getItemId(payload as T))
      } catch (e) {
        console.error('[useRealtimeCollection] fetchItem failed', e)
        return null
      }
    }

    // already object
    return (options.mapEventData ? options.mapEventData(payload) : payload) as T
  }

  /** ───────────────── core API ───────────────── */

  const connect = async () => {
    await connectSignalR()

    if (options.events.created) {
      on(options.events.created, async (d: unknown) => {
        const item = await ensureItem(d)
        if (item) addOrUpdate(item)
      })
    }

    if (options.events.updated) {
      on(options.events.updated, async (d: unknown) => {
        const item = await ensureItem(d)
        if (item) addOrUpdate(item)
      })
    }

    if (options.events.deleted) {
      on(options.events.deleted, (d: unknown) => {
        const id =
          typeof d === 'number' || typeof d === 'string'
            ? (d as K)
            : options.getItemId(options.mapEventData ? options.mapEventData(d) : (d as T))
        items.value = items.value.filter((i) => options.getItemId(i) !== id)
      })
    }

    // if (options.events.archived && options.archiveField) {
    //   on(options.events.archived, async (d: unknown) => {
    //     const item = await ensureItem(d)
    //     if (!item) return
    //     const id = options.getItemId(item)
    //     const idx = items.value.findIndex((i) => options.getItemId(i) === id)
    //     if (idx !== -1) {
    //       const clone: any = { ...items.value[idx] }
    //       clone[options.archiveField as string] = true
    //       items.value.splice(idx, 1, clone as T)
    //     }
    //   })
    // }
  }

  const disconnect = async () => disconnectSignalR()

  /** expose same API as раньше */
  return {
    items,
    isConnected,
    error,
    connect,
    disconnect,
    addItem: addOrUpdate,
    updateItem: addOrUpdate,
    removeItem: (id: K) => (items.value = items.value.filter((i) => options.getItemId(i) !== id)),
    clear: () => (items.value = []),
  }
}
