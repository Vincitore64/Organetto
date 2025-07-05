<template>
  <a-form :model="formState" layout="vertical" @finish="onFinish">
    <a-form-item :label="t('boards.createForm.nameLabel')" name="name"
      :rules="[{ required: true, message: t('boards.createForm.nameRequired') }]">
      <a-input v-model:value="formState.name" :placeholder="t('boards.createForm.namePlaceholder')" />
    </a-form-item>
    <a-form-item :label="t('boards.createForm.descriptionLabel')" name="description">
      <a-textarea v-model:value="formState.description" :placeholder="t('boards.createForm.descriptionPlaceholder')"
        :rows="4" />
    </a-form-item>
    <a-form-item>
      <a-button type="primary" html-type="submit" :loading="loading" block>
        {{ t('boards.createForm.submit') }}
      </a-button>
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
import type { CreateBoardState } from '@/application'
import { reactive } from 'vue'
import { useI18n } from 'vue-i18n'


defineProps<{ loading?: boolean }>()

const emit = defineEmits<{ (e: 'submit', values: CreateBoardState): void }>()

const { t } = useI18n()

const formState = reactive<CreateBoardState>({
  name: '',
  description: '',
})

const onFinish = (values: unknown) => {
  emit('submit', values as CreateBoardState)
}
</script>

<style scoped lang="scss">
// Add any specific styles for the form if needed</style>