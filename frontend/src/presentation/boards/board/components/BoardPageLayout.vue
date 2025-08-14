<script setup lang="ts">
import { ref, computed, shallowRef, nextTick } from 'vue'
import { useMouse } from '@vueuse/core'
import BoardHeader from './BoardHeader.vue'
import BoardColumn from './BoardColumn.vue'
import AddListCard from './AddListCard.vue'
import CardModal from './CardModal.vue'
import FilterPanel from './FilterPanel.vue'
import DragGhost from './DragGhost.vue'
import type { BoardVm, CardVm, ColumnVm } from '@/application'
import _ from 'lodash'
import { useVModelFields } from '@/presentation/shared/hooks/useVModelFields'
import { updateCollectionItem } from '@/shared'
import { useDnd } from '@/application/shared/dnd/hooks/useDnd'
import draggable from 'vuedraggable'
import { useKanbanDnd } from '@/application/shared/dnd/hooks/useKanbanDnd'

interface Props {
  board: BoardVm
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'update:board', v: BoardVm): void
}>()

// const board = useVModel(props, 'board')
const boardFields = useVModelFields<BoardVm, keyof BoardVm, 'update:board', Props>(props, 'board', emit)
const { x: mouseX, y: mouseY } = useMouse()
const listsContainerRef = ref<HTMLElement>()

const activeCard = ref<CardVm | null>(null)
const activeList = ref<ColumnVm | null>(null)
const selectedCard = ref<CardVm | null>(null)
const listOfSelectedCard = ref<ColumnVm | null>(null)
const showFilters = ref(false)

const boardColumns = computed({
  get() {
    return boardFields.columns.value
    // return _(boardFields.columns.value).sortBy(c => c.position).value()
  },
  set(v) {
    // debugger
    console.log('boardColumns set', v)
    console.trace('boardColumns set caller')
    boardFields.columns.value = v
  }
})

// const dnd = useDnd(boardColumns, () => props.board.id);
const dnd = useKanbanDnd(boardColumns, { // { value: boardColumns.value }
  debounceMs: 400,
})
const {
  columnDraggableBind,
  cardDraggableBind,
  cardListAttrs
} = dnd
// const {
//   registerContainer,
//   registerColumnEl,
//   registerCardEls,
//   onPointerDown,
//   state,
//   ghostStyle,
// } = dnd;


const newBoardColumnPosition = computed(() => {
  const last = _(boardColumns.value).last()
  return last?.position ? last.position + 1 : boardColumns.value.length
})

// const { handleDragStart: handleListDragStart, handleDragEnd: handleListDragEnd } = useBoardDrag()
// const { handleCardDragStart, handleCardDragEnd } = useCardDrag()

const dragOverlayStyle = computed<Record<string, any>>(() => ({
  position: 'fixed',
  left: `${mouseX.value}px`,
  top: `${mouseY.value}px`,
  pointerEvents: 'none',
  zIndex: 1000,
  transform: 'translate(-50%, -50%)'
}))

function onCreated(c: ColumnVm) {
  debugger
  const updatedColumns = [...boardColumns.value, c]
  boardColumns.value = updatedColumns
}

function onUpdated(c: ColumnVm) {
  debugger
  updateCollectionItem(boardColumns,
    col => col.id === c.id,
    colClone => c)
}

function onDelete(c: ColumnVm) {
  debugger
  const updatedColumns = boardColumns.value.filter(col => col.id !== c.id)
  boardColumns.value = updatedColumns
}

function onCardCreated(c: CardVm, list: ColumnVm) {
  debugger
  updateCollectionItem(boardColumns,
    col => col.id === list.id,
    colClone => {
      colClone.cards.push(c)
    })
}

function onCardDeleted(c: CardVm, list: ColumnVm) {
  debugger
  updateCollectionItem(boardColumns,
    col => col.id === list.id,
    colClone => {
      const idx = colClone.cards.findIndex(x => x.id === c.id)
      if (idx >= 0) colClone.cards.splice(idx, 1)
    })
}

function onCardUpdated(c: CardVm, list: ColumnVm) {
  debugger
  updateCollectionItem(boardColumns,
    col => col.id === list.id,
    colClone => {
      const idx = colClone.cards.findIndex(x => x.id === c.id)
      if (idx >= 0) colClone.cards[idx] = c
    })
}

const toggleFilters = () => {
  showFilters.value = !showFilters.value
}

const handleCardClick = (card: CardVm, column: ColumnVm) => {
  selectedCard.value = card
  listOfSelectedCard.value = column
}

