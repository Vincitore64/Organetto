<template>
  <header class="header-nav">
    <nav>
      <ul>
        <li><router-link to="/">Home</router-link></li>
        <li><router-link to="/features">Features</router-link></li>
        <li><router-link to="/pricing">Pricing</router-link></li>
        <li><router-link to="/login">Login</router-link></li>
        <li><router-link to="/register"><a-button type="primary">Sign Up</a-button></router-link></li>
      </ul>
    </nav>
  </header>
</template>

<script setup lang="ts">
// No additional logic needed
</script>

<style lang="scss" scoped>
.header-nav {
  padding: 1rem;

  ul {
    display: flex;
    gap: 1.5rem;
    list-style: none;
    margin: 0;
    padding: 0;
    align-items: center;
  }
}
</style>