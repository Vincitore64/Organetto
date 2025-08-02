<script lang="ts" setup>
import type { DividerProps } from 'ant-design-vue/es/divider'
const props = defineProps<DividerProps & {
  top?: boolean,
  bottom?: boolean,
}>()

</script>
<template>
  <a-divider v-bind="props" :class="{'mb-4': bottom, 'mt-1': top, 'mb-0': !bottom, 'mt-0': !top}"></a-divider>
</template>
<style scoped lang="scss">
.mb-4 {
  margin-bottom: 16px;
}
.mt-1 {
  margin-top: 4px;
}
.mb-0 {
  margin-bottom: 0;
}
.mt-0 {
  margin-top: 0;
}
</style>