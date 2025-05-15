<template>
  <main class="registration-page__wrapper">
    <SidebarBanner />
    <section class="registration-page__form-wrapper">
      <RegistrationForm class="registration-page__form" />
    </section>
  </main>
</template>

<script setup lang="ts">
import SidebarBanner from '@/presentation/registration/components/SidebarBanner.vue'
import RegistrationForm from '@/presentation/registration/components/RegistrationForm.vue'
</script>

<style scoped>
.registration-page__wrapper {
  display: grid;
  grid-template-columns: 1fr 1fr;
  min-height: 100vh;
}

.registration-page__form-wrapper {
  display: grid;
  align-items: center;
  justify-items: center;
  background: linear-gradient(135deg, #4b33c6, #fbe8c9);
}

@media (max-width: 768px) {
  .registration-page__wrapper {
    grid-template-columns: 1fr;
  }
}
</style>