<template>
  <div class="add-card-container">
    <a-button
      v-if="!isAdding"
      type="text"
      block
      class="add-card-button"
      @click="startAdding"
    >
      <template #icon>
        <PlusOutlined />
      </template>
      {{ t('board.addCard.placeholder') }}
    </a-button>
    
    <div v-else class="add-card-form glass">
      <a-textarea
        ref="textareaRef"
        v-model:value="title"
        :placeholder="t('board.addCard.placeholder')"
        :auto-size="{ minRows: 2, maxRows: 4 }"
        class="card-textarea"
        @keydown.enter.prevent="handleSubmit"
        @keydown.esc="handleCancel"
      />
      
      <div class="form-actions">
        <a-button
          type="primary"
          size="small"
          class="submit-button"
          @click="handleSubmit"
        >
          {{ t('board.addCard.add') }}
        </a-button>
        
        <a-button
          type="text"
          size="small"
          class="cancel-button"
          @click="handleCancel"
        >
          <template #icon>
            <CloseOutlined />
          </template>
        </a-button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, nextTick } from 'vue'
import { PlusOutlined, CloseOutlined } from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'


interface Props {
  listId: string
}

const props = defineProps<Props>()
const { t } = useI18n()

const isAdding = ref(false)
const title = ref('')
const textareaRef = ref<HTMLTextAreaElement>()

const handleSubmit = async () => {
  if (title.value.trim()) {
    // await listStore.createCard({ 
    //   listId: props.listId, 
    //   title: title.value.trim() 
    // })
    title.value = ''
    isAdding.value = false
  }
}

const handleCancel = () => {
  title.value = ''
  isAdding.value = false
}

const startAdding = async () => {
  isAdding.value = true
  await nextTick()
  textareaRef.value?.focus()
}
</script>

<style scoped lang="scss">
.add-card-container {
  width: 100%;
}

.add-card-button {
  background: rgba(0, 0, 0, 0.02);
  border: 2px dashed rgba(0, 0, 0, 0.1);
  border-radius: 8px;
  color: var(--color-text-weak);
  font-size: 14px;
  font-weight: 500;
  height: 40px;
  transition: var(--transition-smooth);
  
  &:hover {
    background: rgba(var(--color-primary-rgb), 0.04);
    border-color: rgba(var(--color-primary-rgb), 0.2);
    color: var(--color-primary-600);
    transform: translateY(-1px);
  }
  
  &:focus {
    background: rgba(var(--color-primary-rgb), 0.04);
    border-color: var(--color-primary-400);
    color: var(--color-primary-600);
  }
}

.add-card-form {
  background: rgba(255, 255, 255, 0.98);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 8px;
  padding: 16px;
  box-shadow: var(--shadow-light);
  
  .card-textarea {
    background: rgba(255, 255, 255, 0.8);
    border: 1px solid rgba(0, 0, 0, 0.06);
    border-radius: 6px;
    font-size: 14px;
    transition: var(--transition-smooth);
    
    &:focus {
      border-color: var(--color-primary-400);
      box-shadow: 0 0 0 2px rgba(var(--color-primary-rgb), 0.1);
    }
    
    :deep(.ant-input) {
      background: transparent;
      border: none;
      box-shadow: none;
      
      &:focus {
        box-shadow: none;
      }
    }
  }
  
  .form-actions {
    display: flex;
    align-items: center;
    gap: 8px;
    margin-top: 12px;
    
    .submit-button {
      background: var(--color-blue-gradient);
      border: none;
      border-radius: 6px;
      font-size: 13px;
      font-weight: 500;
      box-shadow: var(--shadow-light);
      transition: var(--transition-smooth);
      
      &:hover {
        transform: translateY(-1px);
        box-shadow: var(--shadow-medium);
      }
    }
    
    .cancel-button {
      color: var(--color-text-weak);
      border-radius: 6px;
      transition: var(--transition-smooth);
      
      &:hover {
        color: var(--color-text);
        background: rgba(0, 0, 0, 0.04);
      }
    }
  }
}

@media (max-width: 768px) {
  .add-card-button {
    font-size: 13px;
    height: 36px;
  }
  
  .add-card-form {
    padding: 12px;
    
    .card-textarea {
      font-size: 13px;
    }
    
    .form-actions {
      margin-top: 8px;
      
      .submit-button {
        font-size: 12px;
      }
    }
  }
}
</style>