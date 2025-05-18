<template>
  <section class="kanban-preview">
    <div class="preview-container">
      <!-- Placeholder for Kanban board image or live component -->
      <!-- <img src="@/assets/kanban-screenshot.png" alt="Kanban board preview" /> -->
    </div>
  </section>
</template>

<script setup lang="ts">
// No additional logic
</script>

<style lang="scss" scoped>
.kanban-preview {
  display: grid;
  place-items: center;
  padding: 2rem;
  background: linear-gradient(135deg, #6857ff 0%, #a175ff 100%);

  .preview-container img {
    max-width: 100%;
    border-radius: 0.5rem;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  }
}
</style>