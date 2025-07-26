<template>
  <a-drawer
    :open="isVisible"
    placement="right"
    :closable="false"
    :width="320"
    class="filter-panel"
    @close="handleClose"
  >
    <template #title>
      <div class="panel-header">
        <h3 class="panel-title">{{ t('board.filters.title') }}</h3>
        <a-button
          type="text"
          size="small"
          class="close-button"
          @click="handleClose"
        >
          <template #icon>
            <CloseOutlined />
          </template>
        </a-button>
      </div>
    </template>
    
    <div class="panel-content">
      <!-- Clear all filters -->
      <a-button
        v-if="hasActiveFilters"
        type="text"
        size="small"
        class="clear-all-button"
        @click="clearFilters"
      >
        {{ t('board.filters.clearAll') }}
      </a-button>
      
      <!-- Search -->
      <div class="filter-section">
        <a-input
          v-model:value="searchTerm"
          :placeholder="t('board.filters.searchPlaceholder')"
          class="search-input"
          @input="handleSearchChange"
        >
          <template #prefix>
            <SearchOutlined />
          </template>
        </a-input>
      </div>
      
      <!-- Labels filter -->
      <div class="filter-section">
        <h4 class="section-title">{{ t('board.filters.labels') }}</h4>
        <div class="labels-list">
          <a-checkbox
            v-for="label in availableLabels"
            :key="label.id"
            v-model:checked="selectedLabels[label.id]"
            class="label-checkbox"
            @change="handleLabelChange"
          >
            <span
              class="label-color"
              :style="{ backgroundColor: label.color }"
            ></span>
            {{ label.name }}
          </a-checkbox>
        </div>
      </div>
      
      <!-- Members filter -->
      <div class="filter-section">
        <h4 class="section-title">{{ t('board.filters.members') }}</h4>
        <div class="members-list">
          <a-checkbox
            v-for="member in availableMembers"
            :key="member.id"
            v-model:checked="selectedMembers[member.id]"
            class="member-checkbox"
            @change="handleMemberChange"
          >
            <a-avatar
              :size="24"
              :src="member.avatar"
              class="member-avatar"
            >
              {{ member.name.charAt(0) }}
            </a-avatar>
            {{ member.name }}
          </a-checkbox>
        </div>
      </div>
      
      <!-- Due date filter -->
      <div class="filter-section">
        <h4 class="section-title">{{ t('board.filters.dueDate') }}</h4>
        <a-radio-group
          v-model:value="selectedDueDate"
          class="due-date-group"
          @change="handleDueDateChange"
        >
          <a-radio value="none">{{ t('board.filters.noDueDate') }}</a-radio>
          <a-radio value="overdue">{{ t('board.filters.overdue') }}</a-radio>
          <a-radio value="today">{{ t('board.filters.dueToday') }}</a-radio>
          <a-radio value="week">{{ t('board.filters.dueThisWeek') }}</a-radio>
          <a-radio value="month">{{ t('board.filters.dueThisMonth') }}</a-radio>
        </a-radio-group>
      </div>
    </div>
  </a-drawer>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import {
  SearchOutlined,
  CloseOutlined,
  TagOutlined,
  UserOutlined,
  CalendarOutlined,
  CheckOutlined
} from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'


interface Emits {
  filtersChange: [filters: {
    searchTerm: string
    selectedLabels: string[]
    selectedMembers: Record<string, any>
    dueDateFilter: string
  }]
}

const emit = defineEmits<Emits>()
const { t } = useI18n()

const isVisible = ref(true)
const searchTerm = ref('')
const selectedLabels = ref<string[]>([])
const selectedMembers = ref<Record<string, any>>({})
const dueDateFilter = ref('')

const mockLabels = ref([
  'Bug', 
  'Feature', 
  'Enhancement', 
  'High Priority', 
  'Low Priority'
])

const availableLabels = ref(mockLabels.value.map(l => ({ id: l, name: l, })))

