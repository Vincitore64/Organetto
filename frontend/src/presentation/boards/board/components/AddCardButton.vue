<template>
  <div v-if="isAdding" :class="styles.addingCard">
    <form @submit.prevent="handleSubmit">
      <textarea
        ref="textareaRef"
        v-model="title"
        :placeholder="$t('board.addCard.placeholder')"
        :class="styles.textarea"
        :rows="3"
        @keydown.esc="handleCancel"
      />
      <div :class="styles.actions">
        <a-button
          type="primary"
          html-type="submit"
          size="small"
          :class="styles.addButton"
        >
          {{ $t('board.addCard.add') }}
        </a-button>
        <a-button
          type="text"
          size="small"
          :class="styles.cancelButton"
          @click="handleCancel"
        >
          <template #icon>
            <CloseOutlined />
          </template>
        </a-button>
      </div>
    </form>
  </div>
  
  <button v-else :class="styles.addButton" @click="startAdding">
    <PlusOutlined />
    <span>{{ $t('board.addCard.addCard') }}</span>
  </button>
</template>

<script setup lang="ts">
import { ref, nextTick } from 'vue'
import { PlusOutlined, CloseOutlined } from '@ant-design/icons-vue'
import { useListStore } from '../../stores/listStore'
import { useI18n } from 'vue-i18n'
import styles from './AddCardButton.module.scss'

interface Props {
  listId: string
}

const props = defineProps<Props>()
const { t } = useI18n()
const listStore = useListStore()

const isAdding = ref(false)
const title = ref('')
const textareaRef = ref<HTMLTextAreaElement>()

const handleSubmit = async () => {
  if (title.value.trim()) {
    await listStore.createCard({ 
      listId: props.listId, 
      title: title.value.trim() 
    })
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

<style module="styles" lang="scss">
@import './AddCardButton.module.scss';
</style>