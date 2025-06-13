<template>
  <a-card hoverable class="create-card" :body-style="{ height: '100%', display: 'grid', alignItems: 'center' }"
    :style="{ background: 'var(--color-surface)', borderColor: 'var(--color-stroke)' }" @click="$emit('create')">
    <!-- <template #cover>
      <div class="placeholder" />
    </template> -->
    <div class="create-body">
      <plus-outlined class="icon" />
      <div>{{ t('boards.page.create') }}</div>
    </div>
  </a-card>
</template>
<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { PlusOutlined } from '@ant-design/icons-vue'
const { t } = useI18n()
</script>
<style scoped lang="scss">
.placeholder {
  height: 120px;
  background: var(--color-primary-50);
}

.create-card {
  border: 1px dashed var(--color-stroke);
  transition: border-color .2s ease;
}

.create-card:hover {
  border-color: var(--color-primary-500);
}

.create-body {
  text-align: center;
  padding: 16px 0;
  color: var(--color-text-weak);

  >div {
    font-size: 18px;
  }
}

.icon {
  font-size: 32px;
  margin-bottom: 8px;
  color: var(--color-primary-500);
}
</style>