<template>
  <a-layout class="board-page" v-if="boardPageViews.state.value">
    <a-layout-sider width="240"><!-- class="sider glass" -->
      <Sidebar />
    </a-layout-sider>

    <a-layout class="main-layout">
      <a-layout-header class="header glass">
        <div class="header-content">
          <div class="welcome-section">
            <!-- <h1 class="page-title">{{ t('boards.page.title') }}</h1> -->
            <h1 class="page-subtitle one-line-text">{{ t('boards.page.subtitle') }}</h1>
          </div>

          <!-- Фильтры и сортировка -->
          <div class="filters-section">
            <div class="filters-left">
              <a-button-group>
                <a-button type="primary" size="small">{{ t('boards.page.allBoards') }}</a-button>
                <a-button size="small">{{ t('boards.page.recent') }}</a-button>
                <a-button size="small">{{ t('boards.page.favorites') }}</a-button>
                <a-button size="small">{{ t('boards.page.archive') }}</a-button>
              </a-button-group>
            </div>
            <div class="filters-right">
              <a-select size="small" style="width: 120px" :placeholder="t('boards.sort.recent')">
                <a-select-option value="recent">{{ t('boards.sort.recent') }}</a-select-option>
                <a-select-option value="name">{{ t('boards.sort.name') }}</a-select-option>
                <a-select-option value="activity">{{ t('boards.sort.activity') }}</a-select-option>
              </a-select>
              <a-button size="small" type="text">
                <AppstoreOutlined />
              </a-button>
              <a-button size="small" type="text">
                <UnorderedListOutlined />
              </a-button>
            </div>
          </div>

          <div class="user-section">
            <a-tooltip :title="t('boards.notifications.tooltip')">
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
                    <UserOutlined /> {{ t('boards.user.profile') }}
                  </a-menu-item>
                  <a-menu-item key="settings">
                    <SettingOutlined /> {{ t('boards.user.settings') }}
                  </a-menu-item>
                  <a-menu-divider />
                  <a-menu-item key="logout">
                    <LogoutOutlined /> {{ t('boards.user.logout') }}
                  </a-menu-item>
                </a-menu>
              </template>
            </a-dropdown>
          </div>
        </div>
      </a-layout-header>

      <a-layout-content class="content">
        <div class="content-wrapper">
          <!-- Статистика и метрики -->
          <!-- <div class="stats-section">
            <div class="stats-grid">
              <div class="stat-card">
                <div class="stat-icon">
                  <DashboardOutlined />
                </div>
                <div class="stat-content">
                  <div class="stat-number">{{ boadrdPageViews.state.value?.views.value?.length || 0 }}</div>
                  <div class="stat-label">Активных досок</div>
                </div>
              </div>
              <div class="stat-card">
                <div class="stat-icon">
                  <TeamOutlined />
                </div>
                <div class="stat-content">
                  <div class="stat-number">12</div>
                  <div class="stat-label">Участников</div>
                </div>
              </div>
              <div class="stat-card">
                <div class="stat-icon">
                  <CheckCircleOutlined />
                </div>
                <div class="stat-content">
                  <div class="stat-number">47</div>
                  <div class="stat-label">Завершено задач</div>
                </div>
              </div>
              <div class="stat-card">
                <div class="stat-icon">
                  <TrophyOutlined />
                </div>
                <div class="stat-content">
                  <div class="stat-number">8</div>
                  <div class="stat-label">Проектов</div>
                </div>
              </div>
            </div>
          </div> -->

          <div class="section-header">
            <h2 class="subtitle">
              <ClockCircleOutlined class="subtitle-icon" />
              {{ t('boards.page.recentlyViewed') }}
            </h2>
            <div class="section-actions">
              <a-tooltip :title="t('boards.page.create')">
                <a-button type="primary" size="small" class="create-btn" @click="showCreateBoardModal">
                  <PlusOutlined />
                  {{ t('boards.page.create') }}
                </a-button>
              </a-tooltip>
              <a-button type="text" size="small" class="view-all-btn">
                {{ t('boards.page.showAll') }}
                <ArrowRightOutlined />
              </a-button>
            </div>
          </div>

          <div class="boards-container">
            <!-- {{ boadrdPageViews.state.value.isConnected.value }} -->
            <transition-group name="board-list" tag="div" class="boards-grid">
              <BoardCard v-for="board in boardPageViews.state.value.views.value" :key="board.id" :board="board"
                @open="openBoard" class="board-item" />
              <CreateBoardCard @create="showCreateBoardModal" class="board-item create-item" />
            </transition-group>
          </div>

          <!-- Шаблоны и быстрые действия -->
          <div class="templates-section">
            <div class="section-header">
              <h3 class="section-title">
                <FileTextOutlined class="subtitle-icon" />
                {{ t('boards.templates.title') }}
              </h3>
              <a-button type="text" size="small">
                {{ t('boards.templates.allTemplates') }}
                <ArrowRightOutlined />
              </a-button>
            </div>
            <div class="templates-grid">
              <a-card v-for="template in templates" :key="template.id" class="template-card" hoverable size="small">
                <template #cover>
                  <div class="template-preview">
                    <component :is="template.icon" class="template-icon" />
                  </div>
                </template>

                <a-card-meta :title="template.title" :description="template.description" />

                <template #actions>
                  <a-button size="small" type="primary" block>
                    {{ t('boards.templates.use') }}
                  </a-button>
                </template>
              </a-card>
            </div>
          </div>

          <!-- Активность команды -->
          <!-- <div class="activity-section">
            <div class="section-header">
              <h3 class="section-title">
                <HistoryOutlined class="subtitle-icon" />
                {{ t('boards.activity.title') }}
              </h3>
              <a-button type="text" size="small">
                {{ t('boards.activity.viewAll') }}
                <ArrowRightOutlined />
              </a-button>
            </div>
            <div class="activity-list">
              <div class="activity-item" v-for="activity in recentActivity" :key="activity.id">
                <a-avatar size="small" :src="activity.user.avatar">
                  <UserOutlined v-if="!activity.user.avatar" />
                </a-avatar>
                <div class="activity-content">
                  <span class="activity-text">
                    <strong>{{ activity.user.name }}</strong> {{ activity.action }}
                    <a-button type="link" size="small">{{ activity.target }}</a-button>
                  </span>
                  <span class="activity-time">{{ activity.time }}</span>
                </div>
              </div>
            </div>
          </div> -->
        </div>
      </a-layout-content>
    </a-layout>
    <a-modal v-model:open="isCreateBoardModalVisible" :title="t('boards.createModal.title')" :footer="null">
      <CreateBoardForm @submit="createBoard" :loading="isCreatingBoard" />
    </a-modal>
  </a-layout>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import Sidebar from '@/presentation/shared/components/SideBar.vue'
