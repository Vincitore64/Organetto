<template>
  <section
    ref="columnRef"
    :class="[styles.column, { [styles.dragging]: isDragging }]"
    :style="dragStyle"
  >
    <header
      :class="styles.header"
      @mousedown="startColumnDrag"
      @touchstart="startColumnDrag"
    >
      <h3 :class="styles.title">{{ list.title }}</h3>
      <span :class="styles.count">{{ list.cards.length }}</span>
      <a-dropdown :trigger="['click']" placement="bottomRight">
        <a-button
          type="text"
          size="small"
          :class="styles.menuButton"
          :aria-label="t('board.column.menu')"
        >
          <template #icon>
            <MoreOutlined />
          </template>
        </a-button>
        <template #overlay>
          <a-menu>
            <a-menu-item key="edit">
              <EditOutlined />
              {{ t('board.column.edit') }}
            </a-menu-item>
            <a-menu-item key="archive">
              <InboxOutlined />
              {{ t('board.column.archive') }}
            </a-menu-item>
            <a-menu-divider />
            <a-menu-item key="delete" danger>
              <DeleteOutlined />
              {{ t('board.column.delete') }}
            </a-menu-item>
          </a-menu>
        </template>
      </a-dropdown>
    </header>
    
    <main :class="styles.content">
      <!-- Virtual scrolling for performance with large card lists -->
      <!-- <VirtualList
        :items="list.cards"
        :item-height="120"
        :class="styles.cardList"
      >
        <template #default="{ item: card }">
          <CardItem
            :key="card.id"
            :card="card"
            @click="handleCardClick(card)"
            @drag-start="handleCardDragStart"
            @drag-end="handleCardDragEnd"
          />
        </template>
      </VirtualList> -->
      
      <!-- <AddCardButton :list-id="list.id" /> -->
    </main>
  </section>
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
import styles from './BoardColumn.module.scss'
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
</script>

<style module="styles" lang="scss">
@import './BoardColumn.module.scss';

.cardList {
  display: grid;
  gap: 8px;
  grid-template-columns: 1fr;
  flex: 1;
  overflow-y: auto;
  padding-right: 4px;
  
  &::-webkit-scrollbar {
    width: 6px;
  }
  
  &::-webkit-scrollbar-track {
    background: transparent;
  }
  
  &::-webkit-scrollbar-thumb {
    background: #cbd5e1;
    border-radius: 3px;
    
    &:hover {
      background: #94a3b8;
    }
  }
}
</style>