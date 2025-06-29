<script setup lang="ts">
import { ref, onMounted } from 'vue'
import {
  HomeOutlined,
  AppstoreOutlined,
  SettingOutlined,
  MoreOutlined,
  UnorderedListOutlined,
  ProfileOutlined
} from '@ant-design/icons-vue'
import { Spin } from 'ant-design-vue'
import ProjectLogo from './ProjectLogo.vue'
import { useI18n } from 'vue-i18n'
import { tryInjectServices } from '@/shared/services/utils'
import { ProvidedService } from '@/shared/services/ProvidedService'
import { useUsersComposable } from '@/application/users/hooks/useUsers'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import type { UsersStore } from '@/application/users/stores/usersStore'
import type { UserDto } from '@/dataAccess/users/models'
import { useAsyncState } from '@vueuse/core'

interface MenuItem {
  path: string
  label: string
  icon: unknown
}

const { t } = useI18n()

// Services injection
const container = tryInjectServices()
const apiClient = container.resolve(ApiClient)
const usersStore = container.resolve<UsersStore>(ProvidedService.UsersStore)
const { getCurrentUser } = useUsersComposable(apiClient, usersStore)

// Component state
const isOpen = ref(false)
const isMobile = ref(false)
const isMobileOpen = ref(false)
const year = new Date().getFullYear()

// Current user state
const { state: currentUser, isLoading: isUserLoading, execute: loadCurrentUser } = useAsyncState(
  () => getCurrentUser(),
  null as UserDto | null,
  { immediate: false }
)

const toggleSidebar = () => {
  if (isMobile.value) {
    isMobileOpen.value = !isMobileOpen.value
  } else {
    isOpen.value = !isOpen.value
  }
}

// Load current user on component mount
onMounted(async () => {
  try {
    await loadCurrentUser()
  } catch (error) {
    console.error('Failed to load current user:', error)
  }
})

const menu: MenuItem[] = [
  { path: '/boards', label: 'Dashboard', icon: HomeOutlined },
  { path: '/boards', label: t('mainLayout.sidebar.boards'), icon: AppstoreOutlined },
  { path: '/boards', label: 'Settings', icon: SettingOutlined },
  { path: '/boards', label: t('mainLayout.sidebar.actions'), icon: ProfileOutlined },
  { path: '/boards', label: t('mainLayout.sidebar.cards'), icon: UnorderedListOutlined },
]
</script>

<template>
  <!-- Mobile overlay -->
  <!-- <div v-if="isMobileOpen" class="mobile-overlay" @click="closeMobileSidebar"></div> -->

  <!-- Burger button (mobile) -->
  <!-- <button class="burger" @click="toggleSidebar">☰</button> -->

  <aside class="sidebar" :class="{ 'sidebar--open': isOpen, 'mobile-open': isMobileOpen }" @click.self="toggleSidebar">
    <!-- Logo -->
    <ProjectLogo size="middle" class="logo" />
    <!-- <div class="logo">
      <div class="logo__icon">
        O
      </div>
      <h1 class="logo__text">Organetto</h1>
      <button v-if="isMobile" class="mobile-close-btn" @click="closeMobileSidebar">
        <CloseOutlined />
      </button>
    </div> -->

    <!-- Navigation -->
    <nav class="sidebar__nav">
      <!-- <RouterLink v-for="item in menu" :key="item.path" :to="item.path" class="sidebar__link"
        active-class="sidebar__link--active">
        <component :is="item.icon" class="sidebar__icon" />
        {{ item.label }}
      </RouterLink> -->
      <a v-for="item in menu" :key="item.path" :to="item.path" class="sidebar__link"
        active-class="sidebar__link--active">
        <component :is="item.icon" class="sidebar__icon" />
        {{ item.label }}
      </a>
    </nav>

    <!-- User Profile -->
    <div class="user" v-if="!isUserLoading">
      <div class="user__avatar">
        {{ currentUser?.name?.charAt(0)?.toUpperCase() || '?' }}
      </div>
      <div class="user__info">
        <p class="user__name">{{ currentUser?.name || 'Пользователь' }}</p>
        <p class="user__email">{{ currentUser?.email || 'email@example.com' }}</p>
      </div>
      <section class="user__menu">
        <MoreOutlined />
      </section>
    </div>

    <!-- Loading state for user profile -->
     <div class="user user--loading" v-else>
       <div class="user__avatar user__avatar--loading">
         <Spin size="small" />
       </div>
       <div class="user__info">
         <p class="user__name user__name--loading"></p>
         <p class="user__email user__email--loading"></p>
       </div>
       <section class="user__menu">
         <MoreOutlined />
       </section>
     </div>

    <!-- Footer -->
    <footer class="sidebar__footer">
      <p>&copy; {{ year }} Organetto</p>
    </footer>
  </aside>
</template>

<style scoped lang="scss">
// ---- variables ----
$sidebar-width: 240px;

