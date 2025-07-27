<template>
  <a-card 
    hoverable 
    class="feature-card" 
    :class="{ 'clickable': clickable }"
    :body-style="{
      height: 'max-content',
      display: 'flex',
      flexDirection: 'column',
      justifyContent: 'center',
      padding: '16px 12px 0px 12px'
    }" 
    @click="handleClick"
  >
    <template #cover v-if="showCover">
      <div class="feature-cover">
        <div class="gradient-bg" :class="coverVariant">
          <slot name="cover">
            <div class="floating-icons" v-if="showFloatingIcons">
              <div 
                class="floating-icon" 
                v-for="n in 6" 
                :key="n" 
                :style="getFloatingIconStyle(n)"
              >
                <component :is="getRandomIcon()" />
              </div>
            </div>
          </slot>
        </div>
      </div>
    </template>

    <div class="feature-body">
      <!-- Icon slot -->
      <div class="main-icon" v-if="$slots.icon || icon">
        <slot name="icon">
          <component :is="icon" class="icon" v-if="icon" />
        </slot>
      </div>
      
      <!-- Title -->
      <h3 class="feature-title" v-if="title || $slots.title">
        <slot name="title">{{ title }}</slot>
      </h3>
      
      <!-- Description -->
      <p class="feature-description" v-if="description || $slots.description">
        <slot name="description">{{ description }}</slot>
      </p>
      
      <!-- Footer slot -->
      <div class="feature-footer" v-if="$slots.footer">
        <slot name="footer" />
      </div>
    </div>
  </a-card>
</template>

<script setup lang="ts">
import {
  BulbOutlined,
  RocketOutlined,
  StarOutlined,
  HeartOutlined,
  ThunderboltOutlined,
  CrownOutlined
} from '@ant-design/icons-vue'
import type { Component } from 'vue'

interface Props {
  title?: string
  description?: string
  icon?: Component
  clickable?: boolean
  showCover?: boolean
  showFloatingIcons?: boolean
  coverVariant?: 'default' | 'primary' | 'success' | 'warning' | 'danger'
}

const props = withDefaults(defineProps<Props>(), {
  clickable: false,
  showCover: false,
  showFloatingIcons: false,
  coverVariant: 'default'
})

const emit = defineEmits<{
  click: []
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

const handleClick = () => {
  if (props.clickable) {
    emit('click')
  }
}
</script>

<style scoped lang="scss">
.feature-card {
  border-radius: 12px;
  border: 2px dashed var(--color-primary-300, #91d5ff);
  background: var(--color-surface, #ffffff);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  min-height: 200px;

  &.clickable {
    cursor: pointer;
    
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
        &.default {
          background: linear-gradient(135deg,
              rgba(24, 144, 255, 0.1),
              rgba(82, 196, 26, 0.1),
              rgba(250, 173, 20, 0.1));
        }
        
        &.primary {
          background: linear-gradient(135deg,
              rgba(24, 144, 255, 0.15),
              rgba(64, 169, 255, 0.15));
        }
        
        &.success {
          background: linear-gradient(135deg,
              rgba(82, 196, 26, 0.15),
              rgba(115, 209, 61, 0.15));
        }
        
        &.warning {
          background: linear-gradient(135deg,
              rgba(250, 173, 20, 0.15),
              rgba(255, 197, 61, 0.15));
        }
        
        &.danger {
          background: linear-gradient(135deg,
              rgba(255, 77, 79, 0.15),
              rgba(255, 120, 117, 0.15));
        }
      }
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

.feature-cover {
  height: 120px;
  position: relative;
  overflow: hidden;

  .gradient-bg {
    width: 100%;
    height: 100%;
    transition: background 0.3s ease;
    position: relative;
    
    &.default {
      background: linear-gradient(135deg,
          rgba(240, 242, 245, 0.8),
          rgba(230, 247, 255, 0.8));
    }
    
    &.primary {
      background: linear-gradient(135deg,
          rgba(24, 144, 255, 0.1),
          rgba(64, 169, 255, 0.1));
    }
    
    &.success {
      background: linear-gradient(135deg,
          rgba(82, 196, 26, 0.1),
          rgba(115, 209, 61, 0.1));
    }
    
    &.warning {
      background: linear-gradient(135deg,
          rgba(250, 173, 20, 0.1),
          rgba(255, 197, 61, 0.1));
    }
    
    &.danger {
      background: linear-gradient(135deg,
          rgba(255, 77, 79, 0.1),
          rgba(255, 120, 117, 0.1));
    }
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
        0%, 100% {
          transform: translateY(0px) rotate(0deg);
        }
        50% {
          transform: translateY(-10px) rotate(180deg);
        }
      }
    }
  }
}

.feature-body {
  text-align: center;
  // flex: 1;
  // display: flex;
  // flex-direction: column;
  // justify-content: center;

  .main-icon {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    width: 64px;
    height: 64px;
    border-radius: 50%;
    background: var(--color-blue-gradient, linear-gradient(135deg, #1890ff, #096dd9));
    margin: 0 auto 16px;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    box-shadow: 0 4px 16px rgba(24, 144, 255, 0.3);

    .icon {
      font-size: 28px;
      color: white;
    }
  }

  .feature-title {
    font-size: 19px;
    font-weight: 600;
    color: var(--color-text, #262626);
    margin: 0 0 8px 0;
    line-height: 1.5;
  }

  .feature-description {
    font-size: 16px;
    color: var(--color-text-weak, #8c8c8c);
    margin: 0 0 12px 0;
    line-height: 1;
    // flex: 1;
  }

  .feature-footer {
    margin-top: auto;
  }
}

// Responsive adjustments
@media (max-width: 768px) {
  .feature-card {
    min-height: 180px;
    
    :deep(.ant-card-body) {
      padding: 16px;
    }
  }
  
  .feature-body {
    .main-icon {
      width: 48px;
      height: 48px;
      margin-bottom: 12px;
      
      .icon {
        font-size: 20px;
      }
    }
    
    .feature-title {
      font-size: 16px;
    }
    
    .feature-description {
      font-size: 14px;
    }
  }
  
  .feature-cover {
    height: 80px;
  }
}
</style>