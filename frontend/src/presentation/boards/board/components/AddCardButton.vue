<script setup lang="ts">
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import AddItemCard from '../shared/components/AddItemCard.vue'
import { useAsyncState } from '@vueuse/core'
import { useCreateCard, type CardVm } from '@/application'
import type { CreateCardPayload } from '@/dataAccess/cards/models'
import { useForm } from '@/presentation/shared'

const props = defineProps<{
  listId: number,
  position: number,
}>()

const emit = defineEmits<{
  (e: 'created', v: CardVm): void,
}>()

const { t } = useI18n()

const createCardState = useCreateCard()

const { form, antdForm, reset, submit } = useForm({
  initialValues: { title: '' },
  transform: (p) => ({
    columnId: props.listId,
    position: props.position,
    title: p.title,
  }),
  action: createCardAction,
  rules: {
    title: [{ required: true, message: t('board.addCard.createForm.titleRequired') }],
  },
  useAntd: true,
  onSuccess(response) {
    debugger
    reset()
    emit('created', response as CardVm)
  }
})

async function createCardAction({ columnId, position, title }: CreateCardPayload) {
  return await createCardState.mutateAsync({ columnId, position, title })
}

const handleSubmit = async () => {
  submit()
}

const handleCancel = () => {
  reset()
}
</script>
<template>
  <AddItemCard class="add-card-btn" :loading="createCardState.isPending.value" @submit="handleSubmit" @cancel="handleCancel">
    <template :props="{ isAdding }" #default>
      <a-form layout="vertical"><!-- :rules="formInstance.rulesRef" @finish="onFinish" -->
        <a-form-item name="title" v-bind="antdForm!.validateInfos.title"><!-- :label="t('board.addList.createForm.titleLabel')"  -->
          <a-textarea
            v-model:value="form.title"
            :placeholder="t('board.addCard.placeholder')"
            :auto-size="{ minRows: 2, maxRows: 4 }"
            class="card-textarea"
            @keydown.enter.prevent="handleSubmit"
            @keydown.esc="handleCancel"
          />
          <!-- <a-input v-model:value="form.title" :placeholder="t('board.addCard.placeholder')" /> -->
        </a-form-item>
      </a-form>
      <!-- <a-textarea
        ref="textareaRef"
        v-model:value="form.title"
        :placeholder="t('board.addCard.placeholder')"
        :auto-size="{ minRows: 2, maxRows: 4 }"
        class="card-textarea"
        @keydown.enter.prevent="handleSubmit"
        @keydown.esc="handleCancel"
      /> -->
    </template>
  </AddItemCard>
</template>

<style scoped lang="scss">
.add-card-container {
  width: 100%;
}

// The "Add card" button follows the same visual language as the board cards on the
// boards listing page.  A lightly tinted surface with a coloured dashed border
// helps it stand out without feeling heavy.  When hovered or focused the
// border and background gently shift towards the primary colour, mirroring the
// hover behaviour used by other cards in the application.

.add-card-btn {
  :deep(.add-item-form) {
    background: white;
  }
}

// .add-card-button {
//   background: rgba(255, 255, 255, 0.6);
//   border: 2px dashed var(--color-primary-300, #91d5ff);
//   border-radius: 10px;
//   color: var(--color-text-weak);
//   font-size: 14px;
//   font-weight: 500;
//   height: 40px;
//   transition: var(--transition-smooth);

//   &:hover {
//     background: rgba(var(--color-primary-rgb), 0.08);
//     border-color: var(--color-primary-400, #69c0ff);
//     color: var(--color-primary-600);
//     transform: translateY(-1px);
//     box-shadow: var(--shadow-light);
//   }

//   &:focus {
//     background: rgba(var(--color-primary-rgb), 0.08);
//     border-color: var(--color-primary-400, #69c0ff);
//     color: var(--color-primary-600);
//   }
// }

// The add card form uses a solid surface rather than heavy glass to better match
// the cards shown on the boards page.  Input fields have clear focus states
// and the actions align with other primary buttons.
// .add-card-form {
//   background: rgba(255, 255, 255, 0.98);
//   backdrop-filter: blur(20px);
//   border: 1px solid rgba(0, 0, 0, 0.06);
//   border-radius: 10px;
//   padding: 16px;
//   box-shadow: var(--shadow-light);

//   .card-textarea {
//     background: rgba(255, 255, 255, 0.9);
//     border: 1px solid rgba(0, 0, 0, 0.06);
//     border-radius: 8px;
//     font-size: 14px;
//     transition: var(--transition-smooth);

//     &:focus {
//       border-color: var(--color-primary-400);
//       box-shadow: 0 0 0 2px rgba(var(--color-primary-rgb), 0.1);
//     }

//     :deep(.ant-input) {
//       background: transparent;
//       border: none;
//       box-shadow: none;

//       &:focus {
//         box-shadow: none;
//       }
//     }
//   }

//   .form-actions {
//     display: flex;
//     align-items: center;
//     gap: 8px;
//     margin-top: 12px;

//     .submit-button {
//       background: var(--color-blue-gradient);
//       border: none;
//       border-radius: 6px;
//       font-size: 13px;
//       font-weight: 500;
//       box-shadow: var(--shadow-light);
//       transition: var(--transition-smooth);

//       &:hover {
//         transform: translateY(-1px);
//         box-shadow: var(--shadow-medium);
//       }
//     }

//     .cancel-button {
//       color: var(--color-text-weak);
//       border-radius: 6px;
//       transition: var(--transition-smooth);

//       &:hover {
//         color: var(--color-text);
//         background: rgba(0, 0, 0, 0.04);
//       }
//     }
//   }
// }

// @media (max-width: 768px) {
//   .add-card-button {
//     font-size: 13px;
//     height: 36px;
//   }
  
//   .add-card-form {
//     padding: 12px;
    
//     .card-textarea {
//       font-size: 13px;
//     }
    
//     .form-actions {
//       margin-top: 8px;
      
//       .submit-button {
//         font-size: 12px;
//       }
//     }
//   }
// }
</style>