<template>
  <aside :class="styles.panel">
    <header :class="styles.header">
      <h3 :class="styles.title">{{ t('board.filters.title') }}</h3>
      <a-button
        v-if="hasActiveFilters"
        type="text"
        size="small"
        :class="styles.clearButton"
        @click="clearFilters"
      >
        <template #icon>
          <CloseOutlined />
        </template>
        {{ t('board.filters.clearAll') }}
      </a-button>
    </header>

    <main :class="styles.content">
      <!-- Search -->
      <section :class="styles.filterGroup">
        <a-input
          v-model:value="searchTerm"
          :placeholder="t('board.filters.searchPlaceholder')"
          :class="styles.searchInput"
          allow-clear
        >
          <template #prefix>
            <SearchOutlined :class="styles.searchIcon" />
          </template>
        </a-input>
      </section>

      <!-- Labels -->
      <section :class="styles.filterGroup">
        <h4 :class="styles.filterTitle">
          <TagOutlined />
          {{ t('board.filters.labels') }}
        </h4>
        <div :class="styles.filterOptions">
          <a-tag
            v-for="label in mockLabels"
            :key="label"
            :class="styles.filterOption"
            :color="selectedLabels.includes(label) ? 'blue' : 'default'"
            :checkable="true"
            :checked="selectedLabels.includes(label)"
            @change="(checked: boolean) => toggleLabel(label, checked)"
          >
            {{ label }}
          </a-tag>
        </div>
      </section>

      <!-- Members -->
      <section :class="styles.filterGroup">
        <h4 :class="styles.filterTitle">
          <UserOutlined />
          {{ t('board.filters.members') }}
        </h4>
        <div :class="styles.filterOptions">
          <div
            v-for="member in mockMembers"
            :key="member.id"
            :class="[
              styles.memberOption,
              { [styles.active]: selectedMembers.includes(member.id) }
            ]"
            @click="toggleMember(member.id)"
          >
            <a-avatar
              :src="member.avatar"
              :alt="member.name"
              :size="24"
              :class="styles.memberAvatar"
            >
              {{ member.name.charAt(0).toUpperCase() }}
            </a-avatar>
            <span>{{ member.name }}</span>
            <CheckOutlined
              v-if="selectedMembers.includes(member.id)"
              :class="styles.checkIcon"
            />
          </div>
        </div>
      </section>

      <!-- Due Date -->
      <section :class="styles.filterGroup">
        <h4 :class="styles.filterTitle">
          <CalendarOutlined />
          {{ t('board.filters.dueDate') }}
        </h4>
        <a-select
          v-model:value="dueDateFilter"
          :class="styles.select"
          :placeholder="t('board.filters.dueDatePlaceholder')"
          allow-clear
        >
          <a-select-option value="overdue">
            {{ t('board.filters.overdue') }}
          </a-select-option>
          <a-select-option value="today">
            {{ t('board.filters.dueToday') }}
          </a-select-option>
          <a-select-option value="week">
            {{ t('board.filters.dueThisWeek') }}
          </a-select-option>
          <a-select-option value="month">
            {{ t('board.filters.dueThisMonth') }}
          </a-select-option>
          <a-select-option value="no-due">
            {{ t('board.filters.noDueDate') }}
          </a-select-option>
        </a-select>
      </section>
    </main>
  </aside>
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
import styles from './FilterPanel.module.scss'

interface Emits {
  filtersChange: [filters: {
    searchTerm: string
    selectedLabels: string[]
    selectedMembers: string[]
    dueDateFilter: string
  }]
}

const emit = defineEmits<Emits>()
const { t } = useI18n()

const searchTerm = ref('')
const selectedLabels = ref<string[]>([])
const selectedMembers = ref<string[]>([])
const dueDateFilter = ref('')

const mockLabels = ref([
  'Bug', 
  'Feature', 
  'Enhancement', 
  'High Priority', 
  'Low Priority'
])

const mockMembers = ref([
  { id: '1', name: 'John Doe', avatar: '/api/placeholder/24/24' },
  { id: '2', name: 'Jane Smith', avatar: '/api/placeholder/24/24' },
  { id: '3', name: 'Bob Johnson', avatar: '/api/placeholder/24/24' },
])

const hasActiveFilters = computed(() => 
  searchTerm.value || 
  selectedLabels.value.length > 0 || 
  selectedMembers.value.length > 0 || 
  dueDateFilter.value
)

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

<style module="styles" lang="scss">
@use './FilterPanel.module.scss';

.memberOption {
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s ease;
  
  &:hover {
    background: #f8fafc;
  }
  
  &.active {
    background: #e0f2fe;
    color: #0277bd;
  }
}

.checkIcon {
  color: #10b981;
  font-size: 14px;
}

.filterOptions {
  display: grid;
  gap: 4px;
}
</style>