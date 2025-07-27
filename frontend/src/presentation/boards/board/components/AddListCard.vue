<template>
  <div class="add-list-container">
    <a-button
      v-if="!isAdding"
      type="dashed"
      block
      size="large"
      class="add-list-button"
      @click="startAdding"
    >
      <template #icon>
        <PlusOutlined />
      </template>
      {{ t('board.addList.placeholder') }}
    </a-button>
    
    <div v-else class="add-list-form glass">
      <a-input
        ref="inputRef"
        v-model:value="title"
        :placeholder="t('board.addList.placeholder')"
        class="list-input"
        @keydown.enter="handleSubmit"
        @keydown.esc="handleCancel"
      />
      
      <div class="form-actions">
        <a-button
          type="primary"
          size="small"
          class="submit-button"
          @click="handleSubmit"
        >
          {{ t('board.addList.addButton') }}
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
import { useI18n } from 'vue-i18n'


interface Props {
  boardId: number
}

const props = defineProps<Props>()
const { t } = useI18n()
// const listStore = useListStore()

const isAdding = ref(false)
const title = ref('')
const inputRef = ref<HTMLInputElement>()

const handleSubmit = async (e: Event) => {
  e.preventDefault()
  if (title.value.trim()) {
    // await listStore.createList({ boardId: props.boardId, title: title.value.trim() })
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
  inputRef.value?.focus()
}
</script>

<style scoped lang="scss">
.add-list-container {
  width: 300px;
  min-width: 300px;
  max-width: 300px;
}

// The "Add list" button echoes the styling of the create board card by using a
// coloured dashed border and a light surface.  This helps it blend with the
// overall boards aesthetic.
.add-list-button {
  height: 120px;
  background: rgba(255, 255, 255, 0.7);
  backdrop-filter: blur(20px);
  border: 2px dashed var(--color-primary-300, #91d5ff);
  border-radius: 12px;
  color: var(--color-text-weak);
  font-size: 16px;
  font-weight: 500;
  transition: var(--transition-smooth);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 8px;

  &:hover {
    background: rgba(var(--color-primary-rgb), 0.1);
    border-color: var(--color-primary-400, #69c0ff);
    color: var(--color-primary-600);
    transform: translateY(-2px);
    box-shadow: var(--shadow-medium);
  }

  &:focus {
    background: rgba(var(--color-primary-rgb), 0.1);
    border-color: var(--color-primary-400, #69c0ff);
    color: var(--color-primary-600);
  }

  .anticon {
    font-size: 24px;
  }
}

// The add list form mirrors the visual treatment of other cards: a clean white
// surface with gentle rounding and subtle shadow.  Inputs use a slightly
// translucent background to indicate interactivity and highlight the focus
// state with a coloured outline.
.add-list-form {
  width: 300px;
  min-width: 300px;
  max-width: 300px;
  background: rgba(255, 255, 255, 0.98);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 12px;
  padding: 16px;
  box-shadow: var(--shadow-light);

  .list-input {
    background: rgba(255, 255, 255, 0.9);
    border: 1px solid rgba(0, 0, 0, 0.06);
    border-radius: 8px;
    font-size: 16px;
    font-weight: 500;
    height: 44px;
    transition: var(--transition-smooth);

    &:focus {
      border-color: var(--color-primary-400);
      box-shadow: 0 0 0 2px rgba(var(--color-primary-rgb), 0.1);
    }

    :deep(.ant-input) {
      background: transparent;
      border: none;
      box-shadow: none;
      font-size: 16px;
      font-weight: 500;

      &:focus {
        box-shadow: none;
      }

      &::placeholder {
        color: var(--color-text-weak);
      }
    }
  }

  .form-actions {
    display: flex;
    align-items: center;
    gap: 12px;
    margin-top: 16px;

    .submit-button {
      background: var(--color-blue-gradient);
      border: none;
      border-radius: 8px;
      font-size: 14px;
      font-weight: 500;
      height: 36px;
      box-shadow: var(--shadow-light);
      transition: var(--transition-smooth);

      &:hover {
        transform: translateY(-1px);
        box-shadow: var(--shadow-medium);
      }
    }

    .cancel-button {
      color: var(--color-text-weak);
      border-radius: 8px;
      height: 36px;
      transition: var(--transition-smooth);

      &:hover {
        color: var(--color-text);
        background: rgba(0, 0, 0, 0.04);
      }
    }
  }
}

@media (max-width: 768px) {
  .add-list-container,
  .add-list-form {
    width: 280px;
    min-width: 280px;
    max-width: 280px;
  }
  
  .add-list-button {
    height: 100px;
    font-size: 14px;
    
    .anticon {
      font-size: 20px;
    }
  }
  
  .add-list-form {
    padding: 16px;
    
    .list-input {
      font-size: 14px;
      height: 40px;
      
      :deep(.ant-input) {
        font-size: 14px;
      }
    }
    
    .form-actions {
      margin-top: 12px;
      
      .submit-button {
        font-size: 13px;
        height: 32px;
      }
      
      .cancel-button {
        height: 32px;
      }
    }
  }
}
</style>