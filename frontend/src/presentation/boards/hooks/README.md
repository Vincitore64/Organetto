# Хуки для работы с досками

## useBoardPageViews

Хук `useBoardPageViews` предоставляет функцию для получения списка досок пользователя.

```typescript
import { useBoardPageViews } from '@/presentation/boards'
import { ApiClient } from '@/dataAccess/services/ApiClient'

// В компоненте Vue
const apiClient = inject(ApiClient)
const fetchBoardPageViews = useBoardPageViews(apiClient)

// Использование
const { allViews, searchTerms, sortBy, sortDirection, views } = await fetchBoardPageViews({ userId: 123 })
```

## useRealtimeBoardPageViews

Хук `useRealtimeBoardPageViews` расширяет функциональность `useBoardPageViews`, добавляя поддержку realtime обновлений через SignalR.

```typescript
import { useRealtimeBoardPageViews } from '@/presentation/boards'
import { ApiClient } from '@/dataAccess/services/ApiClient'

// В компоненте Vue
const apiClient = inject(ApiClient)
const fetchBoardPageViews = useRealtimeBoardPageViews(apiClient)

// Использование
const { 
  allViews, 
  searchTerms, 
  sortBy, 
  sortDirection, 
  views,
  isConnected, // статус подключения к SignalR
  error // ошибка подключения, если есть
} = await fetchBoardPageViews({ userId: 123 })
```

### Поддерживаемые realtime события

- `BoardCreated` - создание новой доски
- `BoardUpdated` - обновление существующей доски
- `BoardDeleted` - удаление доски
- `BoardArchived` - архивация доски

При получении этих событий от сервера, список досок автоматически обновляется без необходимости повторного запроса данных.