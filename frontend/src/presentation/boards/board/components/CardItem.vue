<script setup lang="ts">
import { ref, computed } from 'vue'
import { useDraggable } from '@vueuse/core'
import {
  CalendarOutlined,
  MessageOutlined,
  PaperClipOutlined,
  CheckSquareOutlined,
  SettingOutlined, EditOutlined, EllipsisOutlined
} from '@ant-design/icons-vue'
import dayjs from 'dayjs'
import { Card } from '../../types/board'
import AvatarGroup from './AvatarGroup.vue'


interface Props {
  card: Card
}

interface Emits {
  click: [card: Card]
  dragStart: [cardId: string]
  dragEnd: [cardId: string]
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()

const cardRef = ref<HTMLElement>()
const isDragging = ref(false)

// Drag functionality using VueUse
const { style: dragStyle } = useDraggable(cardRef, {
  initialValue: { x: 0, y: 0 },
  onStart: () => {
    isDragging.value = true
    emit('dragStart', props.card.id)
  },
  onEnd: () => {
    isDragging.value = false
    emit('dragEnd', props.card.id)
  }
})

// Mock data for demonstration
const mockUsers = computed(() => 
  props.card.assignees.map(id => ({
    id,
    name: `User ${id}`,
    avatar: '/api/placeholder/24/24'
  }))
)

const hasAttachments = computed(() => Math.random() > 0.7)
const hasComments = computed(() => Math.random() > 0.6)
const hasChecklist = computed(() => Math.random() > 0.5)

const commentCount = computed(() => Math.floor(Math.random() * 5) + 1)
const attachmentCount = computed(() => Math.floor(Math.random() * 3) + 1)
const checklistProgress = computed(() => {
  const completed = Math.floor(Math.random() * 5) + 1
  const total = completed + Math.floor(Math.random() * 3) + 1
  return `${completed}/${total}`
})

const formatDate = (date: Date) => {
  return dayjs(date).format('MMM DD')
}

const handleClick = () => {
  if (!isDragging.value) {
    emit('click', props.card)
  }
}

const startDrag = () => {
  // Additional drag start logic if needed
}
</script>

<template>
  <a-card
    class="board-card"
    :class="{ 'card-dragging': isDragging }"
    :draggable="true"
    @dragstart="handleDragStart"
    @dragend="handleDragEnd"
    @click="handleClick"
    :hoverable="true"
    size="small"
  >
    <!-- Labels -->
    <div v-if="card.labels?.length" class="card-labels">
      <a-tag
        v-for="label in card.labels"
        :key="label.id"
        class="card-label"
        :style="{ backgroundColor: label.color, borderColor: label.color }"
        size="small"
      >
        {{ label.name }}
      </a-tag>
    </div>
    <template #actions>
      <setting-outlined key="setting" />
      <edit-outlined key="edit" />
      <ellipsis-outlined key="ellipsis" />
    </template>
    <a-card-meta class="board-card__meta" :title="card.title" :description="card.description">

    </a-card-meta>

    <!-- Title -->
    <!-- <h4 class="card-title">{{ card.title }}</h4> -->

    <!-- Description -->
    <!-- <p v-if="card.description" class="card-description">
      {{ card.description }}
    </p> -->

    <!-- Footer with badges -->
    <div v-if="hasFooterContent" class="card-footer">
      <div class="card-badges">
        <!-- Due date -->
        <div v-if="card.dueDate" class="card-badge due-date">
          <ClockCircleOutlined />
          <span>{{ formatDueDate(card.dueDate) }}</span>
        </div>

        <!-- Comments count -->
        <div v-if="card.commentsCount" class="card-badge comments">
          <MessageOutlined />
          <span>{{ card.commentsCount }}</span>
        </div>

        <!-- Attachments count -->
        <div v-if="card.attachmentsCount" class="card-badge attachments">
          <PaperClipOutlined />
          <span>{{ card.attachmentsCount }}</span>
        </div>

        <!-- Checklist progress -->
        <div v-if="card.checklistsCount" class="card-badge checklist">
          <CheckSquareOutlined />
          <span>{{ card.completedChecklistsCount }}/{{ card.checklistsCount }}</span>
        </div>
      </div>

      <!-- Assigned users -->
      <AvatarGroup
        v-if="card.assignedUsers?.length"
        :users="card.assignedUsers"
        :max-visible="3"
        size="sm"
        class="card-avatars"
      />
    </div>
  </a-card>
</template>

<style scoped lang="scss">
// Cards within a column use the same surface and hover treatment as boards
// overview cards.  Removing the blur effect improves clarity against the
// lighter page background and aligns with the rest of the application.
.board-card {
  width: 100%;
  // background: var(--color-surface);
  // border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 10px;
  box-shadow: var(--shadow-light);
  transition: var(--transition-smooth);
  cursor: pointer;
  margin-bottom: 12px;

  &__meta {
    :deep(.ant-card-meta-title) {
      font-weight: 400;
    }
  }

  &:hover {
    box-shadow: var(--shadow-light-plus);
    transform: translateY(-2px);
    border-color: var(--color-primary-200, #bae7ff);
  }

  &.card-dragging {
    opacity: 0.6;
    transform: rotate(5deg);
  }

  // :deep(.ant-card-body) {
  //   padding: 16px;
  // }
}

.card-labels {
  display: flex;
  flex-wrap: wrap;
  gap: 6px;
  margin-bottom: 12px;
  
  .card-label {
    color: white;
    border: none;
    font-size: 11px;
    font-weight: 500;
    padding: 2px 8px;
    border-radius: 12px;
    text-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
  }
}

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
  font-size: 13px;
  color: var(--color-text-weak);
  margin: 0 0 12px 0;
  line-height: 1.4;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.card-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 12px;
  
  .card-badges {
    display: flex;
    align-items: center;
    gap: 8px;
    flex-wrap: wrap;
  }
  
  .card-badge {
    display: flex;
    align-items: center;
    gap: 4px;
    font-size: 12px;
    color: var(--color-text-weak);
    background: rgba(0, 0, 0, 0.04);
    padding: 2px 6px;
    border-radius: 4px;
    
    &.due-date {
      color: var(--color-orange-600);
      background: rgba(var(--color-orange-rgb), 0.1);
    }
    
    &.comments {
      color: var(--color-blue-600);
      background: rgba(var(--color-blue-rgb), 0.1);
    }
    
    &.attachments {
      color: var(--color-purple-600);
      background: rgba(var(--color-purple-rgb), 0.1);
    }
    
    &.checklist {
      color: var(--color-green-600);
      background: rgba(var(--color-green-rgb), 0.1);
    }
  }
  
  .card-avatars {
    margin-left: auto;
  }
}

@media (max-width: 768px) {
  .board-card {
    :deep(.ant-card-body) {
      padding: 12px;
    }
  }
  
  .card-title {
    font-size: 13px;
  }
  
  .card-description {
    font-size: 12px;
  }
  
  .card-footer {
    .card-badges {
      gap: 6px;
    }
    
    .card-badge {
      font-size: 11px;
      padding: 1px 4px;
    }
  }
}
</style>