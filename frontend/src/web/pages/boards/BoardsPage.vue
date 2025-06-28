<template>
  <a-layout class="board-page" v-if="boadrdPageViews.state.value">
    <a-layout-sider width="280" class="sider glass">
      <Sidebar />
    </a-layout-sider>

    <a-layout class="main-layout">
      <a-layout-header class="header glass">
        <div class="header-content">
          <div class="welcome-section">
            <h1 class="page-title">{{ t('boards.page.title') }}</h1>
            <p class="page-subtitle">Управляйте своими проектами эффективно</p>
          </div>
          
          <div class="search-section">
            <SearchBar v-model="boadrdPageViews.state.value.searchTerms.value" class="enhanced-search" />
          </div>
          
          <div class="user-section">
            <a-tooltip title="Уведомления">
              <a-badge :count="3" size="small">
                <bell-outlined class="bell" />
              </a-badge>
            </a-tooltip>
            <a-dropdown placement="bottomRight">
              <a-avatar size="large" class="user-avatar">
                <template #icon>
                  <UserOutlined />
                </template>
              </a-avatar>
              <template #overlay>
                <a-menu>
                  <a-menu-item key="profile">
                    <UserOutlined /> Профиль
                  </a-menu-item>
                  <a-menu-item key="settings">
                    <SettingOutlined /> Настройки
                  </a-menu-item>
                  <a-menu-divider />
                  <a-menu-item key="logout">
                    <LogoutOutlined /> Выйти
                  </a-menu-item>
                </a-menu>
              </template>
            </a-dropdown>
          </div>
        </div>
      </a-layout-header>

      <a-layout-content class="content">
        <div class="content-wrapper">
          <div class="section-header">
            <h2 class="subtitle">
              <ClockCircleOutlined class="subtitle-icon" />
              {{ t('boards.page.recentlyViewed') }}
            </h2>
            <div class="section-actions">
              <a-button type="text" size="small" class="view-all-btn">
                Показать все
                <ArrowRightOutlined />
              </a-button>
            </div>
          </div>
          
          <div class="boards-container">
            <transition-group name="board-list" tag="div" class="boards-grid">
              <BoardCard 
                v-for="board in boadrdPageViews.state.value.views.value" 
                :key="board.id" 
                :board="board"
                @open="openBoard" 
                class="board-item"
              />
              <CreateBoardCard 
                @create="createBoard" 
                class="board-item create-item"
              />
            </transition-group>
          </div>
          
          <!-- Дополнительные секции -->
          <div class="additional-sections">
            <div class="quick-actions">
              <h3 class="section-title">Быстрые действия</h3>
              <div class="action-cards">
                <a-card class="action-card" hoverable>
                  <template #cover>
                    <div class="action-icon">
                      <PlusOutlined />
                    </div>
                  </template>
                  <a-card-meta title="Создать доску" description="Начните новый проект" />
                </a-card>
                <a-card class="action-card" hoverable>
                  <template #cover>
                    <div class="action-icon">
                      <TeamOutlined />
                    </div>
                  </template>
                  <a-card-meta title="Пригласить команду" description="Добавьте участников" />
                </a-card>
                <a-card class="action-card" hoverable>
                  <template #cover>
                    <div class="action-icon">
                      <FileTextOutlined />
                    </div>
                  </template>
                  <a-card-meta title="Шаблоны" description="Используйте готовые шаблоны" />
                </a-card>
              </div>
            </div>
          </div>
        </div>
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import Sidebar from '@/presentation/shared/components/SideBar.vue'
import SearchBar from '@/presentation/shared/components/SearchBar.vue'
// import SortDropdown from '@/components/SortDropdown.vue'
import BoardCard from '@/presentation/boards/components/BoardCard.vue'
import CreateBoardCard from '@/presentation/boards/components/CreateBoardCard.vue'
import { 
  BellOutlined, 
  UserOutlined, 
  SettingOutlined, 
  LogoutOutlined, 
  ClockCircleOutlined, 
  ArrowRightOutlined, 
  PlusOutlined, 
  TeamOutlined, 
  FileTextOutlined 
} from '@ant-design/icons-vue'
import { useBoardPageViews } from '@/presentation/boards/hooks/useBoardPageViews'
import { useAsyncState } from '@vueuse/core'
import { tryInjectServices } from '@/shared'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import _ from 'lodash'

const props = defineProps<{
  userId: string,
}>()

const { t } = useI18n()
const apiClient = tryInjectServices().resolve(ApiClient)
const router = useRouter()
const boadrdPageViews = useAsyncState(useBoardPageViews(apiClient), null, { immediate: false })

// const search = ref('')
// const sortOrder = ref('recent')
// const boards = ref<BoardPageView[]>([
//   { id: 1, title: 'Тестирование', thumbnailUrl: defaultThumbnailHref },
//   { id: 2, title: '1-on-1 Meeting Agenda', thumbnailUrl: defaultThumbnailHref },
//   { id: 3, title: 'Тестирование', thumbnailUrl: defaultThumbnailHref },
// ])
// const filteredBoards = computed(() => {
//   const list = boards.value.filter(b => b.title.toLowerCase().includes(search.value.toLowerCase()))
//   if (sortOrder.value === 'alphabet') list.sort((a, b) => a.title.localeCompare(b.title))
//   return list
// })

function startUp() {
  const userId = _.parseInt(props.userId)
  if (!userId) return
  boadrdPageViews.execute(0, { userId })
}

startUp()

const openBoard = (id: number) => router.push({ name: 'Board', params: { id, userId: props.userId } })
const createBoard = () => {
  // router.push({ name: 'BoardCreate' })
}

</script>

