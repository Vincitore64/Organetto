<template>
  <div
    class="board-column"
    :data-column-id="list.id"
    @dragover.prevent
    @drop="handleDrop"
  >
    <div class="column-header">
      <div class="header-content">
        <h3 class="column-title">{{ list.title }}</h3>
        <div class="column-badge">{{ list.cards.length }}</div>
      </div>
      <a-dropdown :trigger="['click']" placement="bottomRight">
        <a-button
          type="text"
          size="small"
          class="menu-button"
          :aria-label="t('board.column.menu')"
        >
          <template #icon>
            <MoreOutlined />
          </template>
        </a-button>
        <template #overlay>
          <a-menu class="column-menu">
            <a-menu-item key="edit" @click="handleEdit">
              <EditOutlined />
              {{ t('board.column.edit') }}
            </a-menu-item>
            <a-menu-item key="archive" @click="handleArchive">
              <InboxOutlined />
              {{ t('board.column.archive') }}
            </a-menu-item>
            <a-menu-divider />
            <a-menu-item key="delete" danger @click="handleDelete">
              <DeleteOutlined />
              {{ t('board.column.delete') }}
            </a-menu-item>
          </a-menu>
        </template>
      </a-dropdown>
    </div>
    
    <div class="column-content">
      <VirtualList
        :items="list.cards"
        :item-height="120"
        :container-height="600"
        class="cards-list"
      >
        <template #default="{ item }">
          <CardItem
            :card="item"
            class="card-item"
            @click="handleCardClick(item)"
          />
        </template>
      </VirtualList>
      
      <AddCardButton
        :column-id="list.id"
        class="add-card-button"
        @add-card="handleAddCard"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useDraggable } from '@vueuse/core'
import {
  MoreOutlined,
  EditOutlined,
  DeleteOutlined,
  InboxOutlined
} from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
// import { List, Card } from '../../types/board'
import CardItem from './CardItem.vue'
import AddCardButton from './AddCardButton.vue'
import VirtualList from './VirtualList.vue'
import type { ColumnDto } from '@/dataAccess/columns/models'

interface Props {
  list: ColumnDto
}

interface Emits {
  cardClick: [card: Card]
  columnDragStart: [listId: string]
  columnDragEnd: [listId: string]
  cardDragStart: [cardId: string, listId: string]
  cardDragEnd: [cardId: string, listId: string]
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()
const { t } = useI18n()

const columnRef = ref<HTMLElement>()
const isDragging = ref(false)

// Column drag functionality
const { style: dragStyle } = useDraggable(columnRef, {
  initialValue: { x: 0, y: 0 },
  onStart: () => {
    isDragging.value = true
    emit('columnDragStart', props.list.id)
  },
  onEnd: () => {
    isDragging.value = false
    emit('columnDragEnd', props.list.id)
  }
})

const startColumnDrag = () => {
  // Additional column drag logic if needed
}

const handleCardClick = (card: Card) => {
  emit('cardClick', card)
}

const handleCardDragStart = (cardId: string) => {
  emit('cardDragStart', cardId, props.list.id)
}

const handleCardDragEnd = (cardId: string) => {
  emit('cardDragEnd', cardId, props.list.id)
}

const handleDrop = () => {
  // Handle drop logic
}

const handleEdit = () => {
  // Handle edit logic
}

const handleArchive = () => {
  // Handle archive logic
}

const handleDelete = () => {
  // Handle delete logic
}

const handleAddCard = () => {
  // Handle add card logic
}
</script>

<style scoped lang="scss">
// Columns in the board adopt the same surface treatment as cards on the boards
// overview page: a clean white panel with a subtle stroke and shadow.  The glass
// blur effect is removed to improve contrast against the lighter page
// background.
.board-column {
  width: 300px;
  min-width: 300px;
  max-width: 300px;
  background: var(--color-surface);
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 12px;
  box-shadow: var(--shadow-light);
  transition: var(--transition-smooth);
  display: flex;
  flex-direction: column;
  height: fit-content;
  max-height: calc(100vh - 140px);

  &:hover {
    box-shadow: var(--shadow-medium);
    transform: translateY(-2px);
    border-color: var(--color-primary-200, #bae7ff);
  }
}

.column-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px 12px;
  border-bottom: 1px solid rgba(0, 0, 0, 0.04);
  
  .header-content {
    display: flex;
    align-items: center;
    gap: 12px;
    flex: 1;
  }
  
  .column-title {
    font-size: 16px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
  }
  
  .column-badge {
    background: rgba(var(--color-primary-rgb), 0.1);
    color: var(--color-primary-600);
    border: 1px solid rgba(var(--color-primary-rgb), 0.2);
    border-radius: 12px;
    padding: 2px 8px;
    font-size: 12px;
    font-weight: 500;
    min-width: 24px;
    text-align: center;
  }
  
  .menu-button {
    color: var(--color-text-weak);
    border-radius: 6px;
    transition: var(--transition-smooth);
    
    &:hover {
      color: var(--color-text);
      background: rgba(0, 0, 0, 0.04);
    }
  }
}

.column-content {
  display: flex;
  flex-direction: column;
  flex: 1;
  padding: 12px 16px 16px;
  gap: 12px;
  overflow: hidden;
}

.cards-list {
  flex: 1;
  overflow-y: auto;
  
  &::-webkit-scrollbar {
    width: 6px;
  }
  
  &::-webkit-scrollbar-track {
    background: rgba(0, 0, 0, 0.02);
    border-radius: 3px;
  }
  
  &::-webkit-scrollbar-thumb {
    background: rgba(0, 0, 0, 0.1);
    border-radius: 3px;
    
    &:hover {
      background: rgba(0, 0, 0, 0.15);
    }
  }
}

.card-item {
  margin-bottom: 12px;
  
  &:last-child {
    margin-bottom: 0;
  }
}

.add-card-button {
  margin-top: auto;
}

:deep(.column-menu) {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 8px;
  box-shadow: var(--shadow-medium);

  .ant-menu-item {
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(var(--color-primary-rgb), 0.04);
    }
  }
}

@media (max-width: 768px) {
  .board-column {
    width: 280px;
    min-width: 280px;
    max-width: 280px;
  }
  
  .column-header {
    padding: 12px 16px 8px;
    
    .column-title {
      font-size: 14px;
    }
  }
  
  .column-content {
    padding: 8px 12px 12px;
  }
}
</style>