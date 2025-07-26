<template>
  <main :class="styles.container">
    <BoardHeader 
      :board="board"
      :show-filters="showFilters"
      @toggle-filters="toggleFilters"
    />
    
    <FilterPanel v-if="showFilters" @filter-change="handleFilterChange" />
    
    <main :class="styles.main">
      <section :class="styles.boardContent">
        <div 
          ref="listsContainerRef"
          :class="styles.listsContainer"
        >
          <BoardColumn
            v-for="list in board.columns"
            :key="list.id"
            :list="list"
            :draggable="true"
            @card-click="handleCardClick"
          />
          <AddListCard :board-id="board.id" />
        </div>
      </section>
    </main>
    
    <!-- Drag overlay for visual feedback -->
    <div 
      v-if="activeCard || activeList"
      :class="styles.dragOverlay"
      :style="dragOverlayStyle"
    >
      <DragGhost
        v-if="activeCard"
        type="card"
        :item="activeCard"
      />
      <DragGhost
        v-if="activeList"
        type="list"
        :item="activeList"
      />
    </div>

    <CardModal 
      v-if="selectedCard"
      :card="selectedCard"
      @close="handleCardModalClose"
    />
    
    <!-- Screen reader announcements for drag operations -->
    <div id="drag-announcements" class="sr-only" aria-live="polite" />
  </main>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useMouse } from '@vueuse/core'
import BoardHeader from './BoardHeader.vue'
import BoardColumn from './BoardColumn.vue'
import AddListCard from './AddListCard.vue'
import CardModal from './CardModal.vue'
import FilterPanel from './FilterPanel.vue'
import DragGhost from './DragGhost.vue'
// import { useBoardDrag } from '../../hooks/useBoardDrag'
// import { useCardDrag } from '../../hooks/useCardDrag'
import styles from './BoardPageLayout.module.scss'
import type { BoardDetailedDto } from '@/dataAccess/boards/models'
import type { CardDto } from '@/dataAccess/cards/models'
import type { ColumnDto } from '@/dataAccess/columns/models'

interface Props {
  board: BoardDetailedDto
}

const props = defineProps<Props>()

const { x: mouseX, y: mouseY } = useMouse()
const listsContainerRef = ref<HTMLElement>()

const activeCard = ref<CardDto | null>(null)
const activeList = ref<ColumnDto | null>(null)
const selectedCard = ref<CardDto | null>(null)
const showFilters = ref(false)

// const { handleDragStart: handleListDragStart, handleDragEnd: handleListDragEnd } = useBoardDrag()
// const { handleCardDragStart, handleCardDragEnd } = useCardDrag()

const dragOverlayStyle = computed(() => ({
  position: 'fixed',
  left: `${mouseX.value}px`,
  top: `${mouseY.value}px`,
  pointerEvents: 'none',
  zIndex: 1000,
  transform: 'translate(-50%, -50%)'
}))

const toggleFilters = () => {
  showFilters.value = !showFilters.value
}

const handleCardClick = (card: CardDto) => {
  selectedCard.value = card
}

const handleCardModalClose = () => {
  selectedCard.value = null
}

const handleFilterChange = (filters: any) => {
  // Handle filter changes
  console.log('Filters changed:', filters)
}

// const handleDragOver = (event: DragEvent) => {
//   event.preventDefault()
// }

// const handleDrop = (event: DragEvent) => {
//   event.preventDefault()
  
//   const dragType = event.dataTransfer?.getData('text/plain')
  
//   if (dragType === 'card') {
//     handleCardDragEnd(event)
//     activeCard.value = null
//   } else if (dragType === 'list') {
//     handleListDragEnd(event)
//     activeList.value = null
//   }
// }

// // Handle list drag events
// const onListDragStart = (event: DragEvent, list: List) => {
//   if (event.dataTransfer) {
//     event.dataTransfer.setData('text/plain', 'list')
//     event.dataTransfer.setData('application/json', JSON.stringify(list))
//   }
//   activeList.value = list
//   handleListDragStart(event)
// }

// const onListDragEnd = (event: DragEvent) => {
//   activeList.value = null
//   handleListDragEnd(event)
// }

// // Handle card drag events
// const onCardDragStart = (event: DragEvent, card: Card) => {
//   if (event.dataTransfer) {
//     event.dataTransfer.setData('text/plain', 'card')
//     event.dataTransfer.setData('application/json', JSON.stringify(card))
//   }
//   activeCard.value = card
//   handleCardDragStart(event)
// }

// const onCardDragEnd = (event: DragEvent) => {
//   activeCard.value = null
//   handleCardDragEnd(event)
// }
</script>

<style module="styles" lang="scss">
@use './BoardPageLayout.module.scss';

.dragOverlay {
  position: fixed;
  pointer-events: none;
  z-index: 1000;
  transform: translate(-50%, -50%);
}

.sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0);
  white-space: nowrap;
  border: 0;
}
</style>