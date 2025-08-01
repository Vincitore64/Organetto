<script lang="ts" setup>
import type { SpinProps } from 'ant-design-vue/es/spin'
import { LoadingOutlined } from '@ant-design/icons-vue'
import { h } from 'vue'

const props = defineProps<SpinProps>()

const indicator = h(LoadingOutlined, {
  style: {
    fontSize: '24px',
  },
  spin: true,
});

</script>
<template>
  <a-spin v-bind="props" :indicator="indicator">
    <slot />
  </a-spin>
</template>
<style lang="scss" scoped>
.ant-spin-nested-loading {
  min-height: 0;
  height: 100%;
  :deep(.ant-spin-container) {
    height: 100%;
    min-height: 0;
  }
}
</style>