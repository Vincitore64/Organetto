<template>
  <main class="sidebar">
    <img src="@/presentation/shared/assets/images/logo.png" alt="Organetto logo" class="logo" />
    <h1>Start organizing<br />with your free Kanban board.</h1>
    <p>No credit card required.</p>
  </main>
</template>

<script setup lang="ts">
// no script needed yet
</script>

<style scoped>
.sidebar {
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 2rem;
  /* color: white; */
  gap: 1.5rem;
}

.logo {
  width: 750px;
}

.sidebar h1 {
  font-size: 2rem;
  line-height: 1.2;
  margin: 0;
  padding-left: 15%;
}

.sidebar p {
  font-size: 1rem;
  opacity: 0.9;
  padding-left: 15%;
}
</style>
