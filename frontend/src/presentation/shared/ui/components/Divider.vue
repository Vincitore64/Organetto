<script lang="ts" setup>
import type { DividerProps } from 'ant-design-vue/es/divider'
const props = defineProps<DividerProps>()
</script>
<template>
  <a-divider v-bind="props" class="mb-4 mt-1"></a-divider>
</template>