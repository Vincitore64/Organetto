<template>
  <a-modal
    v-model:open="isVisible"
    :title="null"
    :footer="null"
    :width="800"
    :class="styles.modal"
    @cancel="handleClose"
  >
    <div :class="styles.content">
      <header :class="styles.header">
        <div :class="styles.titleSection">
          <a-input
            v-model:value="title"
            :class="styles.titleInput"
            :bordered="false"
            size="large"
            @blur="updateTitle"
          />
          <span :class="styles.listName">
            {{ $t('board.cardModal.inList', { listName: 'To Do' }) }}
          </span>
        </div>
      </header>

      <main :class="styles.mainContent">
        <div :class="styles.leftColumn">
          <!-- Labels -->
          <section v-if="card.labels.length > 0" :class="styles.section">
            <h3 :class="styles.sectionTitle">
              {{ $t('board.cardModal.labels') }}
            </h3>
            <div :class="styles.labels">
              <a-tag
                v-for="(label, index) in card.labels"
                :key="index"
                :class="[styles.label, styles[`label-${index % 6}`]]"
                closable
                @close="removeLabel(index)"
              >
                {{ label }}
              </a-tag>
            </div>
          </section>

          <!-- Description -->
          <section :class="styles.section">
            <h3 :class="styles.sectionTitle">
              <FileTextOutlined />
              {{ $t('board.cardModal.description') }}
            </h3>
            <a-textarea
              v-model:value="description"
              :placeholder="$t('board.cardModal.descriptionPlaceholder')"
              :class="styles.descriptionTextarea"
              :rows="4"
              :auto-size="{ minRows: 4, maxRows: 8 }"
              @blur="updateDescription"
            />
          </section>

          <!-- Comments/Activity -->
          <section :class="styles.section">
            <h3 :class="styles.sectionTitle">
              <MessageOutlined />
              {{ $t('board.cardModal.activity') }}
            </h3>
            <div :class="styles.commentForm">
              <a-avatar
                :src="currentUser.avatar"
                :alt="currentUser.name"
                :class="styles.avatar"
              >
                {{ currentUser.name.charAt(0).toUpperCase() }}
              </a-avatar>
              <a-textarea
                v-model:value="newComment"
                :placeholder="$t('board.cardModal.commentPlaceholder')"
                :class="styles.commentInput"
                :rows="2"
                :auto-size="{ minRows: 2, maxRows: 4 }"
                @keydown.ctrl.enter="addComment"
              />
              <a-button
                v-if="newComment.trim()"
                type="primary"
                size="small"
                @click="addComment"
              >
                {{ $t('board.cardModal.addComment') }}
              </a-button>
            </div>
          </section>
        </div>

        <aside :class="styles.sidebar">
          <!-- Add to card -->
          <section :class="styles.section">
            <h3 :class="styles.sectionTitle">
              {{ $t('board.cardModal.addToCard') }}
            </h3>
            <div :class="styles.actionButtons">
              <a-button
                :class="styles.actionButton"
                block
                @click="openMembersModal"
              >
                <template #icon>
                  <UserOutlined />
                </template>
                {{ $t('board.cardModal.members') }}
              </a-button>
              <a-button
                :class="styles.actionButton"
                block
                @click="openLabelsModal"
              >
                <template #icon>
                  <TagOutlined />
                </template>
                {{ $t('board.cardModal.labels') }}
              </a-button>
              <a-button
                :class="styles.actionButton"
                block
                @click="openDatePicker"
              >
                <template #icon>
                  <CalendarOutlined />
                </template>
                {{ $t('board.cardModal.dueDate') }}
              </a-button>
              <a-button
                :class="styles.actionButton"
                block
                @click="openAttachmentModal"
              >
                <template #icon>
                  <PaperClipOutlined />
                </template>
                {{ $t('board.cardModal.attachment') }}
              </a-button>
            </div>
          </section>

          <!-- Actions -->
          <section :class="styles.section">
            <h3 :class="styles.sectionTitle">
              {{ $t('board.cardModal.actions') }}
            </h3>
            <div :class="styles.actionButtons">
              <a-button
                :class="styles.actionButton"
                block
                @click="moveCard"
              >
                <template #icon>
                  <ArrowRightOutlined />
                </template>
                {{ $t('board.cardModal.move') }}
              </a-button>
              <a-button
                :class="styles.actionButton"
                block
                @click="copyCard"
              >
                <template #icon>
                  <CopyOutlined />
                </template>
                {{ $t('board.cardModal.copy') }}
              </a-button>
              <a-button
                :class="[styles.actionButton, styles.danger]"
                block
                @click="archiveCard"
              >
                <template #icon>
                  <DeleteOutlined />
                </template>
                {{ $t('board.cardModal.archive') }}
              </a-button>
            </div>
          </section>

          <!-- Members -->
          <section v-if="mockUsers.length > 0" :class="styles.section">
            <h3 :class="styles.sectionTitle">
              {{ $t('board.cardModal.members') }}
            </h3>
            <AvatarGroup
              :users="mockUsers"
              :max-visible="5"
              size="md"
            />
          </section>

          <!-- Due Date -->
          <section v-if="card.dueDate" :class="styles.section">
            <h3 :class="styles.sectionTitle">
              {{ $t('board.cardModal.dueDate') }}
            </h3>
            <div :class="styles.dueDate">
              <CalendarOutlined />
              <span>{{ formatDate(card.dueDate) }}</span>
              <a-button
                type="text"
                size="small"
                @click="removeDueDate"
              >
                <template #icon>
                  <CloseOutlined />
                </template>
              </a-button>
            </div>
          </section>
        </aside>
      </main>
    </div>
  </a-modal>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import {
  FileTextOutlined,
  MessageOutlined,
  UserOutlined,
  TagOutlined,
  CalendarOutlined,
  PaperClipOutlined,
  ArrowRightOutlined,
  CopyOutlined,
  DeleteOutlined,
  CloseOutlined
} from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import dayjs from 'dayjs'
import { Card } from '../../types/board'
import AvatarGroup from './AvatarGroup.vue'
import styles from './CardModal.module.scss'

