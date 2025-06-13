<template>
  <a-layout class="board-page">
    <a-layout-sider width="220" class="sider glass">
      <Sidebar />
    </a-layout-sider>

    <a-layout style="background: transparent;">
      <a-layout-header class="header glass">
        <!-- <div class="brand">
          <h1>{{ t('boards.page.title') }}</h1>
        </div> -->
        <h1 class="page-title">{{ t('boards.page.title') }}</h1>
        <SearchBar v-model="search" /><!-- style="width: max-content;"  -->
        <!-- <section class="sort-desktop"></section> -->
        <!-- <SortDropdown v-model:value="sortOrder" class="sort-desktop" /> -->
        <bell-outlined class="bell" />
        <a-avatar size="large">
          <template #icon>
            <UserOutlined />
          </template>
        </a-avatar>
      </a-layout-header>

      <a-layout-content class="content">
        <!-- <h1>{{ t('boards.page.title') }}</h1> -->
        <h2 class="subtitle">{{ t('boards.page.recentlyViewed') }}</h2>
        <div class="boards-grid">
          <BoardCard v-for="board in filteredBoards" :key="board.id" :board="board" @open="openBoard" />
          <CreateBoardCard @create="createBoard" />
        </div>
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import Sidebar from '@/presentation/shared/components/SideBar.vue'
import SearchBar from '@/presentation/shared/components/SearchBar.vue'
// import SortDropdown from '@/components/SortDropdown.vue'
import BoardCard from '@/presentation/boards/components/BoardCard.vue'
import CreateBoardCard from '@/presentation/boards/components/CreateBoardCard.vue'
import { BellOutlined, UserOutlined } from '@ant-design/icons-vue'

interface Board { id: number; title: string; thumbnailUrl: string }
const { t } = useI18n()
const router = useRouter()
const search = ref('')
const sortOrder = ref('recent')
const boards = ref<Board[]>([
  { id: 1, title: 'Тестирование', thumbnailUrl: 'https://embed-ssl.wistia.com/deliveries/d5ae8190f0aa7dfbe0b01f336f29d44094b967b5.webp?image_crop_resized=1280x720' },
  { id: 2, title: '1-on-1 Meeting Agenda', thumbnailUrl: 'https://embed-ssl.wistia.com/deliveries/d5ae8190f0aa7dfbe0b01f336f29d44094b967b5.webp?image_crop_resized=1280x720' },
  { id: 3, title: 'Тестирование', thumbnailUrl: 'https://embed-ssl.wistia.com/deliveries/d5ae8190f0aa7dfbe0b01f336f29d44094b967b5.webp?image_crop_resized=1280x720' },
])
const filteredBoards = computed(() => {
  const list = boards.value.filter(b => b.title.toLowerCase().includes(search.value.toLowerCase()))
  if (sortOrder.value === 'alphabet') list.sort((a, b) => a.title.localeCompare(b.title))
  return list
})
const openBoard = (id: number) => router.push({ name: 'BoardDetail', params: { id } })
const createBoard = () => router.push({ name: 'BoardCreate' })
</script>

<style scoped lang="scss">
.board-page {
  height: 100vh;
  background: var(--color-bg-gradient);
}

.sider {
  // background: var(--color-surface);
  background: transparent;
  // filter: blur(10px);
}

.header {
  display: grid;
  grid-auto-flow: column;
  grid-template-columns: 100px 1fr auto auto;
  align-items: center;
  padding: 0 24px;
  padding-top: 16px;
  height: 80px;
  gap: 16px;
  // background: var(--color-surface);
  // border-bottom: 1px solid var(--color-stroke);
  color: var(--color-text);
  box-shadow: 10px 3px 15px rgba(0, 0, 0, 0.3019607843);
  border-left: 1px solid #cdcdcd;
}

.brand {
  display: flex;
  align-items: center;
  margin-right: 24px;
}

.page-title {
  font-size: 32px;
  margin: 0;
  margin-right: 16px;
  font-family: Sofia Sans Extra Condensed;
  letter-spacing: 0.5px;
}

.brand img {
  width: 188px;
  margin-right: 8px;
}

.brand-name {
  color: var(--color-primary-500);
  font-weight: 600;
}

.bell {
  font-size: 20px;
  margin: 0 12px;
  cursor: pointer;
  color: var(--color-text-weak);
}

.content {
  padding: 24px;
  overflow: auto;
  background: var(--color-bg-gradient);
}

.subtitle {
  margin: 16px 0 8px;
  font-size: 24px;
  color: var(--color-text);
  font-family: Sofia Sans Extra Condensed;
}

.sort-desktop {
  margin-left: 24px;
}

.boards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 16px;
}
</style>