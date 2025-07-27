<template>
  <a-layout-header class="board-header glass">
    <div class="header-content">
      <div class="title-section">
        <h1 class="board-title">{{ board.title }}</h1>
        <a-button
          type="text"
          size="small"
          class="star-button"
          :class="{ starred: isStarred }"
          :aria-label="t('board.header.starBoard')"
          @click="toggleStar"
        >
          <template #icon>
            <StarFilled v-if="isStarred" />
            <StarOutlined v-else />
          </template>
        </a-button>
        <a-tag class="visibility-tag" :color="visibilityColor">
          {{ t(`board.visibility.personal`) }}
        </a-tag>
      </div>
      
      <div class="actions-section">
        <a-button
          type="text"
          size="small"
          class="filter-button"
          :class="{ active: showFilters }"
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
          class="share-button"
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
            class="menu-button"
            :aria-label="t('board.header.boardMenu')"
          >
            <template #icon>
              <MoreOutlined />
            </template>
          </a-button>
          <template #overlay>
            <a-menu class="board-menu">
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
      </div>
    </div>
  </a-layout-header>
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

<style scoped lang="scss">
// Use the shared header height variable so that the board header aligns with
// other pages.  The background is semi‑opaque white with a slight blur and
// subtle border to separate it from the content.  Shadow values mirror those
// used on the boards overview page.
.board-header {
  height: var(--header-height);
  padding: 0 16px;
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border: none;
  border-bottom: 1px solid rgba(0, 0, 0, 0.06);
  box-shadow: var(--shadow-light);
  z-index: 100;

  .header-content {
    display: flex;
    align-items: center;
    justify-content: space-between;
    height: 100%;
    padding: 0 var(--content-padding, 32px);
    gap: 24px;
  }
}

.title-section {
  display: flex;
  align-items: center;
  gap: 12px;

  .board-title {
    font-size: 24px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
  }

  .star-button {
    color: var(--color-text-weak);
    transition: var(--transition-smooth);
    border-radius: 6px;
    
    &:hover {
      background: rgba(0, 0, 0, 0.04);
      color: var(--color-primary-500);
    }
    
    &.starred {
      color: #fbbf24;
    }
  }

  .visibility-tag {
    background: rgba(var(--color-primary-rgb), 0.1);
    color: var(--color-primary-600);
    border: 1px solid rgba(var(--color-primary-rgb), 0.2);
    border-radius: 12px;
    font-size: 12px;
    font-weight: 500;
  }
}

.actions-section {
  display: flex;
  align-items: center;
  gap: 12px;

  .filter-button {
    display: flex;
    align-items: center;
    gap: 6px;
    background: rgba(0, 0, 0, 0.04);
    border: 1px solid rgba(0, 0, 0, 0.06);
    color: var(--color-text);
    border-radius: 6px;
    font-size: 14px;
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(0, 0, 0, 0.06);
      border-color: rgba(0, 0, 0, 0.1);
    }
    
    &.active {
      background: rgba(var(--color-primary-rgb), 0.1);
      border-color: var(--color-primary-300);
      color: var(--color-primary-600);
    }
  }

  .share-button {
    display: flex;
    align-items: center;
    gap: 6px;
    background: var(--color-blue-gradient);
    border: none;
    border-radius: 6px;
    font-size: 14px;
    font-weight: 500;
    box-shadow: var(--shadow-light);
    transition: var(--transition-smooth);
    
    &:hover {
      transform: translateY(-1px);
      box-shadow: var(--shadow-medium);
    }
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

:deep(.board-menu) {
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
  .board-header {
    .header-content {
      padding: 0 16px;
      flex-direction: column;
      gap: 12px;
      height: auto;
      padding-top: 12px;
      padding-bottom: 12px;
    }
    
    .title-section {
      justify-content: center;
    }
    
    .actions-section {
      justify-content: center;
      flex-wrap: wrap;
    }
  }
}
</style>