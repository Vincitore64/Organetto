<template>
  <a-layout class="board-page-container image-bg">
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
import { useBoardDetail } from '@/application/boards/hooks/useBoardCrud'
// import { useBoardStore } from '../../stores/boardStore'
// import { useListStore } from '../../stores/listStore'
// import { useBoardHub } from '../../hooks/useBoardHub'
// import { useConnectionStatus } from '../../hooks/useConnectionStatus'

const props = defineProps<{
  boardId: string
}>()

const client = tryInjectServices().resolve(ApiClient)

const { t } = useI18n()

const { isLoading, data: activeBoard } = useBoardDetail(props.boardId)

// const { state: activeBoard, isLoading, execute } = useApiState<ApiClient, BoardDetailedDto, [number]>(client)
//   (client => client.boards.getById.bind(client.boards))
//   (_.parseInt(props.boardId))
//   ()

// execute()

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
// The board container adopts the same soft background gradient as the boards
// overview page.  A subtle grain overlay is retained for texture, using the
// secondary gradient defined in the global colour palette.
.board-page-container {
  min-height: 100vh;
  // background: var(--color-bg-gradient);
  background-position-y: top;
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

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  gap: 1rem;
  font-family: 'Sofia Sans Extra Condensed', sans-serif;
  
  .loading-text {
    color: var(--color-text);
    font-size: 1.75rem;
    font-weight: 500;
    margin-top: 1rem;
  }
  
  :deep(.ant-spin-dot) {
    i {
      // background-color: rgba(255, 255, 255, 0.8);
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