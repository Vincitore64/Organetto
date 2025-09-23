<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { useGetCardDetail, type CardVm } from '@/application'
import ModalContainer from '@/presentation/shared/components/ModalContainer.vue'
import { useVModel } from '@vueuse/core'
import _ from 'lodash'
import CardModalContent from './CardModalContent.vue'
import Spinner from '@/presentation/shared/ui/components/Spinner.vue'
import type { CardDetailVm } from '@/application/boards/columns/cards/models/Card'


interface Props {
  card: CardVm
  columnId: number
  listName: string,
  visible: boolean
}

interface Emits {
  close: []
  update: [card: Partial<CardVm>]
  cardUpdated: [card: CardVm]
  'update:card': [card: CardVm]
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()

const { data: cardDetail, isLoading } = useGetCardDetail(computed(() => ({ id: props.card.id, columnId: props.columnId })))

const activeCard = ref<CardDetailVm | null>(null)

const card = useVModel(props, 'card', emit)

const isVisible = computed({
  get: () => props.visible,
  set: (value) => {
    if (!value) {
      emit('close')
    }
  }
})


watch(cardDetail, (newCard) => {
  activeCard.value = newCard
})

const handleClose = () => {
  emit('close')
}

const updateCard = (updatedCard: CardVm) => {
  card.value = updatedCard
  emit('cardUpdated', updatedCard)
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
    @close="handleClose"
  >
    <main class="card-modal__modal-containter">
      <Spinner tip="Loading..." :spinning="isLoading">
        <CardModalContent
          v-if="activeCard"
          v-model:card="activeCard"
          :column-id="props.columnId"
          :list-name="props.listName"
          @card-updated="updateCard"
        />
      </Spinner>
    </main>
  </ModalContainer>
</template>
<style scoped lang="scss">
.card-modal {
  .item-modal-title {
    font-size: 2rem;
  }
  &__modal-containter {
    display: grid;
    min-height: 256px;
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

@media (max-width: 768px) {
  :deep(.card-modal) {
    .ant-modal-content {
      margin: 16px;
      width: calc(100vw - 32px) !important;
    }
  }
}
</style>