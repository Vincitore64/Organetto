<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { useDraggable } from '@vueuse/core'
import {
  MoreOutlined,
  EditOutlined,
  DeleteOutlined,
  InboxOutlined
} from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import CardItem from './CardItem.vue'
import AddCardButton from './AddCardButton.vue'
import { useRemoveColumn, useUpdateColumn, type CardVm, type ColumnVm } from '@/application'
import Spinner from '@/presentation/shared/ui/components/Spinner.vue'
import _ from 'lodash'
import type { UseKanbanDndReturn } from '@/application/shared/dnd/hooks/useKanbanDnd'
import draggable from 'vuedraggable'

interface Emits {
  cardClick: [card: CardVm]
  cardCreated: [card: CardVm, list: ColumnVm]
  cardDeleted: [card: CardVm, list: ColumnVm]
  columnUpdated: [list: ColumnVm]
  columnDeleted: [list: ColumnVm]
  columnDragStart: [listId: number]
  columnDragEnd: [listId: number]
  cardDragStart: [cardId: number, listId: number]
  cardDragEnd: [cardId: number, listId: number]
}

const props = defineProps<{
  list: ColumnVm
  dnd: UseKanbanDndReturn,
  boardId: number
}>()
const emit = defineEmits<Emits>()
const { t } = useI18n()

const updateState = useUpdateColumn()
const removeState = useRemoveColumn()

const columnRef = ref<HTMLElement>()
const isDragging = ref(false)
const isTitleEditing = ref(false)

const titleForEditing = ref(props.list.title)

// Column drag functionality
const { style: dragStyle } = useDraggable(columnRef, {
  initialValue: { x: 0, y: 0 },
  onStart: () => {
    isDragging.value = true
    emit('columnDragStart', props.list.id)
  },
  onEnd: () => {
    isDragging.value = false
    emit('columnDragEnd', props.list.id)
  }
})

watch(() => props.list.id, () => {
  titleForEditing.value = props.list.title
})

const cards = computed(() => _(props.list.cards).sortBy(c => c.position).value())

const newCardPosition = computed(() => {
  const last = _(cards.value).last()
  return last?.position ? last.position + 1 : cards.value.length
})

const startColumnDrag = () => {
  // Additional column drag logic if needed
}

const handleCardClick = (card: CardVm) => {
  emit('cardClick', card)
}

const handleCardDragStart = (cardId: number) => {
  emit('cardDragStart', cardId, props.list.id)
}

const handleCardDragEnd = (cardId: number) => {
  emit('cardDragEnd', cardId, props.list.id)
}

const handleDrop = () => {
  // Handle drop logic
}

const handleEdit = () => {
  // Handle edit logic
  isTitleEditing.value = true
}

const handleArchive = () => {
  // Handle archive logic
}

const handleDelete = async () => {
  await removeState.mutateAsync({ boardId: props.boardId, id: props.list.id })
  emit('columnDeleted', props.list)
}

const handleAddCard = (c: CardVm) => {
  emit('cardCreated', c, props.list)
}

const onTitleBlur = async () => {
  // debugger
  if (props.list.title === titleForEditing.value) return

  const updatedList = { ...props.list, title: titleForEditing.value }
  await updateState.mutateAsync({ ...updatedList, boardId: props.boardId })
  emit('columnUpdated', updatedList)
  isTitleEditing.value = false
}
</script>

