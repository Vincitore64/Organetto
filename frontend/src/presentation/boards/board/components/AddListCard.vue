<template>
  <main class="add-list-container">
    <FeatureCard
      v-if="!isAdding"
      :title="t('board.addList.placeholder')"
      :icon="PlusOutlined"
      :clickable="true"
      :show-cover="true"
      :show-floating-icons="true"
      class="add-list-card"
      @click="startAdding"
    />
    
    <div v-else class="add-list-form">
      <header class="add-list-title">
        <!-- {{ t('board.addList.title') }} -->
          Create a new list
      </header>
      <a-divider class="item-modal-title-divider"></a-divider>
      <a-input
        ref="inputRef"
        v-model:value="title"
        :placeholder="t('board.addList.placeholder')"
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
  </main>
</template>

<script setup lang="ts">
import { ref, nextTick } from 'vue'
import { useI18n } from 'vue-i18n'
import { PlusOutlined, CloseOutlined } from '@ant-design/icons-vue'
import FeatureCard from '@/presentation/shared/components/FeatureCard.vue'


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

// Customize the FeatureCard for add list functionality
.add-list-card {
  height: 200px;
  :deep(.feature-cover) {
    height: 80px;
  }
  :deep(.feature-body) {
    .main-icon {
      width: 48px;
      height: 48px;
      margin-bottom: 8px;
      
      .icon {
        font-size: 20px;
      }
    }
    
    .feature-title {
      font-size: 16px;
      font-weight: 400;
      margin: 0;
    }
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
  .add-list-title {
    text-align: center;
    // margin-bottom: 1rem;
    background: transparent;
    font-size: 1.75rem;
    font-weight: 300;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
  }

  .list-input {
    background: rgba(255, 255, 255, 0.9);
    border: 1px solid rgba(0, 0, 0, 0.06);
    border-radius: 8px;
    font-size: 16px;
    font-weight: 500;
    height: 44px;
    transition: var(--transition-smooth);

    // &:focus {
    //   border-color: var(--color-primary-400);
    //   box-shadow: 0 0 0 2px rgba(var(--color-primary-rgb), 0.1);
    // }

    // :deep(.ant-input) {
    //   background: transparent;
    //   border: none;
    //   box-shadow: none;
    //   font-size: 16px;
    //   font-weight: 500;

    //   &:focus {
    //     box-shadow: none;
    //   }

    //   &::placeholder {
    //     color: var(--color-text-weak);
    //   }
    // }
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
  
  .add-list-card {
    height: 100px;
    
    :deep(.feature-body) {
      .main-icon {
        width: 40px;
        height: 40px;
        margin-bottom: 6px;
        
        .icon {
          font-size: 16px;
        }
      }
      
      .feature-title {
        font-size: 14px;
      }
    }
  }
  
  .add-list-form {
    padding: 16px;
    
    // .list-input {
    //   font-size: 14px;
    //   height: 40px;
      
    //   :deep(.ant-input) {
    //     font-size: 14px;
    //   }
    // }
    
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