<template>
  <main class="home-page">
    <!-- Left column: logo, headline, CTA -->
    <section class="intro">
      <img src="@/presentation/shared/assets/images/logo.png" alt="Organetto Logo" class="logo" />
      <h1 class="title">Start organizing with your free Kanban board.</h1>
      <a-button type="primary" size="large">Get Started</a-button>
    </section>

    <!-- Right column: navigation + board preview -->
    <section class="preview">
      <header class="preview-header">
        <nav class="nav">
          <a-button type="text" size="large" href="#">Home</a-button>
          <a-button type="text" size="large" href="#">Features</a-button>
          <a-button type="text" size="large" href="#">Pricing</a-button>
          <a-button type="text" size="large" href="#">Login</a-button>
          <a-button type="text" size="large" href="#">Sign Up</a-button>
          <!-- <a href="#">Home</a>
          <a href="#">Features</a>
          <a href="#">Pricing</a>
          <a href="#">Login</a>
          <a href="#">Sign Up</a> -->
        </nav>
      </header>
      <div class="screenshot">
        <KanbanPreview />
      </div>
    </section>
  </main>
</template>

<script setup lang="ts">
import { KanbanPreview } from '@/presentation'
</script>

<style lang="scss" scoped>
.home-page {
  display: grid;
  grid-template-columns: 1.2fr 1fr;
  height: 100vh;
}

/* Intro column */
.intro {
  padding: 0 2rem;
  display: grid;
  grid-template-columns: 1fr;
  grid-auto-rows: auto max-content 50px;
  height: 60%;

  >button {
    justify-self: start;
    width: 40%;
    font-size: 20px;
    height: 100%;
    margin-left: 3rem;
  }
}

.logo {
  width: 512px;
  height: auto;
}

.title {
  font-size: 3.8rem;
  font-weight: 600;
  line-height: 1.2;
  padding: 0 3rem;
  margin: 0;
  margin-bottom: 3rem;
}

/* Preview column */
.preview {
  display: grid;
  grid-template-rows: max-content 1fr;
  // background: linear-gradient(135deg, #6e41ff 0%, #b77aff 100%);
  background: linear-gradient(135deg, #9580ff, #fbe8c9);
  padding: 2rem;
  border-radius: 0 1rem 1rem 0;
}

.preview-header {
  border-radius: 0.5rem;
  padding-top: 1rem;
}

.nav {
  display: grid;
  grid-template-columns: repeat(5, auto);
  align-items: center;
  gap: 1rem;

  .ant-btn-text {
    :deep(>span) {
      font-weight: 500;
      font-size: 18px;
    }
  }

  .ant-btn-text:not(:disabled):hover {
    color: rgba(0, 0, 0, 0.88);
    /* background-color: rgba(0, 0, 0, 0.06); */
    background-color: rgba(255, 255, 255, 0.8);
  }

  .ant-btn-text:not(:disabled):active {
    color: rgba(0, 0, 0, 0.88);
    background-color: rgba(255, 255, 255, 1);
  }
}

.nav a {
  color: #000;
  text-decoration: none;
  font-weight: 500;
}

/* Screenshot area */
.screenshot {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 75%;
}

.screenshot img {
  max-width: 100%;
  border-radius: 0.5rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}
</style>
<!-- <template>
  <main class="home-page">
    <HeaderNav />
    <div class="content-grid">
      <HeroSection />
      <KanbanPreview />
    </div>
  </main>
</template>

<script setup lang="ts">
import { HeaderNav, HeroSection, KanbanPreview } from '@/presentation'
</script>

<style lang="scss" scoped>
.home-page {
  display: grid;
  grid-template-rows: auto 1fr;
  height: 100vh;

  .content-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    overflow: hidden;
  }
}
</style> -->