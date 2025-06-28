# SignalR Hooks

## useSignalRClient

Хук `useSignalRClient` предоставляет удобный интерфейс для работы с SignalR в компонентах Vue.

### Пример использования

```typescript
import { useSignalRClient } from '@/application/shared'
import { SignalRHubType } from '@/shared/env'
import { ref, onMounted } from 'vue'

export default {
  setup() {
    // Инициализация клиента для хаба досок
    const { connect, disconnect, on, send, isConnected, error } = useSignalRClient(SignalRHubType.Boards)
    
    // Данные из хаба
    const boards = ref([])
    
    // Подписка на события
    on('BoardsUpdated', (updatedBoards) => {
      boards.value = updatedBoards
    })
    
    // Подключение при монтировании компонента
    onMounted(async () => {
      await connect()
      
      // Отправка сообщения на сервер
      await send('GetBoards', { userId: 123 })
    })
    
    return {
      boards,
      isConnected,
      error
    }
  }
}
```

### Доступные хабы

В приложении доступны следующие типы хабов:

- `SignalRHubType.Boards` - хаб для работы с досками
- `SignalRHubType.Notifications` - хаб для работы с уведомлениями
- `SignalRHubType.Chat` - хаб для работы с чатом

### Вспомогательные функции

Для получения URL хабов можно использовать следующие функции из модуля `@/shared/env`:

- `getSignalRHubUrl(hubType: SignalRHubType)` - получает URL для указанного типа хаба
- `getBoardsHubUrl()` - получает URL для хаба досок
- `getNotificationsHubUrl()` - получает URL для хаба уведомлений
- `getChatHubUrl()` - получает URL для хаба чата