const mockMembers = ref([
  { id: '1', name: 'John Doe', avatar: '/api/placeholder/24/24' },
  { id: '2', name: 'Jane Smith', avatar: '/api/placeholder/24/24' },
  { id: '3', name: 'Bob Johnson', avatar: '/api/placeholder/24/24' },
])

const availableMembers = ref(mockMembers.value)

const hasActiveFilters = computed(() => 
  searchTerm.value || 
  selectedLabels.value.length > 0 || 
  selectedMembers.value.length > 0 || 
  dueDateFilter.value
)

const handleClose = () => {
  isVisible.value = false
}

const toggleLabel = (label: string, checked: boolean) => {
  if (checked) {
    selectedLabels.value.push(label)
  } else {
    selectedLabels.value = selectedLabels.value.filter(l => l !== label)
  }
  emitFiltersChange()
}

const toggleMember = (memberId: string) => {
  if (selectedMembers.value.includes(memberId)) {
    selectedMembers.value = selectedMembers.value.filter(m => m !== memberId)
  } else {
    selectedMembers.value.push(memberId)
  }
  emitFiltersChange()
}

const clearFilters = () => {
  searchTerm.value = ''
  selectedLabels.value = []
  selectedMembers.value = []
  dueDateFilter.value = ''
  emitFiltersChange()
}

const handleMemberChange = () => {

}

const emitFiltersChange = () => {
  emit('filtersChange', {
    searchTerm: searchTerm.value,
    selectedLabels: selectedLabels.value,
    selectedMembers: selectedMembers.value,
    dueDateFilter: dueDateFilter.value
  })
}

// Watch for changes and emit
watch([searchTerm, dueDateFilter], emitFiltersChange)
</script>

<style scoped lang="scss">
:deep(.filter-panel) {
  .ant-drawer-content {
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(20px);
  }
  
  .ant-drawer-header {
    background: rgba(255, 255, 255, 0.98);
    backdrop-filter: blur(20px);
    border-bottom: 1px solid rgba(0, 0, 0, 0.06);
    padding: 16px 24px;
  }
  
  .ant-drawer-body {
    padding: 24px;
  }
}

.panel-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  
  .panel-title {
    font-size: 18px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
  }
  
  .close-button {
    color: var(--color-text-weak);
    border-radius: 6px;
    transition: var(--transition-smooth);
    
    &:hover {
      color: var(--color-text);
      background: rgba(0, 0, 0, 0.04);
    }
  }
}

.panel-content {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.clear-all-button {
  align-self: flex-start;
  color: var(--color-red-600);
  font-weight: 500;
  border-radius: 6px;
  transition: var(--transition-smooth);
  
  &:hover {
    background: rgba(var(--color-red-rgb), 0.04);
    color: var(--color-red-700);
  }
}

.filter-section {
  display: flex;
  flex-direction: column;
  gap: 12px;
  
  .section-title {
    font-size: 14px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
  }
}

.search-input {
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

.labels-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
  
  .label-checkbox {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 12px;
    border-radius: 6px;
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(0, 0, 0, 0.02);
    }
    
    .label-color {
      width: 16px;
      height: 16px;
      border-radius: 4px;
      flex-shrink: 0;
    }
  }
}

.members-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
  
  .member-checkbox {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 12px;
    border-radius: 6px;
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(0, 0, 0, 0.02);
    }
    
    .member-avatar {
      flex-shrink: 0;
    }
  }
}

.due-date-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
  
  :deep(.ant-radio-wrapper) {
    padding: 8px 12px;
    border-radius: 6px;
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(0, 0, 0, 0.02);
    }
  }
}

@media (max-width: 768px) {
  :deep(.filter-panel) {
    .ant-drawer-content {
      width: 100% !important;
    }
  }
  
  .panel-header {
    .panel-title {
      font-size: 16px;
    }
  }
  
  .panel-content {
    gap: 20px;
  }
  
  .filter-section {
    gap: 10px;
    
    .section-title {
      font-size: 13px;
    }
  }
}
</style>