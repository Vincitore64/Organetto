<template>
  <main class="login-page">
    <!-- Brand / marketing side -->
    <section class="splash">
      <!-- <OrganettoLogo class="logo" /> -->
      <!-- <h1 class="brand">Organetto</h1> -->
      <ProjectLogo size="huge" class="brand" />
      <p class="tagline">{{ t('login.page.tagline') }}</p>
    </section>

    <!-- Form side (plugs reusable component) -->
    <section class="panel">
      <LoginForm @success="onSuccess" />
    </section>
  </main>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import LoginForm from '@/presentation/login/components/LoginForm.vue'
import { useUsersComposable } from '@/application/users/hooks/useUsers'
import { ProvidedService, tryInjectServices } from '@/shared'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import type { UsersStore } from '@/application/users/stores/usersStore'
import ProjectLogo from '@/presentation/shared/components/ProjectLogo.vue'

const router = useRouter()
const { t } = useI18n()
const apiClient = tryInjectServices().resolve(ApiClient)
const usersStore = tryInjectServices().resolve<UsersStore>(ProvidedService.UsersStore)
const usersComposable = useUsersComposable(apiClient, usersStore)

async function onSuccess() {
  debugger
  const u = await usersComposable.getCurrentUser()
  router.push({ name: 'Boards', params: { userId: u.id } })
}
</script>

<style scoped lang="scss">
// @use '@/styles/mixins' as *;

.login-page {
  display: grid;
  grid-template-columns: 1fr 1fr;
  min-height: 100vh;
  // background: var(--gradient-hero, linear-gradient(120deg, #d8ecff 0%, #f5faff 40%, #fef6e9 100%));
  background: var(--gradient-hero, linear-gradient(120deg, #d8ecff 0%, #f5faff 40%, #fbe6c5 100%));

  @media (max-width: 960px) {
    grid-template-columns: 1fr;

    .splash {
      display: none;
    }
  }
}

.splash {
  padding-left: 20%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 1rem;
  color: var(--color-text);
  font-family: 'Sofia Sans Extra Condensed';
  letter-spacing: 1px;

  .logo {
    // width: 164px;
    height: 164px;
  }

  .brand {
    font-size: 7rem;
    margin: 0;
    color: var(--color-text);
  }

  .tagline {
    font-size: 2.5rem;
    color: var(--color-text-weak);
    padding-left: 192px;
  }
}

.panel {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;

  >.card {
    width: 390px;
    background: linear-gradient(135deg, #ffffff, #fff7eb);
  }
}
</style>