<template>
  <nav class="sidebar">
    <!-- <section class="brand">
      <img src="@/presentation/shared/assets/images/logo.png" alt="logo" />
    </section> -->
    <ProjectLogo class="brand" size="middlePlus" />
    <a-menu mode="inline" :selectedKeys="['boards']" :style="{ background: 'transparent', border: 'none' }">
      <a-menu-item key="boards">
        <main class="sidebar__item">
          <unordered-list-outlined /> {{ t('mainLayout.sidebar.boards') }}
        </main>
      </a-menu-item>
      <a-menu-item key="settings">
        <main class="sidebar__item">
          <setting-outlined /> {{ t('mainLayout.sidebar.settings') }}
        </main>
      </a-menu-item>
      <a-menu-item key="actions">
        <main class="sidebar__item">
          <profile-outlined /> {{ t('mainLayout.sidebar.actions') }}
        </main>
      </a-menu-item>
      <a-menu-item key="cards">
        <main class="sidebar__item">
          <unordered-list-outlined /> {{ t('mainLayout.sidebar.cards') }}
        </main>
      </a-menu-item>
    </a-menu>
  </nav>
</template>
<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { UnorderedListOutlined, SettingOutlined, ProfileOutlined } from '@ant-design/icons-vue'
import ProjectLogo from './ProjectLogo.vue'

const { t } = useI18n()
</script>
<style scoped lang="scss">
.sidebar {
  height: 100%;
  padding: 24px 0;
  background: var(--color-surface) !important;
  // border-right: 1px solid var(--color-stroke);
  color: var(--color-text);

  :deep(.sidebar__item) {
    display: grid;
    grid-auto-flow: column;
    gap: 12px;
    align-items: center;
    grid-template-columns: 18px 1fr;
    // font-family: Sofia Sans Extra Condensed;
    font-family: Jost;
    font-size: 18px;
    // letter-spacing: 0.5px;

    >span {
      padding-bottom: 2px;

      >svg {
        width: 18px;
        height: 18px;
      }
    }
  }
}

.brand {
  display: flex;
  align-items: center;
  padding: 0 16px;
  padding-bottom: 24px;
}

:deep(.ant-menu-item-selected) {
  background: rgb(255 255 255 / 69%) !important;
  color: var(--color-primary-500) !important;
  background: rgba(241, 241, 241, 0.932) !important;
}

:deep(.ant-menu-item:active) {
  background: rgba(209, 209, 209, 0.69) !important;
  // color: var(--color-primary-500) !important;
}
</style>