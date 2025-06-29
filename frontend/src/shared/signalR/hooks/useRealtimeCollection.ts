import { type Ref } from 'vue'
import { useSignalRClient } from './useSignalRClient'
import { SignalRHubType } from '../models'
import { isString, isNumber, isObject, findIndex, cloneDeep, merge, get } from 'lodash'
import { z, type ZodSchema } from 'zod'

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

  /** Zod схема для объектов только с ID */
  idOnlySchema?: ZodSchema

  /** Zod схема для полных объектов */
  fullObjectSchema?: ZodSchema

  /** Ключ для получения ID из объекта (по умолчанию 'id') */
  idKey?: string
}

/**
 * Универсальный хук для работы с коллекциями данных с поддержкой realtime обновлений через SignalR.
 *
 * Поддерживает автоматическую загрузку элементов по ID, если SignalR отправляет только идентификаторы.
 * Использует Lodash для оптимизации операций с массивами и объектами.
 * Использует Zod для типобезопасной валидации схем объектов.
 *
 * Новые возможности:
 * - idOnlySchema: Zod схема для валидации объектов только с ID
 * - fullObjectSchema: Zod схема для валидации полных объектов
 * - idKey: настраиваемый ключ для получения ID из объектов
 * - Типобезопасная валидация с помощью Zod
 * - Безопасный доступ к свойствам через Lodash get
 *
 * @template T - Тип элементов коллекции
 * @template K - Тип идентификатора элемента (по умолчанию number | string)
 * @param initialItems - Начальные элементы коллекции
 * @param options - Опции конфигурации хука
 * @returns Объект с реактивной коллекцией и методами управления
 *
 * @example
 * ```typescript
 * // Определяем Zod схемы
 * const idOnlySchema = z.object({ id: z.number() }).strict()
 * const fullObjectSchema = z.object({
 *   id: z.number(),
 *   name: z.string(),
 *   status: z.string()
 * })
 *
 * const collection = useRealtimeCollection(items, {
 *   // ... другие опции
 *   idOnlySchema,
 *   fullObjectSchema,
 *   idKey: 'id',
 *   fetchItem: async (id) => await api.getItem(id)
 * })
 * ```
 */
