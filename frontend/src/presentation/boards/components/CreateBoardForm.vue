<template>
  <a-form layout="vertical"><!-- :rules="formInstance.rulesRef" @finish="onFinish" -->
    <a-form-item :label="t('boards.createForm.nameLabel')" name="name" v-bind="formInstance.validateInfos.name">
      <a-input v-model:value="name" :placeholder="t('boards.createForm.namePlaceholder')" />
    </a-form-item>
    <a-form-item :label="t('boards.createForm.descriptionLabel')" name="description"
      v-bind="formInstance.validateInfos.description">
      <a-textarea v-model:value="description" :placeholder="t('boards.createForm.descriptionPlaceholder')" :rows="4" />
    </a-form-item>
    <a-form-item>
      <a-button type="primary" :loading="loading" block @click.prevent="onFinish"><!-- html-type="submit"  -->
        {{ t('boards.createForm.submit') }}
      </a-button>
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
import type { CreateBoardState } from '@/application'
// import { reactive } from 'vue'
import { useI18n } from 'vue-i18n'
import type { FormInstance } from '@/presentation/shared'
import { useVModelFields } from '@/presentation/shared/hooks/useVModelFields'


const props = defineProps<{ modelValue: CreateBoardState, formInstance: FormInstance, loading?: boolean }>()

const emit = defineEmits<{ (e: 'submit', values: CreateBoardState): void, (e: 'update:modelValue', value: CreateBoardState): void }>()

const { t } = useI18n()

const { name, description } = useVModelFields(props, emit)

const onFinish = (values: unknown) => {
  emit('submit', values as CreateBoardState)
}
</script>

<style scoped lang="scss">
// Add any specific styles for the form if needed</style>