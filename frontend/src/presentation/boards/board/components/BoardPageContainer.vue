<template>
  <main v-if="isLoading" class="min-h-screen flex items-center justify-center">
    <div class="text-lg text-gray-600">{{ t('board.loading') }}</div>
  </main>
  
  <main v-else-if="!activeBoard" class="min-h-screen flex items-center justify-center">
    <div class="text-lg text-gray-600">{{ t('board.notFound') }}</div>
  </main>
  
  <BoardPageLayout v-else :board="activeBoard" />
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