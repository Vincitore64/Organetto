<template>
  <FeatureCard
    :title="t('boards.page.create')"
    :description="t('boards.createCard.description')"
    :icon="PlusOutlined"
    :clickable="true"
    :show-cover="true"
    :show-floating-icons="true"
    cover-variant="primary"
    class="create-board-card"
    @click="$emit('create')"
  >
    <template #footer>
      <div class="create-features">
        <div class="feature">
          <CheckOutlined class="feature-icon" />
          <span>{{ t('boards.createCard.templates') }}</span>
        </div>
        <div class="feature">
          <CheckOutlined class="feature-icon" />
          <span>{{ t('boards.createCard.collaboration') }}</span>
        </div>
      </div>
    </template>
  </FeatureCard>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import {
  PlusOutlined,
  CheckOutlined
} from '@ant-design/icons-vue'
import FeatureCard from '@/presentation/shared/components/FeatureCard.vue'

const { t } = useI18n()

defineEmits<{ (e: 'create'): void }>()
</script>

<style scoped lang="scss">
.create-board-card {
  // Custom styling for the create board card
  // border: 2px dashed var(--color-primary-300, #91d5ff);
  min-height: 320px;
  
  // &:hover {
  //   // border-color: var(--color-primary-500, #1890ff);
    
  //   // :deep(.main-icon) {
  //   //   transform: scale(1.1) rotate(90deg) !important;
  //   // }
  // }
}

.create-features {
  display: flex;
  justify-content: center;
  gap: 16px;
  margin-top: 8px;

  .feature {
    display: flex;
    align-items: center;
    gap: 4px;
    font-size: 14px;
    color: var(--color-success, #52c41a);

    .feature-icon {
      font-size: 14px;
    }
  }
}

// Responsive adjustments
@media (max-width: 768px) {
  .create-features {
    flex-direction: column;
    gap: 8px;
    align-items: center;
  }
}
</style>