interface Props {
  card: Card
  visible: boolean
}

interface Emits {
  close: []
  update: [card: Partial<Card>]
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()
const { t } = useI18n()

const isVisible = computed({
  get: () => props.visible,
  set: (value) => {
    if (!value) {
      emit('close')
    }
  }
})

const title = ref(props.card.title)
const description = ref(props.card.description || '')
const newComment = ref('')

const currentUser = ref({
  id: 'current-user',
  name: 'Current User',
  avatar: '/api/placeholder/32/32'
})

const mockUsers = computed(() => 
  props.card.assignees.map(id => ({
    id,
    name: `User ${id}`,
    avatar: '/api/placeholder/32/32'
  }))
)

const formatDate = (date: Date) => {
  return dayjs(date).format('MMM DD, YYYY')
}

const handleClose = () => {
  emit('close')
}

const updateTitle = () => {
  if (title.value !== props.card.title) {
    emit('update', { title: title.value })
  }
}

const updateDescription = () => {
  if (description.value !== props.card.description) {
    emit('update', { description: description.value })
  }
}

const removeLabel = (index: number) => {
  const newLabels = [...props.card.labels]
  newLabels.splice(index, 1)
  emit('update', { labels: newLabels })
}

const addComment = () => {
  if (newComment.value.trim()) {
    // Handle comment addition logic here
    console.log('Adding comment:', newComment.value)
    newComment.value = ''
  }
}

const removeDueDate = () => {
  emit('update', { dueDate: undefined })
}

// Action handlers
const openMembersModal = () => {
  console.log('Open members modal')
}

const openLabelsModal = () => {
  console.log('Open labels modal')
}

const openDatePicker = () => {
  console.log('Open date picker')
}

const openAttachmentModal = () => {
  console.log('Open attachment modal')
}

const moveCard = () => {
  console.log('Move card')
}

const copyCard = () => {
  console.log('Copy card')
}

const archiveCard = () => {
  console.log('Archive card')
}
</script>

<style module="styles" lang="scss">
@import './CardModal.module.scss';

.content {
  display: grid;
  grid-template-rows: auto 1fr;
  gap: 16px;
  max-height: 80vh;
}

.mainContent {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 24px;
  overflow-y: auto;
}

.leftColumn {
  display: grid;
  gap: 16px;
}

.sidebar {
  display: grid;
  gap: 16px;
  align-content: start;
}

.actionButtons {
  display: grid;
  gap: 8px;
}

.actionButton {
  justify-content: flex-start;
  
  &.danger {
    color: #dc2626;
    
    &:hover {
      color: #dc2626;
      background: #fef2f2;
    }
  }
}

.commentForm {
  display: grid;
  grid-template-columns: auto 1fr auto;
  gap: 12px;
  align-items: start;
}

.dueDate {
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  background: #f8fafc;
  border-radius: 6px;
}
</style>