<template>
  <main class="board-column__wrapper">
    <Spinner :spinning="removeState.isPending.value">
      <div
        class="board-column"
        :data-column-id="list.id"
      >
        <div class="column-header">
          <div class="header-content">
            <h3 class="column-title" v-if="!isTitleEditing">{{ list.title }}</h3>
            <a-input
              class="column-title"
              :bordered="false"
              v-model:value="titleForEditing"
              @blur="onTitleBlur()"
              @drop.prevent=""
              @drag.prevent=""
              style="width: min-content;"
              v-else
            ></a-input>
            <div class="column-badge">{{ list.cards.length }}</div>
          </div>
          <a-dropdown :trigger="['click']" placement="bottomRight">
            <a-button
              type="text"
              size="small"
              class="menu-button"
              :aria-label="t('board.column.menu')"
            >
              <template #icon>
                <MoreOutlined />
              </template>
            </a-button>
            <template #overlay>
              <a-menu class="column-menu">
                <a-menu-item key="edit" @click="handleEdit">
                  <EditOutlined />
                  {{ t('board.column.edit') }}
                </a-menu-item>
                <a-menu-item key="archive" @click="handleArchive">
                  <InboxOutlined />
                  {{ t('board.column.archive') }}
                </a-menu-item>
                <a-menu-divider />
                <a-menu-item key="delete" danger @click="handleDelete">
                  <DeleteOutlined />
                  {{ t('board.column.delete') }}
                </a-menu-item>
              </a-menu>
            </template>
          </a-dropdown>
        </div>
        
        <div class="column-content"> <!-- :data-col-id="list.id" -->
          <!-- <pre>{{ list.cards }}</pre> -->
          <draggable v-bind="dnd.cardDraggableBind(list)">
            <template #item="{ element: data }">
              <section class="card-item__wrapper">
                <CardItem
                  :card="data"
                  :column-id="list.id"
                  class="card-item"
                  @click="handleCardClick(data)"
                  @card-deleted="emit('cardDeleted', data, list)"
                />
              </section>
            </template>

          </draggable>
          <!-- <UseVirtualList class="virtual-card-list" :list="cards" :options="{ itemHeight: 180 }" height="100%">
            <template #default="{ data, index }">
              <section class="card-item__wrapper">
                <section
                  class="card-item__dnd-wrapper"
                >                  
                  <CardItem
                    :card="data"
                    :column-id="list.id"
                    class="card-item"
                    @click="handleCardClick(data)"
                    @card-deleted="emit('cardDeleted', data, list)"
                  />
                </section>
              </section>
            </template>
          </UseVirtualList> -->
          <section class="add-card-button__wrapper">
            <AddCardButton
              :listId="list.id"
              :position="newCardPosition"
              @created="handleAddCard"
            />
          </section>
        </div>
      </div>
    </Spinner>
  </main>
</template>

<style scoped lang="scss">
// Columns in the board adopt the same surface treatment as cards on the boards
// overview page: a clean white panel with a subtle stroke and shadow.  The glass
// blur effect is removed to improve contrast against the lighter page
// background.
.board-column {
  width: 360px;
  min-width: 360px;
  max-width: 360px;
  // background: var(--color-surface);
  background: linear-gradient(135deg, #ffffff, #fff7eb);;
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 12px;
  box-shadow: var(--shadow-light);
  transition: var(--transition-smooth);
  display: flex;
  flex-direction: column;
  height: fit-content;
  max-height: calc(100vh - 140px);

  &:hover {
    box-shadow: var(--shadow-medium);
    transform: translateY(-2px);
    border-color: var(--color-primary-200, #bae7ff);
  }
}

.column-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px 12px;
  border-bottom: 1px solid rgba(0, 0, 0, 0.04);
  
  .header-content {
    display: flex;
    align-items: center;
    gap: 12px;
    flex: 1;
  }
  
  .column-title {
    font-size: 22px;
    // font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
  }
  
  .column-badge {
    background: rgba(var(--color-primary-rgb), 0.1);
    color: var(--color-primary-600);
    border: 1px solid rgba(var(--color-primary-rgb), 0.2);
    border-radius: 12px;
    padding: 2px 8px;
    font-size: 12px;
    font-weight: 500;
    min-width: 24px;
    text-align: center;
  }
  
  .menu-button {
    color: var(--color-text-weak);
    border-radius: 6px;
    transition: var(--transition-smooth);
    
    &:hover {
      color: var(--color-text);
      background: rgba(0, 0, 0, 0.04);
    }
  }
}

.column-content {
  display: flex;
  flex-direction: column;
  flex: 1;
  overflow: hidden;
  .cards,ol {
    padding: 0;
  }
}

.card-item__wrapper {
  padding: 8px 16px;
}
.add-card-button__wrapper {
  padding: 16px;
}

// .cards-list {
//   flex: 1;
//   overflow-y: auto;
  
//   &::-webkit-scrollbar {
//     width: 6px;
//   }
  
//   &::-webkit-scrollbar-track {
//     background: rgba(0, 0, 0, 0.02);
//     border-radius: 3px;
//   }
  
//   &::-webkit-scrollbar-thumb {
//     background: rgba(0, 0, 0, 0.1);
//     border-radius: 3px;
    
//     &:hover {
//       background: rgba(0, 0, 0, 0.15);
//     }
//   }
// }

.card-item {
  margin-bottom: 12px;
  
  &:last-child {
    margin-bottom: 0;
  }
}

.add-card-button {
  margin-top: auto;
}

:deep(.column-menu) {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(0, 0, 0, 0.06);
  border-radius: 8px;
  box-shadow: var(--shadow-medium);

  .ant-menu-item {
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(var(--color-primary-rgb), 0.04);
    }
  }
}

@media (max-width: 768px) {
  .board-column {
    width: 280px;
    min-width: 280px;
    max-width: 280px;
  }
  
  .column-header {
    padding: 12px 16px 8px;
    
    .column-title {
      font-size: 14px;
    }
  }
  
  .column-content {
    padding: 8px 12px 12px;
  }
}
</style>