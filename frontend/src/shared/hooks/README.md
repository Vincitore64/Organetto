# Shared Hooks

Этот каталог содержит универсальные хуки Vue, которые можно использовать в различных частях приложения.

## useRealtimeCollection

`useRealtimeCollection` - это универсальный хук для работы с коллекциями данных, которые требуют обновления в реальном времени через SignalR.

### Возможности

- Работа с любыми типами данных через TypeScript generics
- Автоматическое подключение к SignalR хабу
- Обработка событий создания, обновления, удаления и архивации элементов
- Настраиваемая логика обновления и фильтрации элементов
- Отслеживание статуса подключения и ошибок

### Параметры

```typescript
function useRealtimeCollection<T, K = number | string>(
  initialItems: T[],
  options: RealtimeCollectionOptions<T, K>
): RealtimeCollectionResult<T>
```

#### Опции (RealtimeCollectionOptions)

| Параметр | Тип | Описание |
|----------|-----|----------|
| `hubType` | `SignalRHubType` | Тип хаба SignalR для подключения |
| `getItemId` | `(item: T) => K` | Функция для получения уникального идентификатора элемента |
| `events` | `object` | События для прослушивания |
| `events.created` | `string` (опционально) | Событие создания элемента |
| `events.updated` | `string` (опционально) | Событие обновления элемента |
| `events.deleted` | `string` (опционально) | Событие удаления элемента |
| `events.archived` | `string` (опционально) | Событие архивации элемента |
| `mapEventData` | `(data: any) => T` (опционально) | Функция для преобразования данных события в элемент коллекции |
| `shouldAddItem` | `(item: T) => boolean` (опционально) | Функция для проверки, должен ли элемент быть добавлен в коллекцию |
| `updateItem` | `(existingItem: T, newData: T) => T` (опционально) | Функция для обновления существующего элемента новыми данными |
| `archiveField` | `keyof T` (опционально) | Поле для архивации (если применимо) |

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

### Пример использования

```typescript
// Для досок (boards)
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
  archiveField: 'isArchived',
  shouldAddItem: (board) => !board.isArchived
});

// Для задач (tasks)
const {
  items: tasks,
  connect: connectTasks
} = useRealtimeCollection<TaskDto, number>(initialTasks, {
  hubType: SignalRHubType.Tasks,
  getItemId: (task) => task.id,
  events: {
    created: 'TaskCreated',
    updated: 'TaskUpdated',
    deleted: 'TaskDeleted'
  },
  mapEventData: (data) => ({
    ...data,
    dueDate: data.dueDate ? new Date(data.dueDate) : null
  })
});
```

### Жизненный цикл

Рекомендуется подключаться к хабу при монтировании компонента и отключаться при размонтировании:

```typescript
onMounted(async () => {
  await connect();
});

onBeforeUnmount(async () => {
  await disconnect();
});
```

### Полный пример

Полный пример использования хука можно найти в файле `examples/RealtimeCollectionExample.vue`.