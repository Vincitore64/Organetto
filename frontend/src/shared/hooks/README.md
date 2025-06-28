# Shared Hooks

Этот каталог содержит переиспользуемые хуки для работы с real-time обновлениями.

## useRealtimeCollection (V2)

Улучшенная версия универсального хука для работы с коллекциями данных, которые обновляются в реальном времени через SignalR. Включает поддержку автоматической загрузки элементов по ID.

### Возможности

- 🔄 **Real-time обновления** через SignalR
- 🎯 **Типизированные данные** с поддержкой TypeScript
- 🔧 **Гибкая конфигурация** событий и обработчиков
- 📦 **Поддержка архивирования** элементов
- 🎛️ **Управление подключением** (подключение/отключение)
- 🛠️ **Программное управление** коллекцией (добавление, обновление, удаление)
- ✨ **Автозагрузка по ID** - автоматическая загрузка полных объектов при получении только ID
- 🧩 **Lodash оптимизация** - использование Lodash для улучшенной производительности

### Параметры

- `initialItems: T[]` - начальные элементы коллекции
- `options: RealtimeCollectionOptions<T, K>` - конфигурация хука

#### RealtimeCollectionOptions

- `hubType: SignalRHubType` - тип SignalR хаба
- `getItemId: (item: T) => K` - функция получения ID элемента
- `events: RealtimeEvents` - названия событий SignalR
- `fetchItem?: (id: K) => Promise<T>` - **[V2]** функция загрузки элемента по ID
- `mapEventData?: (data: any) => T` - преобразование данных от SignalR
- `shouldAddItem?: (item: T) => boolean` - фильтр для добавления элементов
- `updateItem?: (existing: T, updated: T) => T` - кастомная логика обновления
- `archiveField?: keyof T` - поле для архивирования (по умолчанию 'isArchived')

#### Возвращаемое значение (RealtimeCollectionResult)

| Свойство/Метод | Тип | Описание |
|----------------|-----|----------|
| `items` | `Ref<T[]>` | Реактивный массив элементов коллекции |
| `isConnected` | `Ref<boolean>` | Статус подключения к SignalR |
| `error` | `Ref<Error \| null>` | Ошибка подключения, если есть |
| `connect` | `() => Promise<void>` | Метод для подключения к хабу |
| `disconnect` | `() => Promise<void>` | Метод для отключения от хаба |
| `addItem` | `(item: T) => void` | Метод для добавления элемента в коллекцию |
| `updateItem` | `(item: T) => void` | Метод для обновления элемента в коллекции |
| `removeItem` | `(itemId: K) => void` | Метод для удаления элемента из коллекции |
| `clear` | `() => void` | Метод для очистки коллекции |

## Примеры использования

### Пример с досками (V2 с автозагрузкой)

```typescript
import { useRealtimeCollection } from '@/shared/hooks'
import { SignalRHubType } from '@/shared/env'
import type { BoardDto } from '@/dataAccess/boards/models'
import { apiClient } from '@/dataAccess/shared'

// Функция для загрузки доски по ID
const fetchBoardById = async (boardId: string): Promise<BoardDto> => {
  return await apiClient.boards.getBoard(boardId)
}

const {
  items: boards,
  isConnected,
  error,
  connect,
  disconnect
} = useRealtimeCollection<BoardDto, string>([], {
  hubType: SignalRHubType.Boards,
  getItemId: (board) => board.id,
  events: {
    created: 'BoardCreated',
    updated: 'BoardUpdated',
    deleted: 'BoardDeleted',
    archived: 'BoardArchived'
  },
  fetchItem: fetchBoardById, // ✨ Автозагрузка при получении только ID
  shouldAddItem: (board) => !board.isArchived
})
```

### Пример с задачами

```typescript
import { useRealtimeCollection } from '@/shared/hooks'
import { SignalRHubType } from '@/shared/env'
import type { TaskDto } from '@/dataAccess/tasks/models'
import { apiClient } from '@/dataAccess/shared'

const {
  items: tasks,
  isConnected,
  addItem,
  updateItem,
  removeItem
} = useRealtimeCollection<TaskDto, number>([], {
  hubType: SignalRHubType.Tasks,
  getItemId: (task) => task.id,
  events: {
    created: 'TaskCreated',
    updated: 'TaskUpdated',
    deleted: 'TaskDeleted'
  },
  fetchItem: (taskId) => apiClient.tasks.getTask(taskId), // Автозагрузка
  updateItem: (existing, updated) => ({
    ...updated,
    localModifications: existing.localModifications
  })
})
```

## Новые возможности V2

### Автозагрузка по ID

Когда SignalR отправляет событие с только ID элемента, хук автоматически загружает полный объект:

```typescript
// SignalR отправляет: { id: "board-123" }
// Хук автоматически вызывает: fetchItem("board-123")
// И добавляет полученный объект в коллекцию
```

### Улучшенная обработка событий

- **Создание/Обновление**: поддержка как полных объектов, так и только ID
- **Удаление**: извлечение ID из объекта или использование примитивного значения
- **Архивирование**: глубокое копирование с Lodash для безопасного обновления

### Оптимизация с Lodash

- `findIndex` - быстрый поиск элементов
- `merge` - глубокое слияние объектов
- `cloneDeep` - безопасное копирование
- `isString`, `isNumber`, `isObject` - надежная проверка типов

## Жизненный цикл

1. **Инициализация** - создание реактивных переменных
2. **Подключение** - установка соединения с SignalR хабом
3. **Обработка событий** - автоматическое обновление коллекции с автозагрузкой
4. **Отключение** - закрытие соединения при размонтировании

## Примеры

- `examples/RealtimeCollectionExample.vue` - базовый пример
- `examples/RealtimeCollectionV2Example.vue` - **новый пример с V2 возможностями**
- `examples/RealtimeChatExample.vue` - пример для чата