<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import { useRealtimeCollection } from '@/shared/hooks/useRealtimeCollection'
import { SignalRHubType } from '@/shared/env'
import type { BoardDto } from '@/dataAccess/boards/models'
import { apiClient } from '@/dataAccess/shared'

// Состояние загрузки
const isLoading = ref(true)
const loadError = ref<Error | null>(null)

// Получаем начальные данные
const fetchBoards = async () => {
  try {
    isLoading.value = true
    // Предполагаем, что у нас есть метод для получения ID текущего пользователя
    const userId = 'current-user-id' // В реальном приложении получите актуальный ID
    const boards = await apiClient.boards.getBoards(userId)
    return boards
  } catch (error) {
    loadError.value = error as Error
    return []
  } finally {
    isLoading.value = false
  }
}

// Инициализируем коллекцию с realtime обновлениями
const {
  items: boards,
  isConnected,
  error: connectionError,
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
  // Опционально: фильтрация элементов
  shouldAddItem: (board) => !board.isArchived,
  // Опционально: кастомная логика обновления
  updateItem: (existingBoard, newBoard) => ({
    ...existingBoard,
    ...newBoard,
    // Сохраняем локальные свойства, если они есть
    localProperty: existingBoard.localProperty
  })
})

// Поиск и сортировка
const searchTerm = ref('')
const sortBy = ref('title')
const sortDirection = ref<'asc' | 'desc'>('asc')

// Отфильтрованные и отсортированные доски
const filteredBoards = computed(() => {
  // Фильтрация по поисковому запросу
  let result = boards.value
  
  if (searchTerm.value) {
    const term = searchTerm.value.toLowerCase()
    result = result.filter(board => 
      board.title.toLowerCase().includes(term) ||
      (board.description && board.description.toLowerCase().includes(term))
    )
  }
  
  // Сортировка
  return [...result].sort((a, b) => {
    // @ts-ignore - динамический доступ к свойствам
    const valueA = a[sortBy.value]
    // @ts-ignore - динамический доступ к свойствам
    const valueB = b[sortBy.value]
    
    if (typeof valueA === 'string' && typeof valueB === 'string') {
      return sortDirection.value === 'asc' 
        ? valueA.localeCompare(valueB) 
        : valueB.localeCompare(valueA)
    }
    
    return sortDirection.value === 'asc' 
      ? (valueA > valueB ? 1 : -1) 
      : (valueA < valueB ? 1 : -1)
  })
})

// Инициализация при монтировании компонента
onMounted(async () => {
  // Загружаем начальные данные
  const initialBoards = await fetchBoards()
  
  // Добавляем загруженные доски в коллекцию
  initialBoards.forEach(board => {
    boards.value.push(board)
  })
  
  // Подключаемся к SignalR для получения обновлений
  await connect()
})

// Отключаемся при размонтировании компонента
onBeforeUnmount(async () => {
  await disconnect()
})

// Обработчики сортировки
const toggleSort = (field: string) => {
  if (sortBy.value === field) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
  } else {
    sortBy.value = field
    sortDirection.value = 'asc'
  }
}
</script>

<template>
  <div class="realtime-collection-example">
    <div class="status-bar">
      <div class="connection-status" :class="{ connected: isConnected }">
        {{ isConnected ? 'Подключено к realtime обновлениям' : 'Не подключено' }}
      </div>
      <div v-if="connectionError" class="error-message">
        Ошибка подключения: {{ connectionError.message }}
      </div>
    </div>
    
    <div class="search-sort-container">
      <input 
        v-model="searchTerm" 
        placeholder="Поиск досок..." 
        class="search-input"
      />
      
      <div class="sort-controls">
        <span>Сортировать по:</span>
        <button 
          @click="toggleSort('title')" 
          :class="{ active: sortBy === 'title' }"
        >
          Название {{ sortBy === 'title' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}
        </button>
        <button 
          @click="toggleSort('updatedAt')" 
          :class="{ active: sortBy === 'updatedAt' }"
        >
          Дата обновления {{ sortBy === 'updatedAt' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}
        </button>
      </div>
    </div>
    
    <div v-if="isLoading" class="loading">
      Загрузка досок...
    </div>
    
    <div v-else-if="loadError" class="error-message">
      Ошибка загрузки: {{ loadError.message }}
    </div>
    
    <div v-else-if="filteredBoards.length === 0" class="empty-state">
      Доски не найдены
    </div>
    
    <div v-else class="boards-grid">
      <div 
        v-for="board in filteredBoards" 
        :key="board.id" 
        class="board-card"
      >
        <h3>{{ board.title }}</h3>
        <p v-if="board.description">{{ board.description }}</p>
        <div class="board-meta">
          <span>Создано: {{ new Date(board.createdAt).toLocaleDateString() }}</span>
          <span>Обновлено: {{ new Date(board.updatedAt).toLocaleDateString() }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.realtime-collection-example {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.status-bar {
  margin-bottom: 20px;
  display: flex;
  align-items: center;
  gap: 10px;
}

.connection-status {
  padding: 5px 10px;
  border-radius: 4px;
  background-color: #f44336;
  color: white;
}

.connection-status.connected {
  background-color: #4caf50;
}

.search-sort-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.search-input {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 300px;
}

.sort-controls {
  display: flex;
  align-items: center;
  gap: 10px;
}

.sort-controls button {
  padding: 6px 12px;
  border: 1px solid #ddd;
  background-color: white;
  border-radius: 4px;
  cursor: pointer;
}

.sort-controls button.active {
  background-color: #e0e0e0;
  font-weight: bold;
}

.boards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
}

.board-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 15px;
  background-color: white;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.board-card h3 {
  margin-top: 0;
  margin-bottom: 10px;
}

.board-meta {
  margin-top: 15px;
  display: flex;
  justify-content: space-between;
  font-size: 0.8rem;
  color: #666;
}

.loading,
.error-message,
.empty-state {
  text-align: center;
  padding: 40px;
  color: #666;
}

.error-message {
  color: #f44336;
}
</style>