const handleCardModalClose = () => {
  selectedCard.value = null
  listOfSelectedCard.value = null
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

<template>
  <a-layout class="board-page-layout">
    <BoardHeader 
      :board="board"
      :show-filters="showFilters"
      @toggle-filters="toggleFilters"
    />
    
    <FilterPanel v-if="showFilters" @filter-change="handleFilterChange" />
    
    <a-layout-content class="board-main">
      <div class="board-content">
        <div 
          class="lists-container"
        >
          <draggable v-bind="columnDraggableBind">
            <template #item="{ element: list }">
              <BoardColumn
                :list="list"
                :dnd="dnd"
                :board-id="board.id"
                @card-click="(card) => handleCardClick(card, list)"
                @column-updated="onUpdated"
                @column-deleted="onDelete"
                @card-created="onCardCreated"
                @card-deleted="onCardDeleted"
              />
              
            </template>
          </draggable>
          <!-- <section>
                <BoardColumn
                  :list="list"
                  :board-id="board.id"
                  :draggable="true"
                  @card-click="(card) => handleCardClick(card, list)"
                  @column-updated="onUpdated"
                  @column-deleted="onDelete"
                  @card-created="onCardCreated"
                  @card-deleted="onCardDeleted"
                />
              </section> -->
          <!-- <section
            v-for="(list, i) in boardColumns"
            :key="list.id"
          >
            <BoardColumn
              :list="list"
              :board-id="board.id"
              @card-click="(card) => handleCardClick(card, list)"
              @column-updated="onUpdated"
              @column-deleted="onDelete"
              @card-created="onCardCreated"
              @card-deleted="onCardDeleted"
            />
          </section> -->
          <!-- <AddListCard :board-id="board.id" :position="newBoardColumnPosition" @created="onCreated"/> -->
        </div>
      </div>
    </a-layout-content>
    
    <!-- Drag overlay for visual feedback -->
    <!-- <div 
      v-if="activeCard || activeList"
      class="drag-overlay"
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
    </div> -->

    <CardModal
      v-if="selectedCard && listOfSelectedCard"
      v-model:card="selectedCard"
      :column-id="listOfSelectedCard.id"
      :list-name="listOfSelectedCard.title"
      :visible="selectedCard != null"
      @card-updated="(c) => onCardUpdated(c, listOfSelectedCard!)"
      @close="handleCardModalClose"
    />
    
    <!-- Screen reader announcements for drag operations -->
    <div id="drag-announcements" class="sr-only" aria-live="polite" />
  </a-layout>
</template>

<style scoped lang="scss">
// Mirror the boards overview page by switching from a dark purple gradient to the
// light application background.  A secondary gradient layer provides subtle
// texture without overpowering the content.
.board-page-layout {
  min-height: 100vh;
  // background: var(--color-bg-gradient);
  background: transparent;
  position: relative;

  // &::before {
  //   content: '';
  //   position: absolute;
  //   top: 0;
  //   left: 0;
  //   right: 0;
  //   bottom: 0;
  //   background: var(--color-bg-gradient-2);
  //   pointer-events: none;
  // }
}

.board-main {
  padding: 1rem;
  overflow: hidden;
  position: relative;
  background: transparent;
}

.board-content {
  height: calc(100vh - 120px);
  overflow-x: auto;
  overflow-y: hidden;
  padding-bottom: 1rem;
  
  // Custom scrollbar styling
  &::-webkit-scrollbar {
    height: 12px;
  }
  
  &::-webkit-scrollbar-track {
    background: rgba(255, 255, 255, 0.1);
    border-radius: 6px;
  }
  
  &::-webkit-scrollbar-thumb {
    background: rgba(255, 255, 255, 0.3);
    border-radius: 6px;
    transition: background-color 0.2s ease;
    
    &:hover {
      background: rgba(255, 255, 255, 0.5);
    }
  }
}

.lists-container {
  display: flex;
  gap: 1rem;
  height: 100%;
  min-width: min-content;
  padding: 0.5rem;
  align-items: flex-start;
}

.drag-overlay {
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

// Responsive adjustments
@media (max-width: 768px) {
  .board-main {
    padding: 0.5rem;
  }
  
  .lists-container {
    gap: 0.75rem;
    padding: 0.25rem;
  }
  
  .board-content {
    height: calc(100vh - 100px);
  }
}

@media (max-width: 480px) {
  .board-main {
    padding: 0.25rem;
  }
  
  .lists-container {
    gap: 0.5rem;
    padding: 0.125rem;
  }
}
</style>