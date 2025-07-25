<template>
  <div v-if="isLoading" class="min-h-screen flex items-center justify-center">
    <div class="text-lg text-gray-600">{{ $t('board.loading') }}</div>
  </div>
  
  <div v-else-if="!activeBoard" class="min-h-screen flex items-center justify-center">
    <div class="text-lg text-gray-600">{{ $t('board.notFound') }}</div>
  </div>
  
  <BoardPageLayout v-else :board="activeBoard" :lists="lists" />
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { storeToRefs } from 'pinia'
import BoardPageLayout from './BoardPageLayout.vue'
import { useBoardStore } from '../../stores/boardStore'
import { useListStore } from '../../stores/listStore'
import { useBoardHub } from '../../hooks/useBoardHub'
import { useConnectionStatus } from '../../hooks/useConnectionStatus'

interface Props {
  boardId: string
}

const props = defineProps<Props>()
const { t } = useI18n()

const boardStore = useBoardStore()
const listStore = useListStore()
const { isConnected } = useConnectionStatus()
const { joinBoardRoom, leaveBoardRoom } = useBoardHub()

const { activeBoard, isLoading } = storeToRefs(boardStore)
const { lists } = storeToRefs(listStore)

// Load board data when component mounts or boardId changes
watch(
  () => props.boardId,
  async (newBoardId) => {
    if (newBoardId) {
      await boardStore.loadBoard(newBoardId)
      await listStore.loadLists(newBoardId)
    }
  },
  { immediate: true }
)

// Real-time collaboration: Join board room when connected
watch(
  [() => isConnected.value, () => props.boardId],
  ([connected, boardId]) => {
    if (connected && boardId) {
      joinBoardRoom(boardId)
    }
  },
  { immediate: true }
)

onMounted(() => {
  // Initial load
  if (props.boardId) {
    boardStore.loadBoard(props.boardId)
    listStore.loadLists(props.boardId)
  }
})

onUnmounted(() => {
  if (props.boardId) {
    leaveBoardRoom(props.boardId)
  }
})
</script>