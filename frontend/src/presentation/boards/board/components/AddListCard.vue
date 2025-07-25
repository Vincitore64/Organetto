<template>
  <div v-if="isAdding" :class="styles.addingCard">
    <form @submit="handleSubmit">
      <input
        ref="inputRef"
        v-model="title"
        type="text"
        :placeholder="$t('board.addList.placeholder')"
        :class="styles.input"
      />
      <div :class="styles.actions">
        <button type="submit" :class="styles.addButton">
          {{ $t('board.addList.addButton') }}
        </button>
        <button type="button" @click="handleCancel" :class="styles.cancelButton">
          <XIcon :size="20" />
        </button>
      </div>
    </form>
  </div>
  
  <button v-else :class="styles.addCard" @click="startAdding">
    <PlusIcon :size="20" />
    <span>{{ $t('board.addList.addAnother') }}</span>
  </button>
</template>

<script setup lang="ts">
import { ref, nextTick } from 'vue'
import { useI18n } from 'vue-i18n'
import { Plus as PlusIcon, X as XIcon } from 'lucide-vue-next'
import { useListStore } from '../../stores/listStore'
import styles from './AddListCard.module.scss'

interface Props {
  boardId: string
}

const props = defineProps<Props>()
const { t } = useI18n()
const listStore = useListStore()

const isAdding = ref(false)
const title = ref('')
const inputRef = ref<HTMLInputElement>()

const handleSubmit = async (e: Event) => {
  e.preventDefault()
  if (title.value.trim()) {
    await listStore.createList({ boardId: props.boardId, title: title.value.trim() })
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

<style module="styles" lang="scss">
@import './AddListCard.module.scss';
</style>