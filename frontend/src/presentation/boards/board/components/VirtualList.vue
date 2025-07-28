<template>
  <div
    ref="containerRef"
    :class="styles.container"
    :style="{ height: `${containerHeight}px` }"
    @scroll="handleScroll"
  >
    <div
      :class="styles.spacer"
      :style="{ height: `${totalHeight}px` }"
    >
      <div
        :class="styles.visibleItems"
        :style="{
          transform: `translateY(${offsetY}px)`,
          height: `${visibleHeight}px`
        }"
      >
        <div
          v-for="(item, index) in visibleItems"
          :key="item.id"
          :class="styles.item"
          :style="{ height: `${itemHeight}px` }"
        >
          <slot :item="item" :index="startIndex + index" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts" generic="T extends { id: string }">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useElementSize, useScroll } from '@vueuse/core'
import styles from './VirtualList.module.scss'

interface Props {
  items: T[]
  itemHeight: number
  gap: number
  height?: number
  overscan?: number
}

const props = withDefaults(defineProps<Props>(), {
  height: 400,
  gap: 12,
  overscan: 5
})

const containerRef = ref<HTMLElement>()
const { height: elementHeight } = useElementSize(containerRef)
const { y: scrollTop } = useScroll(containerRef)

// Only use virtual scrolling for lists with many items
const useVirtualScrolling = computed(() => props.items.length > 10)

const containerHeight = computed(() => {
  if (!useVirtualScrolling.value) {
    return Math.min(props.height, props.items.length * props.itemHeight)
  }
  return props.height
})

const totalHeight = computed(() => {
  if (!useVirtualScrolling.value) {
    return props.items.length * props.itemHeight + props.items.length * props.gap
  }
  return props.items.length * props.itemHeight + props.items.length * props.gap
})

const visibleCount = computed(() => {
  if (!useVirtualScrolling.value) {
    return props.items.length
  }
  return Math.ceil(containerHeight.value / props.itemHeight) + props.overscan * 2
})

const startIndex = computed(() => {
  if (!useVirtualScrolling.value) {
    return 0
  }
  return Math.max(0, Math.floor(scrollTop.value / props.itemHeight) - props.overscan)
})

const endIndex = computed(() => {
  if (!useVirtualScrolling.value) {
    return props.items.length - 1
  }
  return Math.min(props.items.length - 1, startIndex.value + visibleCount.value - 1)
})

const visibleItems = computed(() => {
  return props.items.slice(startIndex.value, endIndex.value + 1)
})

const offsetY = computed(() => {
  if (!useVirtualScrolling.value) {
    return 0
  }
  return startIndex.value * props.itemHeight
})

const visibleHeight = computed(() => {
  return visibleItems.value.length * props.itemHeight + visibleItems.value.length * props.gap
})

const handleScroll = () => {
  // Scroll handling is managed by useScroll composable
}
</script>

<style module="styles" lang="scss">
.container {
  overflow-y: auto;
  overflow-x: hidden;
  position: relative;
  
  &::-webkit-scrollbar {
    width: 6px;
  }
  
  &::-webkit-scrollbar-track {
    background: transparent;
  }
  
  &::-webkit-scrollbar-thumb {
    background: #cbd5e1;
    border-radius: 3px;
    
    &:hover {
      background: #94a3b8;
    }
  }
}

.spacer {
  position: relative;
  width: 100%;
}

.visibleItems {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
}

.item {
  display: flex;
  align-items: stretch;
  width: 100%;
}
</style>