import BoardCard from '@/presentation/boards/components/BoardCard.vue'
import CreateBoardCard from '@/presentation/boards/components/CreateBoardCard.vue'
import CreateBoardForm from '@/presentation/boards/components/CreateBoardForm.vue'
import {
  BellOutlined,
  UserOutlined,
  SettingOutlined,
  LogoutOutlined,
  ClockCircleOutlined,
  ArrowRightOutlined,
  PlusOutlined,
  // TeamOutlined,
  FileTextOutlined,
  // DashboardOutlined,
  // CheckCircleOutlined,
  // TrophyOutlined,
  AppstoreOutlined,
  UnorderedListOutlined,
  // HistoryOutlined,
  // ProjectOutlined,
  // BulbOutlined,
  // RocketOutlined,
  // CalendarOutlined
} from '@ant-design/icons-vue'
import { useRealtimeBoardPageViews } from '@/presentation/boards/hooks/useBoardPageViews'
import { useAsyncState } from '@vueuse/core'
import { tryInjectServices, useConditionalComputed } from '@/shared'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import _ from 'lodash'
import { ref } from 'vue'
import { useCreateBoard } from '@/application/boards/hooks/useCreateBoard'
import type { CreateBoardState } from '@/application'
import { mapToBoardPageView } from '@/presentation/boards'

const props = defineProps<{
  userId: string,
}>()

const { t } = useI18n()
const apiClient = tryInjectServices().resolve(ApiClient)
const router = useRouter()
const boardPageViews = useAsyncState(useRealtimeBoardPageViews(apiClient), null, { immediate: false })
const { isLoading: isCreatingBoard, execute: executeBoardCreation } = useCreateBoard(apiClient)

const isCreateBoardModalVisibleState = ref(false)

const isCreateBoardModalVisible = useConditionalComputed(
  () => isCreateBoardModalVisibleState.value,
  (v) => isCreateBoardModalVisibleState.value = v,
  () => !isCreatingBoard.value
)

function showCreateBoardModal() {
  isCreateBoardModalVisible.value = true
}

async function createBoard(state: CreateBoardState) {
  console.log('Creating board with:', state)
  const created = await executeBoardCreation(0, state)
  if (!created) {
    console.warn('Board has not been create')
    return
  }
  if (!boardPageViews.state.value?.allViews.value) {
    console.warn('boardPageViews is empty')
    return
  }
  boardPageViews.state.value.allViews.value = [
    ...boardPageViews.state.value.allViews.value,
    mapToBoardPageView(created)
  ]
  isCreateBoardModalVisible.value = false
}
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
  boardPageViews.execute(0, { userId })
    .then(r => {
      if (r) {
        return r.connect()
      }
    })
}

