<template>
  <a-modal v-model:open="open" :footer="null" @cancel="$emit('close')" :width="width" wrapClassName="item-modal-wrap">
    <template #title>
      <header class="item-modal-title">
        {{ title }}
        <a-divider class="item-modal-title-divider"></a-divider>
        <div class="item-modal-title-icon" v-if="iconUrl">
          <img :src="iconUrl" alt="">
        </div>
      </header>
    </template>
    <slot></slot>
    <section class="item-modal-description" v-if="description">
      {{ description }}
    </section>
  </a-modal>
</template>

<script setup lang="ts">
import { useVModel } from '@vueuse/core'

const props = defineProps<{
  open: boolean
  title: string
  description?: string
  iconUrl?: string
  width?: string
  wrapClassName?: string
}>()

const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'update:open', value: boolean): void
}>()
const open = useVModel(props, 'open', emit)

</script>

<style scoped lang="scss">
// .modal-title-header {
//   position: relative;
//   padding-bottom: 16px;
//   text-align: center;
//   font-size: 18px;
//   font-weight: 600;
// }

// .modal-title-divider {
//   margin: 16px 0 0;
// }

// .modal-title-icon {
//   position: absolute;
//   top: calc(100% - 16px);
//   left: 50%;
//   transform: translateX(-50%);
//   background: white;
//   padding: 4px;
//   border-radius: 4px;
//   line-height: 0;
// }

// .modal-description {
//   margin-top: 16px;
//   font-size: 14px;
//   color: var(--color-text-weak);
//   text-align: center;
// }</style>