export function useRealtimeCollection<T, K = number | string>(
  items: Ref<T[]>,
  options: RealtimeCollectionOptions<T, K>,
) {
  // const items = ref<T[]>([...initialItems]) as Ref<T[]>

  const {
    connect: connectSignalR,
    disconnect: disconnectSignalR,
    on,
    isConnected,
    error,
  } = useSignalRClient(options.hubType)

  /** ───────────────── helpers ───────────────── */

  /**
   * Добавляет новый элемент в коллекцию или обновляет существующий.
   * Использует merge из Lodash для глубокого слияния объектов при обновлении.
   *
   * @param item - Элемент для добавления или обновления
   */
  const addOrUpdate = (item: T) => {
    if (!item) return

    const itemId = options.getItemId(item)
    const idx = findIndex(items.value, (i) => options.getItemId(i) === itemId)

    if (idx === -1) {
      // Добавляем новый элемент, если он проходит фильтр
      if (options.shouldAddItem?.(item) !== false) {
        items.value = [...items.value, item]
      }
    } else {
      // Обновляем существующий элемент с глубоким слиянием
      const existingItem = items.value[idx]
      const updatedItem = options.updateItem
        ? options.updateItem(existingItem, item)
        : merge(cloneDeep(existingItem), item)

      items.value.splice(idx, 1, updatedItem)
    }
  }

  /**
   * Преобразует payload от SignalR в полноценный объект типа T.
   * Поддерживает три сценария:
   * 1. Payload - примитивный ID (число/строка) → загружает объект через fetchItem
   * 2. Payload - объект только с ID → загружает полный объект через fetchItem
   * 3. Payload - полный объект → применяет mapEventData если нужно
   *
   * @param payload - Данные от SignalR (ID или объект)
   * @returns Полноценный объект типа T или null в случае ошибки
   */
  const ensureItem = async (payload: unknown): Promise<T | null> => {
    if (!payload) return null

    // Если payload - это примитивный ID (число или строка)
    if (isNumber(payload) || isString(payload)) {
      if (!options.fetchItem) {
        console.warn('[useRealtimeCollection] Received ID but no fetchItem function provided')
        return null
      }
      try {
        return await options.fetchItem(payload as K)
      } catch (e) {
        console.error('[useRealtimeCollection] fetchItem failed for ID:', payload, e)
        return null
      }
    }

    // Если payload - это объект
    if (isObject(payload)) {
      try {
        // Определяем, является ли объект только ID-объектом или полным объектом
        const isIdOnlyObject = isObjectWithIdOnly(payload)

        if (isIdOnlyObject && options.fetchItem) {
          // Получаем ID из объекта только с идентификатором
          const itemId = getIdFromObject(payload)
          if (itemId) {
            try {
              return await options.fetchItem(itemId)
            } catch (e) {
              console.warn('[useRealtimeCollection] fetchItem failed for ID-only object:', e)
              // Если загрузка не удалась, используем payload как есть
            }
          }
        }

        // Используем объект как есть (полный объект), применяя mapEventData если есть
        return options.mapEventData ? options.mapEventData(payload) : (payload as T)
      } catch (e) {
        console.error('[useRealtimeCollection] Failed to process object payload:', e)
        return null
      }
    }

    console.warn('[useRealtimeCollection] Unknown payload type:', typeof payload)
    return null
  }

  /**
   * Определяет, является ли объект объектом только с ID, используя Zod схемы
   */
  const isObjectWithIdOnly = (obj: unknown): boolean => {
    if (!isObject(obj)) return false

    // Если указана схема для ID-объектов, используем её
    if (options.idOnlySchema) {
      const result = options.idOnlySchema.safeParse(obj)
      return result.success
    }

    // Если указана схема для полных объектов, проверяем что объект НЕ соответствует ей
    if (options.fullObjectSchema) {
      const result = options.fullObjectSchema.safeParse(obj)
      return !result.success
    }

    // Fallback: создаем простую схему для объекта только с ID
    const idKey = options.idKey || 'id'
    const idOnlySchema = z
      .object({
        [idKey]: z.union([z.string(), z.number()]),
      })
      .strict() // strict() означает, что других ключей быть не должно

    const result = idOnlySchema.safeParse(obj)
    return result.success
  }

  /**
   * Извлекает ID из объекта, используя безопасный доступ через Lodash get
   */
  const getIdFromObject = (obj: unknown): K | null => {
    if (!isObject(obj)) return null

    const idKey = options.idKey || 'id'

    // Сначала пытаемся получить ID по указанному ключу
    const id = get(obj, idKey) as K
    if (id !== undefined && id !== null) {
      return id
    }

    // Fallback: пытаемся использовать getItemId
    try {
      return options.getItemId(obj as T)
    } catch {
      // Если getItemId не работает, пытаемся найти стандартные поля ID
      return (get(obj, 'id') as K) || (get(obj, 'Id') as K) || null
    }
  }

  /** ───────────────── core API ───────────────── */

  const connect = async () => {
    await connectSignalR()

    if (options.events.created) {
      on(options.events.created, async (d: unknown) => {
        debugger
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
        if (!d) return

        let id: K | null = null

        // Если пришел примитивный ID
        if (isNumber(d) || isString(d)) {
          id = d as K
        } else if (isObject(d)) {
          // Если пришел объект, извлекаем ID используя новую логику
          id = getIdFromObject(d)
          if (!id) {
            console.error('[useRealtimeCollection] Failed to extract ID from delete payload')
            return
          }
        } else {
          console.warn('[useRealtimeCollection] Invalid delete payload type:', typeof d)
          return
        }

        // Удаляем элемент из коллекции
        if (id !== null) {
          items.value = items.value.filter((i) => options.getItemId(i) !== id)
        }
      })
    }

    if (options.events.archived && options.archiveField) {
      on(options.events.archived, async (d: unknown) => {
        const item = await ensureItem(d)
        if (!item) return

        const id = options.getItemId(item)
        const idx = findIndex(items.value, (i) => options.getItemId(i) === id)

        if (idx !== -1) {
          // Создаем глубокую копию и устанавливаем поле архивации
          const archivedItem = cloneDeep(items.value[idx]) as Record<string, unknown>
          archivedItem[options.archiveField as string] = true

          items.value.splice(idx, 1, archivedItem as T)
        }
      })
    }
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
    removeItem: (id: K) => {
      items.value = items.value.filter((i) => options.getItemId(i) !== id)
    },
    clear: () => (items.value = []),
  }
}