startUp()

const openBoard = (id: number) => router.push({ name: 'Board', params: { id, userId: props.userId } })

// Данные для шаблонов
const templates = ref([
  {
    id: 1,
    title: 'Kanban доска',
    description: 'Управление задачами в стиле Kanban',
    icon: 'ProjectOutlined'
  },
  {
    id: 2,
    title: 'Планирование спринта',
    description: 'Agile планирование и отслеживание',
    icon: 'RocketOutlined'
  },
  {
    id: 3,
    title: 'Мозговой штурм',
    description: 'Генерация и организация идей',
    icon: 'BulbOutlined'
  },
  {
    id: 4,
    title: 'Календарь проекта',
    description: 'Временные рамки и дедлайны',
    icon: 'CalendarOutlined'
  }
])

// Данные для активности
// const recentActivity = ref([
//   {
//     id: 1,
//     user: { name: 'Анна Петрова', avatar: '' },
//     action: 'создала карточку в',
//     target: 'Разработка MVP',
//     time: '2 минуты назад'
//   },
//   {
//     id: 2,
//     user: { name: 'Михаил Иванов', avatar: '' },
//     action: 'завершил задачу в',
//     target: 'Тестирование',
//     time: '15 минут назад'
//   },
//   {
//     id: 3,
//     user: { name: 'Елена Сидорова', avatar: '' },
//     action: 'добавила комментарий к',
//     target: 'UI/UX дизайн',
//     time: '1 час назад'
//   },
//   {
//     id: 4,
//     user: { name: 'Дмитрий Козлов', avatar: '' },
//     action: 'переместил карточку в',
//     target: 'Код-ревью',
//     time: '2 часа назад'
//   }
// ])

</script>
<style lang="scss">
// Основные переменные
:root {
  --header-height: 65px;
  --sidebar-width: 280px;
  --content-padding: 32px;
  --border-radius: 12px;
}
</style>
<style scoped lang="scss">
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
  display: grid;
  // gap: 16px;
}

.header {
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
    height: 100%;
    padding: 0 var(--content-padding);
    gap: 24px;
  }
}

.welcome-section {
  // padding: 0 16px;

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
    font-size: 24px;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    letter-spacing: 0px;
    font-weight: 400;
    // opacity: 1;
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
  padding: 0 16px;
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

// Статистика
.stats-section {
  margin-bottom: 32px;

  .stats-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 20px;

    .stat-card {
      background: rgba(255, 255, 255, 0.95);
      backdrop-filter: blur(20px);
      border-radius: var(--border-radius);
      padding: 24px;
      border: 1px solid rgba(0, 0, 0, 0.06);
      box-shadow: var(--shadow-light);
      transition: var(--transition-smooth);
      display: flex;
      align-items: center;
      gap: 16px;

      &:hover {
        transform: translateY(-2px);
        box-shadow: var(--shadow-medium);
      }

      .stat-icon {
        width: 48px;
        height: 48px;
        border-radius: 12px;
        background: linear-gradient(135deg, var(--color-primary-500), var(--color-primary-600));
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        font-size: 20px;
      }

      .stat-content {
        .stat-number {
          font-size: 28px;
          font-weight: 700;
          color: var(--color-text);
          line-height: 1;
          margin-bottom: 4px;
        }

        .stat-label {
          font-size: 14px;
          color: var(--color-text-weak);
          font-weight: 500;
        }
      }
    }
  }
}

// Фильтры
.filters-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex: 1;
  margin: 0 24px;
  padding: 0;
  background: transparent;

  .filters-left {
    :deep(.ant-btn-group) {
      .ant-btn {
        border-radius: 6px;
        // background: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.2);
        // color: rgba(255, 255, 255, 0.85);
        transition: all 0.3s ease;

        &:hover {
          // background: rgba(255, 255, 255, 0.2);
          border-color: rgba(255, 255, 255, 0.3);
          // color: white;
        }

        &.ant-btn-primary {
          // background: rgba(215, 225, 255, 0.904);
          border-color: rgba(255, 255, 255, 0.3);
          color: white;
        }

        &:not(:last-child) {
          margin-right: 8px;
        }
      }
    }
  }

  .filters-right {
    display: flex;
    align-items: center;
    gap: 8px;

    :deep(.ant-select) {
      .ant-select-selector {
        // background: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.2);
        color: rgba(255, 255, 255, 0.85);
      }
    }

    :deep(.ant-btn) {
      // background: rgba(255, 255, 255, 0.1);
      // border: 1px solid rgba(255, 255, 255, 0.2);
      color: rgba(54, 54, 54, 0.85);

      &:hover {
        // background: rgba(255, 255, 255, 0.2);
        border-color: rgba(255, 255, 255, 0.3);
        // color: white;
      }
    }
  }
}

