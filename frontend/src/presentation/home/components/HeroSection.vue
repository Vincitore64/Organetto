<template>
  <section class="hero-section">
    <div class="hero-content">
      <img src="@/presentation/shared/assets/images/logo.png" alt="Organetto logo" class="logo" />
      <h1>Start organizing with your free Kanban board.</h1>
      <router-link to="/register">
        <a-button type="primary" size="large">Get Started</a-button>
      </router-link>
    </div>
  </section>
</template>

<script setup lang="ts">
// No additional logic
</script>

<style lang="scss" scoped>
.hero-section {
  display: grid;
  place-items: center;
  padding: 2rem;

  .hero-content {
    max-width: 400px;
    text-align: left;

    .logo {
      width: 48px;
      margin-bottom: 1rem;
    }

    h1 {
      font-size: 2rem;
      margin-bottom: 1.5rem;
    }
  }
}
</style>