// ---- burger button ----
.burger {
  position: fixed;
  top: 1rem;
  left: 1rem;
  z-index: 1001;
  background: var(--color-surface);
  border: 1px solid var(--color-stroke);
  border-radius: 0.5rem;
  padding: 0.5rem;
  font-size: 1.25rem;
  cursor: pointer;
  backdrop-filter: blur(10px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  color: var(--color-text);

  &:hover {
    background: var(--color-primary-500);
    color: white;
    transform: scale(1.05);
  }

  @media (min-width: 769px) {
    display: none;
  }
}

// ---- main sidebar wrapper ----
.sidebar {
  height: 100vh;
  background-image: url(https://colorlib.com/etc/bootstrap-sidebar/sidebar-06/images/bg_1.jpg);
  // background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border-right: 1px solid var(--color-stroke);
  position: relative;
  overflow: hidden;
  z-index: 10;
  width: $sidebar-width;
  color: #333;
  display: grid;
  grid-template-rows: auto 1fr auto auto;
  // transform: translateX(-100%);
  box-shadow: 4px 0 16px rgba(0, 0, 0, 0.15);

  // @media (max-width: 768px) {
  //   position: fixed;
  //   left: -280px;
  //   top: 0;
  //   z-index: 1000;
  //   box-shadow: 2px 0 8px rgba(0, 0, 0, 0.15);

  //   &.mobile-open {
  //     left: 0;
  //   }
  // }

  &--open {
    transform: translateX(0);
  }

  &::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: linear-gradient(180deg,
        rgba(234, 242, 255, 0.6) 0%,
        rgba(255, 255, 255, 0.8) 50%,
        rgba(253, 245, 229, 1) 100%);
    z-index: -1;
  }
}

// Mobile overlay
.mobile-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  z-index: 999;
  backdrop-filter: blur(4px);
}

// Mobile close button
.mobile-close-btn {
  background: none;
  border: none;
  color: var(--color-text-weak);
  font-size: 1.25rem;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 0.25rem;
  transition: all 0.2s ease;
  margin-left: auto;

  &:hover {
    background: rgba(var(--color-primary-rgb), 0.1);
    color: var(--color-text);
  }
}

// Dark theme styles
[data-theme='dark'] .sidebar {
  background: rgba(30, 30, 38, 0.95);
  border-right: 1px solid var(--color-stroke);
  color: var(--color-text);

  &::before {
    background: linear-gradient(180deg,
        rgba(11, 22, 51, 0.6) 0%,
        rgba(30, 30, 38, 0.8) 50%,
        rgba(19, 36, 92, 0.6) 100%);
  }
}

.sidebar:after {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  content: '';
  // background: linear-gradient(255deg, #96beff 40%, rgb(255 255 255 / 43%) 60%, #ffe29f 100%);
  background: linear-gradient(255deg, #ffffffcf 40%, rgb(255 255 255 / 47%) 60%, #ffe29f 100%);

  opacity: .8;
  z-index: -1;
}

// ---- logo ----
.logo {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1.3rem 1.8rem;
  border-bottom: 1px solid var(--color-stroke);

  &__icon {
    width: 2.5rem;
    height: 2.5rem;
    border-radius: 50%;
    background: var(--color-primary-500);
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 700;
    font-size: 1.25rem;
    color: white;
  }

  &__text {
    font-size: 1.25rem;
    margin: 0;
    font-weight: 700;
    color: var(--color-text);
    flex: 1;
  }
}

.sidebar__close {
  background: none;
  border: none;
  color: #fff;
  font-size: 1.5rem;
  line-height: 1;
  cursor: pointer;
}

// ---- navigation ----
.sidebar__nav {
  padding: 1rem 0;
  overflow-y: auto;
}

.sidebar__link {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem 1rem;
  margin: 0 1rem 0.25rem;
  color: var(--color-text);
  text-decoration: none;
  border-radius: 0.5rem;
  transition: all 0.2s ease;

  &:hover {
    background: rgba(var(--color-primary-rgb, 49, 91, 255), 0.08);
    color: var(--color-text);
  }

  &--active {
    background: rgba(var(--color-primary-rgb, 49, 91, 255), 0.12);
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
    color: var(--color-primary-600);
    font-weight: 600;

    .sidebar__icon {
      color: var(--color-primary-600);
      opacity: 1;
    }
  }
}

.sidebar__icon {
  width: 1.25rem;
  height: 1.25rem;
  opacity: 0.7;
  color: var(--color-text-weak);
  transition: all 0.2s ease;
}

// ---- user profile ----
.user {
  padding: 1rem 1rem;
  border-top: 1px solid var(--color-stroke);
  display: flex;
  align-items: center;
  gap: 0.5rem;

  &__avatar {
    width: 2.5rem;
    height: 2.5rem;
    border-radius: 50%;
    background: var(--color-primary-500);
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 600;
    font-size: 1rem;
    color: white;
  }

  &__info {
    flex: 1;
    min-width: 0;
  }

  &__name {
    font-weight: 600;
    font-size: 0.875rem;
    margin: 0;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    color: var(--color-text);
  }

  &__email {
    font-size: 0.75rem;
    margin: 0;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    color: var(--color-text-weak);
  }

  &__menu {
    color: var(--color-text-weak);
    transition: color 0.2s ease;

    &:hover {
      color: var(--color-text);
    }
  }

  // Loading states
  &--loading {
    opacity: 0.7;
  }

  &__avatar--loading {
    position: relative;
    background: var(--color-surface-variant);
    display: flex;
    align-items: center;
    justify-content: center;
  }

  &__name--loading,
  &__email--loading {
    background: var(--color-surface-variant);
    border-radius: 0.25rem;
    height: 1rem;
    animation: pulse 1.5s ease-in-out infinite;
  }

  &__name--loading {
    width: 80%;
    margin-bottom: 0.25rem;
  }

  &__email--loading {
    width: 60%;
    height: 0.75rem;
  }
}

// ---- footer ----
.sidebar__footer {
  padding: 1rem 1.25rem;
  text-align: center;

  p {
    margin: 0;
    font-size: 0.75rem;
    color: var(--color-text-weak);
  }
}

// ---- animations ----
@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: 0.5;
  }
}

// ---- responsiveness ----
// @media (min-width: 1024px) {
//   .burger {
//     display: none;
//   }

//   .sidebar {
//     transform: translateX(0);
//   }
// }</style>