// Секции
.section-header {
  .create-btn {
    // background: linear-gradient(135deg, var(--color-primary-500), var(--color-primary-600));
    background: var(--color-blue-gradient);
    border: none;
    box-shadow: var(--shadow-light);

    &:hover {
      transform: translateY(-1px);
      box-shadow: var(--shadow-medium);
    }
  }
}

// Шаблоны
.templates-section {
  margin-bottom: 48px;

  .section-title {
    font-size: 20px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    display: flex;
    align-items: center;
    gap: 12px;

    .subtitle-icon {
      color: var(--color-primary-500);
      font-size: 18px;
    }
  }

  .templates-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
    gap: 20px;
    margin-top: 16px;

    :deep(.template-card) {
      background: var(--color-surface, #ffffff);
      border-radius: 12px;
      border: none;
      overflow: hidden;
      transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);

      &:hover {
        transform: translateY(-4px);
        box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
        // background: linear-gradient(135deg, #ffffff, #f6ffed);
      }

      .ant-card-cover {
        .template-preview {
          height: 120px;
          // background: linear-gradient(135deg, var(--color-primary-50), var(--color-primary-100));
          background-image: url(https://colorlib.com/etc/bootstrap-sidebar/sidebar-09/images/bg_1.jpg);
          background-size: cover;
          background-repeat: no-repeat;
          background-position: center center;
          display: flex;
          align-items: center;
          justify-content: center;

          .template-icon {
            font-size: 32px;
            color: var(--color-primary-500);
          }
        }
      }

      .ant-card-meta {
        .ant-card-meta-title {
          font-size: 16px;
          font-weight: 600;
          color: var(--color-text);
          margin-bottom: 8px;
        }

        .ant-card-meta-description {
          font-size: 14px;
          color: var(--color-text-weak);
          line-height: 1.4;
        }
      }

      .ant-card-actions {
        background: transparent;
        border-top: 1px solid rgba(0, 0, 0, 0.06);
        padding: 12px 16px;

        li {
          margin: 0;
          width: 100%;
        }
      }
    }
  }
}

// Активность
.activity-section {
  .section-title {
    font-size: 20px;
    font-weight: 600;
    color: var(--color-text);
    margin: 0;
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    display: flex;
    align-items: center;
    gap: 12px;

    .subtitle-icon {
      color: var(--color-primary-500);
      font-size: 18px;
    }
  }

  .activity-list {
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(20px);
    border-radius: var(--border-radius);
    border: 1px solid rgba(0, 0, 0, 0.06);
    padding: 20px;
    margin-top: 16px;

    .activity-item {
      display: flex;
      align-items: flex-start;
      gap: 12px;
      padding: 12px 0;

      &:not(:last-child) {
        border-bottom: 1px solid rgba(0, 0, 0, 0.06);
      }

      .activity-content {
        flex: 1;

        .activity-text {
          font-size: 14px;
          color: var(--color-text);
          line-height: 1.4;

          strong {
            font-weight: 600;
          }
        }

        .activity-time {
          display: block;
          font-size: 12px;
          color: var(--color-text-weak);
          margin-top: 4px;
        }
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

  .user-section {
    justify-content: center;
  }

  .stats-grid {
    grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  }

  .templates-grid {
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
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

  .stats-grid {
    grid-template-columns: 1fr;
    gap: 16px;

    .stat-card {
      padding: 20px;

      .stat-icon {
        width: 40px;
        height: 40px;
        font-size: 18px;
      }

      .stat-content .stat-number {
        font-size: 24px;
      }
    }
  }

  .filters-section {
    flex-direction: column;
    gap: 16px;
    align-items: stretch;

    .filters-left {
      :deep(.ant-btn-group) {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;

        .ant-btn {
          flex: 1;
          min-width: 80px;
        }
      }
    }

    .filters-right {
      justify-content: center;
    }
  }

  .templates-grid {
    grid-template-columns: 1fr;
    gap: 16px;
  }

  .activity-list {
    padding: 16px;

    .activity-item {
      flex-direction: column;
      align-items: flex-start;
      gap: 8px;

      .activity-content {
        .activity-text {
          font-size: 13px;
        }
      }
    }
  }

  .section-header {
    flex-direction: column;
    gap: 16px;
    align-items: flex-start;

    .section-actions {
      display: flex;
      gap: 8px;
      width: 100%;

      .create-btn {
        flex: 1;
      }
    }
  }

  .header {
    height: auto;
    min-height: 100px;

    .header-content {
      padding: 16px;
    }
  }
}

@media (max-width: 480px) {
  .stats-grid {
    .stat-card {
      flex-direction: column;
      text-align: center;
      gap: 12px;
    }
  }

  .template-card {
    .template-info {
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