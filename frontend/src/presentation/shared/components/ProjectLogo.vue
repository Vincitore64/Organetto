<template>
  <div :class="['logo', sizeClass]">
    <img src="@/presentation/shared/assets/images/logo-min.png" alt="Organetto" class="logo-image" />
    <span class="logo-text">Organetto</span>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'

interface Props {
  size?: 'small' | 'middle' | 'middlePlus' | 'large' | 'huge'
  src?: string
}

const props = withDefaults(defineProps<Props>(), {
  size: 'small',
  src: '@/presentation/shared/assets/images/logo-min.png',
})

const sizeClass = computed(() => `logo--${props.size}`)
// const logoSrc = computed(() => props.src!)
</script>

<style lang="scss" scoped>
.logo {
  display: grid;
  grid-auto-flow: column;
  align-items: end;
  gap: 1rem;
  grid-template-columns: auto 1fr;
  font-family: Sofia Sans Extra Condensed;

  &-image {
    width: 32px;
    height: auto;
  }

  &-text {
    font-weight: 400;
    // color: var(--color-primary-500);
    color: var(--color-text);
    font-size: 1.25rem;
  }

  &--small {
    gap: 0.3rem;

    .logo-image {
      width: 36px;
    }

    .logo-text {
      font-size: 1.5rem;
    }
  }

  &--middle {
    gap: 0.5rem;

    .logo-image {
      width: 56px;
    }

    .logo-text {
      font-size: 2rem;
    }
  }

  &--middlePlus {
    gap: 0.5rem;

    .logo-image {
      width: 72px;
    }

    .logo-text {
      font-size: 2.8rem;
    }
  }

  &--large {
    gap: 0.5rem;

    .logo-image {
      width: 96px;
    }

    .logo-text {
      font-size: 3.8rem;
    }
  }

  &--huge {
    .logo-image {
      width: 164px;
    }

    .logo-text {
      font-size: 7rem;
    }
  }
}
</style>
