<template>
  <main class="registration-page__wrapper">
    <SidebarBanner />
    <section class="registration-page__form-wrapper">
      <RegistrationForm class="registration-page__form" @success="onSuccess" />
    </section>
  </main>
</template>

<script setup lang="ts">
import { SidebarBanner, RegistrationForm } from '@/presentation'
import { useRouter } from 'vue-router'

const router = useRouter()

function onSuccess() {
  router.push({ name: 'Login' })
}

</script>

<style scoped>
.registration-page__wrapper {
  display: grid;
  grid-template-columns: 1fr 1fr;
  min-height: 100vh;
  background: linear-gradient(120deg, #d8ecff 0%, #f5faff 40%, #fbe6c5 100%);
}

.registration-page__form-wrapper {
  display: grid;
  align-items: center;
  justify-items: center;
  /* background: linear-gradient(135deg, #4b33c6, #fbe8c9); */
}

.registration-page__form {
  width: 390px;
  /* background: linear-gradient(135deg, #ffffff, #f5e5cb); */
  background: linear-gradient(135deg, #ffffff, #fff7eb);
}

@media (max-width: 768px) {
  .registration-page__wrapper {
    grid-template-columns: 1fr;
  }
}
</style>