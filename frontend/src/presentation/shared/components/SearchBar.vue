<template>
  <a-input-search v-model:value="vModel" :placeholder="t('mainLayout.search')" enter-button
    @search="emit('search', modelValue)" />
</template>
<script setup lang="ts">
import { useVModel } from '@vueuse/core'
import { defineProps } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const props = defineProps<{ modelValue: string }>()
const emit = defineEmits<{ (e: 'search', value: string): void, (e: 'update:modelValue', value: string): void }>()

const vModel = useVModel(props, 'modelValue', emit)
</script>