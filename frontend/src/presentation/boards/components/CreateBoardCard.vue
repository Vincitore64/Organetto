<template>
  <a-card hoverable class="create-card"
    :body-style="{ height: 'max-content', display: 'flex', flexDirection: 'column', justifyContent: 'center', padding: '24px' }"
    @click="$emit('create')">
    <template #cover>
      <div class="create-cover">
        <div class="gradient-bg">
          <div class="floating-icons">
            <div class="floating-icon" v-for="n in 6" :key="n" :style="getFloatingIconStyle(n)">
              <component :is="getRandomIcon()" />
            </div>
          </div>
        </div>
      </div>
    </template>

    <div class="create-body">
      <div class="main-icon">
        <plus-outlined class="plus-icon" />
      </div>
      <h3 class="create-title">{{ t('boards.page.create') }}</h3>
      <p class="create-description">Создайте новую доску для вашего проекта</p>
      <div class="create-features">
        <div class="feature">
          <CheckOutlined class="feature-icon" />
          <span>Шаблоны</span>
        </div>
        <div class="feature">
          <CheckOutlined class="feature-icon" />
          <span>Совместная работа</span>
        </div>
      </div>
    </div>
  </a-card>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import {
  PlusOutlined,
  CheckOutlined,
  BulbOutlined,
  RocketOutlined,
  StarOutlined,
  HeartOutlined,
  ThunderboltOutlined,
  CrownOutlined
} from '@ant-design/icons-vue'

const { t } = useI18n()

defineEmits<{
  create: []
}>()

const icons = [
  BulbOutlined,
  RocketOutlined,
  StarOutlined,
  HeartOutlined,
  ThunderboltOutlined,
  CrownOutlined
]

const getRandomIcon = () => {
  return icons[Math.floor(Math.random() * icons.length)]
}

const getFloatingIconStyle = (index: number) => {
  const positions = [
    { top: '10%', left: '15%', animationDelay: '0s' },
    { top: '20%', right: '20%', animationDelay: '0.5s' },
    { top: '40%', left: '10%', animationDelay: '1s' },
    { top: '60%', right: '15%', animationDelay: '1.5s' },
    { top: '75%', left: '25%', animationDelay: '2s' },
    { top: '30%', right: '35%', animationDelay: '2.5s' }
  ]
  return positions[index - 1] || {}
}
</script>

<style scoped lang="scss">
.create-card {
  border-radius: 12px;
  border: 2px dashed var(--color-primary-300, #91d5ff);
  background: var(--color-surface, #ffffff);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  cursor: pointer;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  min-height: 320px;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
    border-color: var(--color-primary-500, #1890ff);
    background: linear-gradient(135deg, #ffffff, #f6ffed);

    .main-icon {
      transform: scale(1.1) rotate(90deg);
    }

    .floating-icon {
      animation-play-state: running;
    }

    .gradient-bg {
      background: linear-gradient(135deg,
          rgba(24, 144, 255, 0.1),
          rgba(82, 196, 26, 0.1),
          rgba(250, 173, 20, 0.1));
    }
  }

  :deep(.ant-card-body) {
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    padding: 24px;
  }
}

.create-cover {
  height: 120px;
  position: relative;
  overflow: hidden;

  .gradient-bg {
    width: 100%;
    height: 100%;
    background: linear-gradient(135deg,
        rgba(240, 242, 245, 0.8),
        rgba(230, 247, 255, 0.8));
    transition: background 0.3s ease;
    position: relative;
  }

  .floating-icons {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;

    .floating-icon {
      position: absolute;
      font-size: 16px;
      color: var(--color-primary-400, #69c0ff);
      opacity: 0.6;
      animation: float 3s ease-in-out infinite;
      animation-play-state: paused;

      @keyframes float {

        0%,
        100% {
          transform: translateY(0px) rotate(0deg);
        }

        50% {
          transform: translateY(-10px) rotate(180deg);
        }
      }
    }
  }
}

.create-body {
  text-align: center;

  .main-icon {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    width: 64px;
    height: 64px;
    border-radius: 50%;
    background: linear-gradient(135deg, var(--color-primary-500, #1890ff), var(--color-primary-600, #096dd9));
    margin-bottom: 16px;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    box-shadow: 0 4px 16px rgba(24, 144, 255, 0.3);

    .plus-icon {
      font-size: 28px;
      color: white;
    }
  }

  .create-title {
    font-size: 18px;
    font-weight: 600;
    color: var(--color-text, #262626);
    margin: 0 0 8px 0;
    line-height: 1.4;
  }

  .create-description {
    font-size: 14px;
    color: var(--color-text-weak, #8c8c8c);
    margin: 0 0 16px 0;
    line-height: 1.5;
  }

  .create-features {
    display: flex;
    justify-content: center;
    gap: 16px;

    .feature {
      display: flex;
      align-items: center;
      gap: 4px;
      font-size: 12px;
      color: var(--color-success, #52c41a);

      .feature-icon {
        font-size: 12px;
      }
    }
  }
}
</style>