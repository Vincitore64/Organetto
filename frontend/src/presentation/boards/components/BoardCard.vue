<template>
  <a-card hoverable :style="{ background: 'var(--color-surface)', borderColor: 'var(--color-stroke)' }"
    @click="$emit('open', board.id)">
    <template #cover>
      <img :src='board.thumbnailUrl' alt='thumb' />
    </template>
    <a-card-meta :title="board.title" />
  </a-card>
</template>
<script setup lang="ts">
interface Board { id: number; title: string; thumbnailUrl: string }
const props = defineProps<{ board: Board }>()
</script>