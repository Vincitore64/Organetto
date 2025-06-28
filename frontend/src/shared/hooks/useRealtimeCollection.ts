import { ref, type Ref } from 'vue'
import { useSignalRClient } from '@/application/shared'
import { SignalRHubType } from '@/shared/env'

/**
 * Опции для хука useRealtimeCollection
 */
export interface RealtimeCollectionOptions<T, K> {
  /** Тип хаба SignalR для подключения */
  hubType: SignalRHubType
  /** Функция для получения уникального идентификатора элемента */
  getItemId: (item: T) => K
  /** События для прослушивания */
  events: {
    /** Событие создания элемента */
    created?: string
    /** Событие обновления элемента */
    updated?: string
    /** Событие удаления элемента */
    deleted?: string
    /** Событие архивации элемента */
    archived?: string
  }
  /** Функция для преобразования данных события в элемент коллекции */
  mapEventData?: (data: any) => T
  /** Функция для проверки, должен ли элемент быть добавлен в коллекцию */
  shouldAddItem?: (item: T) => boolean
  /** Функция для обновления существующего элемента новыми данными */
  updateItem?: (existingItem: T, newData: T) => T
  /** Поле для архивации (если применимо) */
  archiveField?: keyof T
}

/**
 * Результат работы хука useRealtimeCollection
 */
export interface RealtimeCollectionResult<T> {
  /** Все элементы коллекции */
  items: Ref<T[]>
  /** Статус подключения к SignalR */
  isConnected: Ref<boolean>
  /** Ошибка подключения, если есть */
  error: Ref<Error | null>
  /** Подключиться к хабу */
  connect: () => Promise<void>
  /** Отключиться от хаба */
  disconnect: () => Promise<void>
  /** Добавить элемент в коллекцию */
  addItem: (item: T) => void
  /** Обновить элемент в коллекции */
  updateItem: (item: T) => void
  /** Удалить элемент из коллекции */
  removeItem: (itemId: any) => void
  /** Очистить коллекцию */
  clear: () => void
}

/**
 * Универсальный хук для работы с коллекцией элементов с поддержкой realtime обновлений
 *
 * @param initialItems - Начальный массив элементов
 * @param options - Опции для настройки realtime обновлений
 * @returns Объект для работы с коллекцией
 */
export function useRealtimeCollection<T, K = number | string>(
  initialItems: T[],
  options: RealtimeCollectionOptions<T, K>,
): RealtimeCollectionResult<T> {
  // Инициализируем коллекцию
  const items = ref<T[]>([...initialItems]) as Ref<T[]>

  // Инициализируем SignalR клиент
  const {
    connect: connectSignalR,
    disconnect: disconnectSignalR,
    on,
    isConnected,
    error,
  } = useSignalRClient(options.hubType)

  // Функция для добавления элемента
  const addItem = (item: T) => {
    // Проверяем, должен ли элемент быть добавлен
    if (options.shouldAddItem && !options.shouldAddItem(item)) {
      return
    }

    // Проверяем, существует ли уже элемент с таким ID
    const itemId = options.getItemId(item)
    const existingIndex = items.value.findIndex((i) => options.getItemId(i) === itemId)

    if (existingIndex === -1) {
      // Добавляем новый элемент
      items.value = [...items.value, item]
    } else {
      // Обновляем существующий элемент
      updateItem(item)
    }
  }

  // Функция для обновления элемента
  const updateItem = (item: T) => {
    const itemId = options.getItemId(item)
    const index = items.value.findIndex((i) => options.getItemId(i) === itemId)

    if (index !== -1) {
      // Обновляем элемент с учетом пользовательской функции обновления
      const updatedItem = options.updateItem ? options.updateItem(items.value[index], item) : item

      items.value = [...items.value.slice(0, index), updatedItem, ...items.value.slice(index + 1)]
    }
  }

  // Функция для удаления элемента
  const removeItem = (itemId: K) => {
    items.value = items.value.filter((item) => options.getItemId(item) !== itemId)
  }

  // Функция для очистки коллекции
  const clear = () => {
    items.value = []
  }

  // Функция для подключения к хабу и настройки обработчиков событий
  const connect = async () => {
    await connectSignalR()

    // Настраиваем обработчики событий
    if (options.events.created) {
      on(options.events.created, (data: any) => {
        const item = options.mapEventData ? options.mapEventData(data) : (data as T)
        addItem(item)
      })
    }

    if (options.events.updated) {
      on(options.events.updated, (data: any) => {
        const item = options.mapEventData ? options.mapEventData(data) : (data as T)
        updateItem(item)
      })
    }

    if (options.events.deleted) {
      on(options.events.deleted, (data: any) => {
        // Если данные - это просто ID
        if (typeof data === 'number' || typeof data === 'string') {
          removeItem(data as K)
        } else {
          // Если данные - это объект с ID
          const item = options.mapEventData ? options.mapEventData(data) : (data as T)
          removeItem(options.getItemId(item))
        }
      })
    }

    if (options.events.archived && options.archiveField) {
      on(options.events.archived, (data: any) => {
        const item = options.mapEventData ? options.mapEventData(data) : (data as T)
        const itemId = options.getItemId(item)
        const index = items.value.findIndex((i) => options.getItemId(i) === itemId)

        if (index !== -1) {
          // Устанавливаем поле архивации в true
          const archivedItem = { ...items.value[index] } as any
          archivedItem[options.archiveField as string] = true

          items.value = [
            ...items.value.slice(0, index),
            archivedItem as T,
            ...items.value.slice(index + 1),
          ]
        }
      })
    }
  }

  // Функция для отключения от хаба
  const disconnect = async () => {
    await disconnectSignalR()
  }

  return {
    items,
    isConnected,
    error,
    connect,
    disconnect,
    addItem,
    updateItem,
    removeItem,
    clear,
  }
}
