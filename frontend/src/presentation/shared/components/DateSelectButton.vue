<script setup lang="ts">
import { ref, computed } from 'vue'
import dayjs, { Dayjs } from 'dayjs'
import { useVModel } from '@vueuse/core'

// Props: allow initial date override
const props = withDefaults(
  defineProps<{ modelValue?: Dayjs | null }>(),
  {
    modelValue: null,
  }
)

const modelValue = useVModel(props, 'modelValue')

// State: popover visibility and selected date
const open = ref(false)

// Display selected date in YYYY-MM-DD format
const formattedDate = computed(() => modelValue.value?.format('YYYY-MM-DD') || 'Select Date')

// Toggle popover visibility
function toggle() {
  open.value = !open.value
}

// Handle date change: update and close popover
function onChange(date: Dayjs) {
  // modelValue.value = date
  open.value = false
}
</script>

<template>
  <main class="date-select-button">
    <a-popover
      v-model:visible="open"
      trigger="click"
      placement="bottom"
    >
      <template #content>
        <a-date-picker
          v-model:value="modelValue"
          @change="onChange"
          allowClear
        />
      </template>
      <section @click="toggle">
        <slot>
          <a-button>
            {{ formattedDate }}
          </a-button>
        </slot>
      </section>
    </a-popover>
  </main>
</template>

<style lang="scss" scoped>
.date-select-button {
  display: grid;
}
</style>
