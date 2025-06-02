<template>
  <main class="login-page">
    <!-- Brand / marketing side -->
    <section class="splash">
      <!-- <OrganettoLogo class="logo" /> -->
      <h1 class="brand">Organetto</h1>
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

const router = useRouter()
const { t } = useI18n()

function onSuccess() {
  router.push('/boards')
}
</script>

<style scoped lang="scss">
// @use '@/styles/mixins' as *;

.login-page {
  display: grid;
  grid-template-columns: 1fr 1fr;
  min-height: 100vh;
  background: var(--gradient-hero, linear-gradient(120deg, #d8ecff 0%, #f5faff 40%, #fef6e9 100%));

  @media (max-width: 960px) {
    grid-template-columns: 1fr;

    .splash {
      display: none;
    }
  }
}

.splash {
  padding-left: clamp(2rem, 6vw, 6rem);
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 1.5rem;
  color: var(--color-text);

  .logo {
    width: 72px;
    height: 72px;
  }

  .brand {
    font-size: 2.5rem;
    font-weight: 700;
    margin: 0;
    color: var(--color-text);
  }

  .tagline {
    font-size: 1.25rem;
    color: var(--color-text-weak);
    max-width: 20ch;
  }
}

.panel {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;

  >.card {
    width: 390px;
  }
}
</style>