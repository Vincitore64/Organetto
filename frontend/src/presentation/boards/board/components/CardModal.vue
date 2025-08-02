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
import AvatarGroup from './AvatarGroup.vue'
import type { CardVm } from '@/application'
import ModalContainer from '@/presentation/shared/components/ModalContainer.vue'


interface Props {
  card: CardVm
  listName: string,
  visible: boolean
}

interface Emits {
  close: []
  update: [card: Partial<CardVm>]
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

const cardLabels = ref<string[]>(['critical'])

const cardUsers = computed(() => 
  // props.card.assignees.map(id => ({
  //   id,
  //   name: `User ${id}`,
  //   avatar: '/api/placeholder/32/32'
  // }))
  []
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
  // const newLabels = [...props.card.]
  // newLabels.splice(index, 1)
  // emit('update', { labels: newLabels })
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

<template>
  <ModalContainer
    v-model:open="isVisible"
    :title="props.card.title"
    :footer="null"
    width="900px"
    class="card-modal"
    wrap-class-name="card-modal"
    @cancel="handleClose"
  >
    <div class="modal-content">
      <!-- <header class="modal-header">
        <div class="title-section">
          <a-input
            v-model:value="title"
            class="title-input"
            :bordered="false"
            size="large"
            @blur="updateTitle"
          />
          <span class="list-name">
            {{ t('board.cardModal.inList', { listName: listName }) }}
          </span>
        </div>
      </header> -->
      <!-- <Divider bottom top/> -->
      <main class="modal-main">
        <div class="left-column">
          <!-- Labels -->
          <section v-if="cardLabels.length > 0" class="modal-section">
            <h3 class="section-title">
              <TagOutlined />
              {{ t('board.cardModal.labels') }}
            </h3>
            <div class="labels-container">
              <a-tag
                v-for="(label, index) in cardLabels"
                :key="index"
                class="label-tag"
                closable
                @close="removeLabel(index)"
              >
                {{ label }}
              </a-tag>
            </div>
          </section>

          <!-- Description -->
          <section class="modal-section">
            <h3 class="section-title">
              <FileTextOutlined />
              {{ t('board.cardModal.description') }}
            </h3>
            <a-textarea
              v-model:value="description"
              :placeholder="t('board.cardModal.descriptionPlaceholder')"
              class="description-textarea"
              :rows="4"
              :auto-size="{ minRows: 4, maxRows: 8 }"
              @blur="updateDescription"
            />
          </section>

          <!-- Comments/Activity -->
          <section class="modal-section">
            <h3 class="section-title">
              <MessageOutlined />
              {{ t('board.cardModal.activity') }}
            </h3>
            <div class="comment-form">
              <a-avatar
                :src="currentUser.avatar"
                :alt="currentUser.name"
                class="user-avatar"
              >
                {{ currentUser.name.charAt(0).toUpperCase() }}
              </a-avatar>
              <a-textarea
                v-model:value="newComment"
                :placeholder="t('board.cardModal.commentPlaceholder')"
                class="comment-input"
                :rows="2"
                :auto-size="{ minRows: 2, maxRows: 4 }"
                @keydown.ctrl.enter="addComment"
              />
              <a-button
                v-if="newComment.trim()"
                type="primary"
                size="small"
                class="add-comment-btn"
                @click="addComment"
              >
                {{ t('board.cardModal.addComment') }}
              </a-button>
            </div>
          </section>
        </div>

        <aside class="sidebar">
          <!-- Add to card -->
          <section class="modal-section">
            <h3 class="section-title">
              {{ t('board.cardModal.addToCard') }}
            </h3>
            <div class="action-buttons">
              <a-button
                class="action-button"
                block
                @click="openMembersModal"
              >
                <template #icon>
                  <UserOutlined />
                </template>
                {{ t('board.cardModal.members') }}
              </a-button>
              <a-button
                class="action-button"
                block
                @click="openLabelsModal"
              >
                <template #icon>
                  <TagOutlined />
                </template>
                {{ t('board.cardModal.labels') }}
              </a-button>
              <a-button
                class="action-button"
                block
                @click="openDatePicker"
              >
                <template #icon>
                  <CalendarOutlined />
                </template>
                {{ t('board.cardModal.dueDate') }}
              </a-button>
              <a-button
                class="action-button"
                block
                @click="openAttachmentModal"
              >
                <template #icon>
                  <PaperClipOutlined />
                </template>
                {{ t('board.cardModal.attachment') }}
              </a-button>
            </div>
          </section>

          <!-- Actions -->
          <section class="modal-section">
            <h3 class="section-title">
              {{ t('board.cardModal.actions') }}
            </h3>
            <div class="action-buttons">
              <a-button
                class="action-button"
                block
                @click="moveCard"
              >
                <template #icon>
                  <ArrowRightOutlined />
                </template>
                {{ t('board.cardModal.move') }}
              </a-button>
              <a-button
                class="action-button"
                block
                @click="copyCard"
              >
                <template #icon>
                  <CopyOutlined />
                </template>
                {{ t('board.cardModal.copy') }}
              </a-button>
              <a-button
                type="primary"
                block
                danger
                @click="archiveCard"
              >
                <template #icon>
                  <DeleteOutlined />
                </template>
                {{ t('board.cardModal.archive') }}
              </a-button>
            </div>
          </section>

          <!-- Members -->
          <section v-if="cardUsers.length > 0" class="modal-section">
            <h3 class="section-title">
              {{ t('board.cardModal.members') }}
            </h3>
            <AvatarGroup
              :users="cardUsers"
              :max-visible="5"
              size="md"
            />
          </section>

          <!-- Due Date -->
          <section v-if="card.dueDate" class="modal-section">
            <h3 class="section-title">
              {{ t('board.cardModal.dueDate') }}
            </h3>
            <div class="due-date">
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
  </ModalContainer>
</template>
<style scoped lang="scss">
.card-modal {
  .item-modal-title {
    font-size: 2rem;
  }
  // .ant-modal-content {
  //   background: rgba(255, 255, 255, 0.95);
  //   backdrop-filter: blur(20px);
  //   border-radius: 16px;
  //   border: 1px solid rgba(0, 0, 0, 0.06);
  //   box-shadow: 0 20px 60px rgba(0, 0, 0, 0.12);
  // }
  
  // .ant-modal-body {
  //   padding: 24px;
  // }
  
  // .ant-modal-close {
  //   top: 16px;
  //   right: 16px;
    
  //   .ant-modal-close-x {
  //     width: 32px;
  //     height: 32px;
  //     line-height: 32px;
  //     border-radius: 8px;
  //     transition: var(--transition-smooth);
      
  //     &:hover {
  //       background: rgba(0, 0, 0, 0.04);
  //     }
  //   }
  // }
}

.modal-content {
  display: flex;
  flex-direction: column;
  // gap: 20px;
  max-height: 80vh;
}

.modal-header {
  .title-section {
    .title-input {
      font-size: 22px;
      font-weight: 600;
      color: var(--color-text);
      font-family: 'Sofia Sans Extra Condensed', sans-serif;
      letter-spacing: 0px;
      padding: 0 12px;
      
      :deep(.ant-input) {
        font-size: 20px;
        font-weight: 700;
        background: transparent;
        
        &:focus {
          box-shadow: none;
        }
      }
    }
    
    .list-name {
      font-size: 12px;
      color: var(--color-text-weak);
      display: block;
      padding: 0 12px;
      font-weight: 400;
    }
  }
}

.modal-main {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 32px;
  overflow-y: auto;
}

.left-column {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.sidebar {
  display: flex;
  flex-direction: column;
  gap: 24px;
  align-content: start;
}

.modal-section {
  .section-title {
    font-size: 20px;
    font-weight: 400;
    color: var(--color-text);
    margin: 0 0 12px 0;
    display: flex;
    align-items: center;
    gap: 8px;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
  }
}

.labels-container {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  
  .label-tag {
    background: linear-gradient(135deg, var(--color-primary-500), var(--color-primary-600));
    color: white;
    border: none;
    border-radius: 6px;
    font-weight: 500;
    
    :deep(.ant-tag-close-icon) {
      color: rgba(255, 255, 255, 0.8);
      
      &:hover {
        color: white;
      }
    }
  }
}

.description-textarea {
  background: rgba(255, 255, 255, 0.8);
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 8px;
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

.comment-form {
  display: grid;
  grid-template-columns: auto 1fr auto;
  gap: 12px;
  align-items: start;
  
  .user-avatar {
    margin-top: 4px;
  }
  
  .comment-input {
    background: rgba(255, 255, 255, 0.8);
    border: 1px solid rgba(0, 0, 0, 0.06);
    border-radius: 8px;
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
  
  .add-comment-btn {
    margin-top: 4px;
    border-radius: 6px;
  }
}

.action-buttons {
  display: flex;
  flex-direction: column;
  gap: 8px;
  
  .action-button {
    justify-content: flex-start;
    // background: rgba(255, 255, 255, 0.8);
    // background: var(--color-white-gradient);
    border: 1px solid rgba(0, 0, 0, 0.06);
    // border-radius: 8px;
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(255, 255, 255, 0.95);
      // border-color: var(--color-primary-400);
      transform: translateY(-1px);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
    }
    
    // &.danger {
    //   color: var(--color-red-600);
      
    //   &:hover {
    //     color: var(--color-red-700);
    //     background: rgba(var(--color-red-rgb), 0.04);
    //     border-color: var(--color-red-300);
    //   }
    // }
  }
}

.due-date {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 12px 16px;
  background: rgba(255, 255, 255, 0.8);
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 8px;
  
  span {
    flex: 1;
    font-weight: 500;
    color: var(--color-text);
  }
}

@media (max-width: 768px) {
  :deep(.card-modal) {
    .ant-modal-content {
      margin: 16px;
      width: calc(100vw - 32px) !important;
    }
  }
  
  .modal-main {
    grid-template-columns: 1fr;
    gap: 24px;
  }
  
  .modal-header {
    .title-section {
      .title-input {
        font-size: 18px;
        
        :deep(.ant-input) {
          font-size: 18px;
        }
      }
    }
  }
  
  .comment-form {
    grid-template-columns: 1fr;
    gap: 8px;
    
    .user-avatar {
      margin-top: 0;
      justify-self: start;
    }
  }
}
</style>