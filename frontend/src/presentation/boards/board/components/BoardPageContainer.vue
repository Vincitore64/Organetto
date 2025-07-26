<template>
  <a-layout class="board-page-container">
    <div v-if="isLoading" class="loading-container">
      <a-spin size="large" />
      <div class="loading-text">{{ t('board.loading') }}</div>
    </div>
    
    <div v-else-if="!activeBoard" class="not-found-container">
      <div class="not-found-content">
        <div class="not-found-text">{{ t('board.notFound') }}</div>
      </div>
    </div>
    
    <BoardPageLayout v-else :board="activeBoard" />
  </a-layout>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import BoardPageLayout from './BoardPageLayout.vue'
import { useApiState } from '@/application/shared/hooks/useApiHandler'
import { tryInjectServices } from '@/shared'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import type { BoardDetailedDto } from '@/dataAccess/boards/models'
import _ from 'lodash'
// import { useBoardStore } from '../../stores/boardStore'
// import { useListStore } from '../../stores/listStore'
// import { useBoardHub } from '../../hooks/useBoardHub'
// import { useConnectionStatus } from '../../hooks/useConnectionStatus'

const props = defineProps<{
  boardId: string
}>()

const client = tryInjectServices().resolve(ApiClient)

const { t } = useI18n()

const { state: activeBoard, isLoading, execute } = useApiState<ApiClient, BoardDetailedDto, [number]>(client)
  (client => client.boards.getById.bind(client.boards))
  (_.parseInt(props.boardId))
  ()

execute()

// const boardStore = useBoardStore()
// const listStore = useListStore()
// const { isConnected } = useConnectionStatus()
// const { joinBoardRoom, leaveBoardRoom } = useBoardHub()

// const { activeBoard, isLoading } = storeToRefs(boardStore)
// const { lists } = storeToRefs(listStore)

// Real-time collaboration: Join board room when connected
// watch(
//   [() => isConnected.value, () => props.boardId],
//   ([connected, boardId]) => {
//     if (connected && boardId) {
//       joinBoardRoom(boardId)
//     }
//   },
//   { immediate: true }
// )

// onMounted(() => {
//   // Initial load
//   if (props.boardId) {
//     boardStore.loadBoard(props.boardId)
//     listStore.loadLists(props.boardId)
//   }
// })

// onUnmounted(() => {
//   if (props.boardId) {
//     leaveBoardRoom(props.boardId)
//   }
// })
</script>

<style scoped lang="scss">
.board-page-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  position: relative;
  
  &::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="grain" width="100" height="100" patternUnits="userSpaceOnUse"><circle cx="50" cy="50" r="1" fill="%23ffffff" opacity="0.1"/></pattern></defs><rect width="100" height="100" fill="url(%23grain)"/></svg>') repeat;
    pointer-events: none;
  }
}

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  gap: 1rem;
  
  .loading-text {
    color: rgba(255, 255, 255, 0.9);
    font-size: 1.125rem;
    font-weight: 500;
    margin-top: 1rem;
  }
  
  :deep(.ant-spin-dot) {
    i {
      background-color: rgba(255, 255, 255, 0.8);
    }
  }
}

.not-found-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 2rem;
  
  .not-found-content {
    background: rgba(255, 255, 255, 0.1);
    backdrop-filter: blur(20px);
    border: 1px solid rgba(255, 255, 255, 0.2);
    border-radius: 16px;
    padding: 3rem;
    text-align: center;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
    
    .not-found-text {
      color: rgba(255, 255, 255, 0.9);
      font-size: 1.25rem;
      font-weight: 500;
    }
  }
}

// Responsive adjustments
@media (max-width: 768px) {
  .not-found-content {
    padding: 2rem;
    margin: 1rem;
    
    .not-found-text {
      font-size: 1.125rem;
    }
  }
}
</style>