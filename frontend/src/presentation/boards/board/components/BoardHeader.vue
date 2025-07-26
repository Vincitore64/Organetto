<template>
  <header :class="styles.header">
    <section :class="styles.titleSection">
      <h1 :class="styles.title">{{ board.title }}</h1>
      <a-button
        type="text"
        size="small"
        :class="[styles.starButton, { [styles.starred]: isStarred }]"
        :aria-label="t('board.header.starBoard')"
        @click="toggleStar"
      >
        <template #icon>
          <StarFilled v-if="isStarred" />
          <StarOutlined v-else />
        </template>
      </a-button>
      <a-tag :class="styles.visibility" :color="visibilityColor">
        {{ t(`board.visibility.personal`) }} <!-- ${board.visibility.toLowerCase()} -->
      </a-tag>
    </section>
    
    <section :class="styles.actions">
      <a-button
        type="text"
        size="small"
        :class="[styles.filterButton, { [styles.active]: showFilters }]"
        @click="handleToggleFilters"
        :aria-label="t('board.header.toggleFilters')"
      >
        <template #icon>
          <FilterOutlined />
        </template>
        {{ t('board.header.filter') }}
      </a-button>
      
      <AvatarGroup
        :users="mockUsers"
        :max-visible="3"
        size="md"
      />
      
      <a-button
        type="primary"
        size="small"
        :class="styles.shareButton"
        :aria-label="t('board.header.shareBoard')"
        @click="handleShare"
      >
        <template #icon>
          <ShareAltOutlined />
        </template>
        {{ t('board.header.share') }}
      </a-button>
      
      <a-dropdown :trigger="['click']" placement="bottomRight">
        <a-button
          type="text"
          size="small"
          :class="styles.menuButton"
          :aria-label="t('board.header.boardMenu')"
        >
          <template #icon>
            <MoreOutlined />
          </template>
        </a-button>
        <template #overlay>
          <a-menu>
            <a-menu-item key="settings">
              <SettingOutlined />
              {{ t('board.header.settings') }}
            </a-menu-item>
            <a-menu-item key="export">
              <ExportOutlined />
              {{ t('board.header.export') }}
            </a-menu-item>
            <a-menu-divider />
            <a-menu-item key="archive">
              <InboxOutlined />
              {{ t('board.header.archive') }}
            </a-menu-item>
            <a-menu-item key="delete" danger>
              <DeleteOutlined />
              {{ t('board.header.delete') }}
            </a-menu-item>
          </a-menu>
        </template>
      </a-dropdown>
    </section>
  </header>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import {
  StarOutlined,
  StarFilled,
  FilterOutlined,
  ShareAltOutlined,
  MoreOutlined,
  SettingOutlined,
  ExportOutlined,
  DeleteOutlined,
  InboxOutlined
} from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import AvatarGroup from './AvatarGroup.vue'
import styles from './BoardHeader.module.scss'
import type { BoardDetailedDto } from '@/dataAccess/boards/models'

interface Props {
  board: BoardDetailedDto
  showFilters: boolean
}

interface Emits {
  toggleFilters: []
  share: []
  star: [starred: boolean]
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()
const { t } = useI18n()

const isStarred = ref(false)

const mockUsers = ref([
  { id: '1', name: 'John Doe', avatar: '/api/placeholder/32/32' },
  { id: '2', name: 'Jane Smith', avatar: '/api/placeholder/32/32' },
  { id: '3', name: 'Bob Johnson', avatar: '/api/placeholder/32/32' },
])

const visibilityColor = computed(() => {
  switch ('Personal') {
    case 'Personal': return 'blue'
    // case 'Team': return 'green'
    // case 'Public': return 'orange'
    default: return 'default'
  }
})

const toggleStar = () => {
  isStarred.value = !isStarred.value
  emit('star', isStarred.value)
}

const handleToggleFilters = () => {
  emit('toggleFilters')
}

const handleShare = () => {
  emit('share')
}
</script>

<style module="styles" lang="scss">
@import './BoardHeader.module.scss';

.starred {
  color: #fbbf24 !important;
}

.active {
  background: #e0f2fe !important;
  color: #0277bd !important;
}
</style>