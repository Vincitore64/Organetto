<script setup lang="ts">
import BoardPageContainer from '@/presentation/boards/board/components/BoardPageContainer.vue'

const props = defineProps<{
  id: string,
  userId: string,
}>()


</script>
<template>
  <main class="board-page__wrapper">
    <BoardPageContainer :board-id="id" />
  </main>
</template>
<style lang="scss" scoped>
.board-page__wrapper {
  display: grid;
}
</style>