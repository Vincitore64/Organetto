<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink } from 'vue-router'
import {
  HomeOutlined,
  AppstoreOutlined,
  SettingOutlined
} from '@ant-design/icons-vue'
// import ProjectLogo from './ProjectLogo.vue'

const isOpen = ref(false)
const year = new Date().getFullYear()

interface MenuItem {
  path: string
  label: string
  icon: unknown
}

const menu: MenuItem[] = [
  { path: '/dashboard', label: 'Dashboard', icon: HomeOutlined },
  { path: '/boards', label: 'Boards', icon: AppstoreOutlined },
  { path: '/settings', label: 'Settings', icon: SettingOutlined }
]

function toggleSidebar() {
  isOpen.value = !isOpen.value
}
</script>

<template>
  <!-- Burger button (mobile) -->
  <button class="burger" @click="toggleSidebar">☰</button>

  <aside class="sidebar" :class="{ 'sidebar--open': isOpen }" @click.self="toggleSidebar">
    <!-- Brand / logo -->
    <header class="sidebar__brand">
      <!-- <img class="sidebar__logo" src="/logo.svg" alt="logo" /> -->
      <!-- <ProjectLogo class="sidebar__logo" size="middlePlus" /> -->
      <h1 class="sidebar__title">Organetto</h1>
      <!-- <button class="sidebar__close" @click="toggleSidebar">×</button> -->
    </header>

    <!-- Navigation -->
    <nav class="sidebar__nav">
      <RouterLink v-for="item in menu" :key="item.path" :to="item.path" class="sidebar__link"
        active-class="sidebar__link--active">
        <component :is="item.icon" class="sidebar__icon" />
        <span>{{ item.label }}</span>
      </RouterLink>
    </nav>

    <!-- Subscribe block (decorative, like in Colorlib sidebar) -->
    <section class="sidebar__subscribe">
      <p class="sidebar__subscribe-title">Subscribe for newsletter</p>
      <form class="sidebar__subscribe-form" @submit.prevent="() => { }">
        <input type="email" placeholder="Email" required class="sidebar__subscribe-input" />
        <button type="submit" class="sidebar__subscribe-btn">Join</button>
      </form>
    </section>

    <footer class="sidebar__footer">© {{ year }} Organetto</footer>
  </aside>
</template>

<style scoped lang="scss">
// ---- variables (you can move to a global SCSS map) ----
$sidebar-bg-from: #315bff; // primary‑500
$sidebar-bg-to: #9b4dff; // accent
$sidebar-width: 280px;
$z-sidebar: 30;
$transition: 0.3s cubic-bezier(0.4, 0, 0.2, 1);

// ---- burger button ----
.burger {
  position: fixed;
  top: 1rem;
  left: 1rem;
  z-index: $z-sidebar + 1;
  width: 2.5rem;
  height: 2.5rem;
  border: none;
  border-radius: 50%;
  background: $sidebar-bg-from;
  color: #fff;
  font-size: 1.25rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  cursor: pointer;
  display: grid;
  place-content: center;
  transition: background $transition;

  &:hover {
    // background: darken($sidebar-bg-from, 7%);
  }
}

// ---- main sidebar wrapper ----
.sidebar {
  position: fixed;
  inset-block: 0;
  inset-inline-start: 0;
  width: $sidebar-width;
  background-image: url(https://colorlib.com/etc/bootstrap-sidebar/sidebar-06/images/bg_1.jpg);
  // background-color: linear-gradient(180deg, $sidebar-bg-from 0%, $sidebar-bg-to 100%);
  color: #fff;
  display: grid;
  grid-template-rows: auto 1fr auto auto;
  transform: translateX(-100%);
  transition: transform $transition;
  z-index: $z-sidebar;
  box-shadow: 4px 0 16px rgba(0, 0, 0, 0.15);

  &--open {
    transform: translateX(0);
  }
}

.sidebar:after {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  content: '';
  background: $sidebar-bg-from;
  background: -moz-linear-gradient(45deg, $sidebar-bg-from 0%, $sidebar-bg-to 100%);
  background: -webkit-gradient(left bottom, right top, color-stop(0%, $sidebar-bg-from), color-stop(100%, $sidebar-bg-to));
  background: -webkit-linear-gradient(45deg, $sidebar-bg-from 0%, $sidebar-bg-to 100%);
  background: -o-linear-gradient(45deg, $sidebar-bg-from 0%, $sidebar-bg-to 100%);
  background: -ms-linear-gradient(45deg, $sidebar-bg-from 0%, $sidebar-bg-to 100%);
  // background: linear-gradient(45deg, $sidebar-bg-from 0%, $sidebar-bg-to 100%);
  background: linear-gradient(255deg, #96beff 40%, rgb(255 255 255 / 43%) 60%, #ffe29f 100%);
  // filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#2f89fc', endColorstr='#ff5db1', GradientType=1);
  opacity: .8;
  z-index: -1;
}

// ---- brand ----
.sidebar__brand {
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  column-gap: 0.5rem;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.15);
}

.sidebar__logo {
  width: 36px;
  height: 36px;
}

.sidebar__title {
  font-size: 1.25rem;
  margin: 0;
  font-weight: 700;
}

.sidebar__close {
  background: none;
  border: none;
  color: #fff;
  font-size: 1.5rem;
  line-height: 1;
  cursor: pointer;
}

// ---- nav ----
.sidebar__nav {
  // display: grid;
  padding: 1rem 0;
  gap: 0.25rem;
}

.sidebar__link {
  display: grid;
  grid-template-columns: 24px 1fr;
  align-items: center;
  column-gap: 0.75rem;
  padding: 0.625rem 1.25rem;
  color: #fff;
  text-decoration: none;
  font-size: 0.95rem;
  transition: background $transition;

  &:hover {
    background: rgba(255, 255, 255, 0.1);
  }

  &--active {
    background: rgba(255, 255, 255, 0.2);
  }
}

.sidebar__icon {
  font-size: 1.15rem;
}

// ---- subscribe block ----
.sidebar__subscribe {
  padding: 1.25rem;
  display: grid;
  gap: 0.75rem;
  border-top: 1px solid rgba(255, 255, 255, 0.15);
}

.sidebar__subscribe-title {
  font-size: 0.9rem;
  font-weight: 600;
  margin: 0;
}

.sidebar__subscribe-form {
  display: grid;
  grid-template-columns: 1fr auto;
  column-gap: 0.5rem;
}

.sidebar__subscribe-input {
  padding: 0.5rem 0.75rem;
  border: none;
  border-radius: 4px;
  font-size: 0.85rem;
}

.sidebar__subscribe-btn {
  padding: 0 1rem;
  background: #fff;
  color: $sidebar-bg-from;
  border: none;
  border-radius: 4px;
  font-weight: 600;
  cursor: pointer;
  transition: background $transition;

  &:hover {
    background: rgba(255, 255, 255, 0.85);
  }
}

// ---- footer ----
.sidebar__footer {
  padding: 0.75rem 1.25rem;
  font-size: 0.8rem;
  text-align: center;
  border-top: 1px solid rgba(255, 255, 255, 0.15);
}

// ---- responsiveness ----
@media (min-width: 1024px) {
  .burger {
    display: none;
  }

  .sidebar {
    transform: translateX(0);
  }
}
</style>
