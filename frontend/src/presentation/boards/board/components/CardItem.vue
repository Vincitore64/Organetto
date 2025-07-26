<template>
  <article
    ref="cardRef"
    :class="[styles.card, { [styles.dragging]: isDragging }]"
    :style="dragStyle"
    @click="handleClick"
    @mousedown="startDrag"
    @touchstart="startDrag"
  >
    <!-- Card labels -->
    <section v-if="card.labels.length > 0" :class="styles.labels">
      <span
        v-for="(label, index) in card.labels"
        :key="index"
        :class="[styles.label, styles[`label-${index % 6}`]]"
      >
        {{ label }}
      </span>
    </section>
    
    <header>
      <h4 :class="styles.title">{{ card.title }}</h4>
    </header>
    
    <main v-if="card.description" :class="styles.description">
      {{ card.description }}
    </main>
    
    <footer :class="styles.footer">
      <div :class="styles.badges">
        <div v-if="card.dueDate" :class="styles.badge">
          <CalendarOutlined />
          <span>{{ formatDate(card.dueDate) }}</span>
        </div>
        
        <div v-if="hasComments" :class="styles.badge">
          <MessageOutlined />
          <span>{{ commentCount }}</span>
        </div>
        
        <div v-if="hasAttachments" :class="styles.badge">
          <PaperClipOutlined />
          <span>{{ attachmentCount }}</span>
        </div>
        
        <div v-if="hasChecklist" :class="styles.badge">
          <CheckSquareOutlined />
          <span>{{ checklistProgress }}</span>
        </div>
      </div>
      
      <AvatarGroup
        v-if="mockUsers.length > 0"
        :users="mockUsers"
        :max-visible="3"
        size="sm"
      />
    </footer>
  </article>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useDraggable } from '@vueuse/core'
import {
  CalendarOutlined,
  MessageOutlined,
  PaperClipOutlined,
  CheckSquareOutlined
} from '@ant-design/icons-vue'
import dayjs from 'dayjs'
import { Card } from '../../types/board'
import AvatarGroup from './AvatarGroup.vue'
import styles from './CardItem.module.scss'

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

<style module="styles" lang="scss">
@use './CardItem.module.scss';
</style>