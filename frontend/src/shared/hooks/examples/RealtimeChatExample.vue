<script setup lang="ts">
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { useRealtimeCollection } from '@/shared/hooks/useRealtimeCollection'
import { SignalRHubType } from '@/shared/env'

// Определение типа сообщения чата
interface ChatMessage {
  id: string
  text: string
  senderId: string
  senderName: string
  timestamp: string
  isRead: boolean
  chatId: string
}

// Имитация API клиента для чата
const chatApiClient = {
  async getMessages(chatId: string): Promise<ChatMessage[]> {
    // В реальном приложении здесь был бы API запрос
    // Возвращаем тестовые данные для примера
    return [
      {
        id: '1',
        text: 'Привет! Как дела?',
        senderId: 'user1',
        senderName: 'Иван',
        timestamp: new Date(Date.now() - 3600000).toISOString(),
        isRead: true,
        chatId
      },
      {
        id: '2',
        text: 'Привет! Всё хорошо, спасибо!',
        senderId: 'user2',
        senderName: 'Мария',
        timestamp: new Date(Date.now() - 3500000).toISOString(),
        isRead: true,
        chatId
      },
      {
        id: '3',
        text: 'Что нового?',
        senderId: 'user1',
        senderName: 'Иван',
        timestamp: new Date(Date.now() - 3400000).toISOString(),
        isRead: true,
        chatId
      }
    ]
  },
  
  async sendMessage(message: Omit<ChatMessage, 'id' | 'timestamp'>): Promise<ChatMessage> {
    // В реальном приложении здесь был бы API запрос
    // Имитируем отправку сообщения
    return {
      ...message,
      id: Math.random().toString(36).substring(2, 15),
      timestamp: new Date().toISOString()
    }
  }
}

// Параметры чата
const chatId = 'chat-123'
const currentUserId = 'user2'
const currentUserName = 'Мария'

// Состояние загрузки
const isLoading = ref(true)
const loadError = ref<Error | null>(null)

// Инициализируем коллекцию сообщений с realtime обновлениями
const {
  items: messages,
  isConnected,
  error: connectionError,
  connect,
  disconnect,
  addItem: addMessage
} = useRealtimeCollection<ChatMessage, string>([], {
  hubType: SignalRHubType.Chat,
  getItemId: (message) => message.id,
  events: {
    created: 'MessageSent',
    updated: 'MessageUpdated',
    deleted: 'MessageDeleted'
  },
  // Фильтруем сообщения только для текущего чата
  shouldAddItem: (message) => message.chatId === chatId,
  // Преобразуем данные события в формат сообщения
  mapEventData: (data) => ({
    ...data,
    // Убедимся, что timestamp в правильном формате
    timestamp: data.timestamp ? data.timestamp : new Date().toISOString()
  })
})

// Текст нового сообщения
const newMessageText = ref('')

// Отсортированные сообщения по времени
const sortedMessages = computed(() => {
  return [...messages.value].sort((a, b) => 
    new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime()
  )
})

// Загрузка сообщений
const loadMessages = async () => {
  try {
    isLoading.value = true
    const chatMessages = await chatApiClient.getMessages(chatId)
    
    // Добавляем сообщения в коллекцию
    chatMessages.forEach(message => {
      addMessage(message)
    })
  } catch (error) {
    loadError.value = error as Error
  } finally {
    isLoading.value = false
  }
}

// Отправка нового сообщения
const sendMessage = async () => {
  if (!newMessageText.value.trim()) return
  
  try {
    const messageData = {
      text: newMessageText.value,
      senderId: currentUserId,
      senderName: currentUserName,
      isRead: false,
      chatId
    }
    
    // Отправляем сообщение через API
    const sentMessage = await chatApiClient.sendMessage(messageData)
    
    // Очищаем поле ввода
    newMessageText.value = ''
    
    // В реальном приложении сообщение придет через SignalR
    // Для демонстрации добавляем его вручную
    addMessage(sentMessage)
  } catch (error) {
    console.error('Ошибка при отправке сообщения:', error)
  }
}

// Форматирование времени сообщения
const formatMessageTime = (timestamp: string) => {
  const date = new Date(timestamp)
  return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}

// Инициализация при монтировании компонента
onMounted(async () => {
  // Загружаем начальные сообщения
  await loadMessages()
  
  // Подключаемся к SignalR для получения обновлений
  await connect()
})

// Отключаемся при размонтировании компонента
onBeforeUnmount(async () => {
  await disconnect()
})
</script>

<template>
  <div class="chat-container">
    <div class="chat-header">
      <h2>Чат</h2>
      <div class="connection-status" :class="{ connected: isConnected }">
        {{ isConnected ? 'Подключено' : 'Не подключено' }}
      </div>
    </div>
    
    <div v-if="connectionError" class="error-message">
      Ошибка подключения: {{ connectionError.message }}
    </div>
    
    <div class="messages-container">
      <div v-if="isLoading" class="loading-messages">
        Загрузка сообщений...
      </div>
      
      <div v-else-if="loadError" class="error-message">
        Ошибка загрузки сообщений: {{ loadError.message }}
      </div>
      
      <div v-else-if="sortedMessages.length === 0" class="no-messages">
        Нет сообщений. Начните общение!
      </div>
      
      <template v-else>
        <div 
          v-for="message in sortedMessages" 
          :key="message.id"
          class="message"
          :class="{ 'own-message': message.senderId === currentUserId }"
        >
          <div class="message-content">
            <div class="message-header">
              <span class="sender-name">{{ message.senderName }}</span>
              <span class="message-time">{{ formatMessageTime(message.timestamp) }}</span>
            </div>
            <div class="message-text">{{ message.text }}</div>
          </div>
        </div>
      </template>
    </div>
    
    <div class="message-input-container">
      <input 
        v-model="newMessageText" 
        placeholder="Введите сообщение..." 
        @keyup.enter="sendMessage"
        class="message-input"
      />
      <button @click="sendMessage" class="send-button">
        Отправить
      </button>
    </div>
  </div>
</template>

<style scoped>
.chat-container {
  display: flex;
  flex-direction: column;
  height: 600px;
  width: 100%;
  max-width: 800px;
  margin: 0 auto;
  border: 1px solid #ddd;
  border-radius: 8px;
  overflow: hidden;
  background-color: #f9f9f9;
}

.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  background-color: #4a76a8;
  color: white;
}

.chat-header h2 {
  margin: 0;
}

.connection-status {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 0.8rem;
  background-color: #f44336;
}

.connection-status.connected {
  background-color: #4caf50;
}

.messages-container {
  flex: 1;
  padding: 15px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.message {
  display: flex;
  margin-bottom: 10px;
}

.message-content {
  max-width: 70%;
  padding: 10px;
  border-radius: 8px;
  background-color: #e9e9eb;
}

.own-message {
  justify-content: flex-end;
}

.own-message .message-content {
  background-color: #dcf8c6;
}

.message-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 5px;
  font-size: 0.8rem;
}

.sender-name {
  font-weight: bold;
  color: #4a76a8;
}

.message-time {
  color: #888;
}

.message-text {
  word-break: break-word;
}

.message-input-container {
  display: flex;
  padding: 15px;
  background-color: white;
  border-top: 1px solid #ddd;
}

.message-input {
  flex: 1;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  margin-right: 10px;
}

.send-button {
  padding: 10px 15px;
  background-color: #4a76a8;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.send-button:hover {
  background-color: #3d6293;
}

.loading-messages,
.error-message,
.no-messages {
  text-align: center;
  padding: 20px;
  color: #666;
}

.error-message {
  color: #f44336;
}
</style>