<style scoped lang="scss">
// Основные переменные
:root {
  --header-height: 120px;
  --sidebar-width: 280px;
  --content-padding: 32px;
  --border-radius: 12px;
  --shadow-light: 0 2px 8px rgba(0, 0, 0, 0.06);
  --shadow-medium: 0 4px 16px rgba(0, 0, 0, 0.12);
  --shadow-heavy: 0 8px 32px rgba(0, 0, 0, 0.18);
  --transition-smooth: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.board-page {
  height: 100vh;
  background: var(--color-bg-gradient);
  overflow: hidden;
}

.sider {
  background: transparent;
  backdrop-filter: blur(20px);
  border-right: 1px solid rgba(255, 255, 255, 0.1);
  z-index: 10;
}

.main-layout {
  background: transparent;
  position: relative;
}

.header {
  height: var(--header-height);
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border: none;
  border-bottom: 1px solid rgba(0, 0, 0, 0.06);
  box-shadow: var(--shadow-light);
  padding: 0;
  z-index: 100;
  
  .header-content {
    display: grid;
    grid-template-columns: 1fr auto 1fr;
    align-items: center;
    height: 100%;
    padding: 0 var(--content-padding);
    gap: 24px;
  }
}

.welcome-section {
  .page-title {
    font-size: 28px;
    font-weight: 700;
    margin: 0;
    color: var(--color-text);
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: -0.5px;
    line-height: 1.2;
  }
  
  .page-subtitle {
    font-size: 14px;
    color: var(--color-text-weak);
    margin: 4px 0 0 0;
    font-weight: 400;
  }
}

.search-section {
  display: flex;
  justify-content: center;
  
  .enhanced-search {
    width: 400px;
    
    :deep(.ant-input-search) {
      border-radius: 24px;
      box-shadow: var(--shadow-light);
      border: 1px solid rgba(0, 0, 0, 0.06);
      
      .ant-input {
        border-radius: 24px 0 0 24px;
        border: none;
        padding: 12px 16px;
        font-size: 14px;
        
        &:focus {
          box-shadow: none;
        }
      }
      
      .ant-btn {
        border-radius: 0 24px 24px 0;
        border: none;
        background: var(--color-primary-500);
        
        &:hover {
          background: var(--color-primary-600);
        }
      }
    }
  }
}

.user-section {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 16px;
  
  .bell {
    font-size: 20px;
    cursor: pointer;
    color: var(--color-text-weak);
    padding: 8px;
    border-radius: 50%;
    transition: var(--transition-smooth);
    
    &:hover {
      background: rgba(0, 0, 0, 0.04);
      color: var(--color-primary-500);
    }
  }
  
  .user-avatar {
    cursor: pointer;
    transition: var(--transition-smooth);
    box-shadow: var(--shadow-light);
    
    &:hover {
      transform: translateY(-1px);
      box-shadow: var(--shadow-medium);
    }
  }
}

.content {
  padding: 0;
  overflow: auto;
  background: transparent;
  height: calc(100vh - var(--header-height));
}

.content-wrapper {
  padding: var(--content-padding);
  max-width: 1400px;
  margin: 0 auto;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
  
  .subtitle {
    display: flex;
    align-items: center;
    gap: 12px;
    font-size: 24px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    
    .subtitle-icon {
      color: var(--color-primary-500);
      font-size: 20px;
    }
  }
  
  .view-all-btn {
    color: var(--color-primary-500);
    font-weight: 500;
    
    &:hover {
      background: rgba(var(--color-primary-rgb), 0.04);
    }
  }
}

.boards-container {
  margin-bottom: 48px;
}

.boards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 20px;
  
  .board-item {
    transition: var(--transition-smooth);
    
    &:hover {
      transform: translateY(-4px);
    }
  }
}

// Анимации для списка досок
.board-list-enter-active,
.board-list-leave-active {
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

.board-list-enter-from {
  opacity: 0;
  transform: translateY(20px) scale(0.95);
}

.board-list-leave-to {
  opacity: 0;
  transform: translateY(-20px) scale(0.95);
}

.board-list-move {
  transition: transform 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

// Дополнительные секции
.additional-sections {
  .section-title {
    font-size: 20px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0 0 16px 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
  }
}

.quick-actions {
  .action-cards {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 16px;
    
    .action-card {
      border-radius: var(--border-radius);
      border: 1px solid rgba(0, 0, 0, 0.06);
      transition: var(--transition-smooth);
      
      &:hover {
        transform: translateY(-2px);
        box-shadow: var(--shadow-medium);
        border-color: var(--color-primary-200);
      }
      
      .action-icon {
        display: flex;
        align-items: center;
        justify-content: center;
        height: 80px;
        background: linear-gradient(135deg, var(--color-primary-50), var(--color-primary-100));
        color: var(--color-primary-500);
        font-size: 24px;
      }
      
      :deep(.ant-card-meta-title) {
        font-weight: 600;
        color: var(--color-text);
      }
      
      :deep(.ant-card-meta-description) {
        color: var(--color-text-weak);
      }
    }
  }
}

// Адаптивность
@media (max-width: 1200px) {
  .header-content {
    grid-template-columns: 1fr;
    gap: 16px;
    text-align: center;
  }
  
  .search-section .enhanced-search {
    width: 100%;
    max-width: 400px;
  }
  
  .user-section {
    justify-content: center;
  }
}

@media (max-width: 768px) {
  .content-wrapper {
    padding: 16px;
  }
  
  .boards-grid {
    grid-template-columns: 1fr;
    gap: 16px;
  }
  
  .action-cards {
    grid-template-columns: 1fr;
  }
  
  .header {
    height: auto;
    min-height: 100px;
    
    .header-content {
      padding: 16px;
    }
  }
}

// Улучшения для glass эффекта
.glass {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.2);
}
</style>