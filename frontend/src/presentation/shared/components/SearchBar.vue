<template>
  <a-input-search v-model:value="modelValue" :placeholder="t('mainLayout.search')" enter-button
    @search="$emit('search', modelValue)" :style="{ background: 'var(--color-surface)' }" />
</template>
<script setup lang="ts">
import { defineProps, defineEmits, toRef } from 'vue'
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
const props = defineProps<{ modelValue: string }>()
// const emit = defineEmits<['update:modelValue', 'search']>()
const emit = defineEmits<{ (e: 'search', value: string): void, (e: 'update:modelValue', value: string): void }>()
const modelValue = toRef(props, 'modelValue')
</script>