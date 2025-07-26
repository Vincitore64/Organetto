<template>
  <div
    v-if="isDragging"
    class="drag-ghost"
    :style="{
      transform: `translate(${position.x}px, ${position.y}px)`,
      pointerEvents: 'none'
    }"
  >
    <a-card class="ghost-card" :bordered="false">
      <div class="card-content">
        <h4 class="card-title">{{ draggedCard?.title }}</h4>
        <p v-if="draggedCard?.description" class="card-description">
          {{ draggedCard.description }}
        </p>
      </div>
      
      <div v-if="hasMetadata" class="card-metadata">
        <div v-if="draggedCard?.dueDate" class="metadata-item due-date">
          <ClockCircleOutlined />
          <span>{{ formatDate(draggedCard.dueDate) }}</span>
        </div>
        
        <div v-if="draggedCard?.commentsCount" class="metadata-item comments">
          <MessageOutlined />
          <span>{{ draggedCard.commentsCount }}</span>
        </div>
        
        <div v-if="draggedCard?.attachmentsCount" class="metadata-item attachments">
          <PaperClipOutlined />
          <span>{{ draggedCard.attachmentsCount }}</span>
        </div>
      </div>
      
      <div v-if="moreCardsCount > 0" class="more-cards">
        {{ t('board.dragGhost.moreCards', { count: moreCardsCount }) }}
      </div>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { Card, List } from '../../types/board'
import CardItem from './CardItem.vue'
import type { CardDto } from '@/dataAccess/boards/models'

interface Props {
  type: 'card' | 'list'
  item: Card | List
}

const props = defineProps<Props>()
const { t } = useI18n()

const previewCards = computed(() => {
  if (props.type === 'list') {
    const list = props.item as List
    return list.cards.slice(0, 3)
  }
  return []
})

const remainingCardsCount = computed(() => {
  if (props.type === 'list') {
    const list = props.item as List
    return Math.max(0, list.cards.length - 3)
  }
  return 0
})
</script>

<style scoped lang="scss">
.drag-ghost {
  position: fixed;
  top: 0;
  left: 0;
  z-index: 9999;
  pointer-events: none;
  transform-origin: top left;
  
  .ghost-card {
    width: 280px;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(20px);
    border: 1px solid rgba(0, 0, 0, 0.08);
    border-radius: 12px;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
    transform: rotate(5deg) scale(0.95);
    transition: none;
    
    :deep(.ant-card-body) {
      padding: 16px;
    }
  }
}

.card-content {
  margin-bottom: 12px;
  
  .card-title {
    font-size: 14px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0 0 8px 0;
    line-height: 1.4;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
  }
  
  .card-description {
    font-size: 12px;
    color: var(--color-text-weak);
    margin: 0;
    line-height: 1.4;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
}

.card-metadata {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 8px;
  
  .metadata-item {
    display: flex;
    align-items: center;
    gap: 4px;
    font-size: 11px;
    color: var(--color-text-weak);
    
    span {
      font-weight: 500;
    }
    
    &.due-date {
      color: var(--color-orange-600);
    }
    
    &.comments {
      color: var(--color-blue-600);
    }
    
    &.attachments {
      color: var(--color-green-600);
    }
  }
}

.more-cards {
  font-size: 11px;
  color: var(--color-text-weak);
  font-weight: 500;
  text-align: center;
  padding: 8px;
  background: rgba(0, 0, 0, 0.02);
  border-radius: 6px;
  margin-top: 8px;
}

@media (max-width: 768px) {
  .drag-ghost {
    .ghost-card {
      width: 240px;
      
      :deep(.ant-card-body) {
        padding: 12px;
      }
    }
  }
  
  .card-content {
    margin-bottom: 10px;
    
    .card-title {
      font-size: 13px;
    }
    
    .card-description {
      font-size: 11px;
    }
  }
  
  .card-metadata {
    gap: 8px;
    
    .metadata-item {
      font-size: 10px;
    }
  }
  
  .more-cards {
    font-size: 10px;
    padding: 6px;
  }
}
</style>