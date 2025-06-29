<template>
  <a-card hoverable class="board-card" :body-style="{ padding: '16px' }" @click="$emit('open', board.id)">
    <template #cover>
      <div class="card-cover">
        <img :src="board.thumbnailUrl" :alt="board.title" class="board-thumbnail" />
        <div class="overlay">
          <div class="overlay-content">
            <EyeOutlined class="view-icon" />
            <span class="view-text">Открыть доску</span>
          </div>
        </div>
      </div>
    </template>

    <div class="card-content">
      <h3 class="board-title">{{ board.title }}</h3>
      <div class="board-meta">
        <div class="meta-item">
          <ClockCircleOutlined class="meta-icon" />
          <span class="meta-text">Недавно</span>
        </div>
        <div class="meta-item">
          <UserOutlined class="meta-icon" />
          <span class="meta-text">Личная</span>
        </div>
      </div>
    </div>
  </a-card>
</template>

<script setup lang="ts">
import { EyeOutlined, ClockCircleOutlined, UserOutlined } from '@ant-design/icons-vue'

interface Board {
  id: number
  title: string
  thumbnailUrl: string
}

defineProps<{ board: Board }>()
defineEmits<{
  open: [id: number]
}>()
</script>

<style scoped lang="scss">
.board-card {
  display: grid;
  grid-template-rows: 1fr auto;
  border-radius: 12px;
  border: 1px solid rgba(0, 0, 0, 0.06);
  background: var(--color-surface, #ffffff);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  cursor: pointer;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
    border-color: var(--color-primary-200, #bae7ff);

    .overlay {
      opacity: 1;
    }

    .board-thumbnail {
      transform: scale(1.05);
    }
  }

  :deep(.ant-card-body) {
    padding: 16px;
  }
}

.card-cover {
  position: relative;
  height: 100%;
  overflow: hidden;
  background: linear-gradient(135deg, #f0f2f5, #e6f7ff);

  .board-thumbnail {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  }

  .overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.6);
    display: flex;
    align-items: center;
    justify-content: center;
    opacity: 0;
    transition: opacity 0.3s cubic-bezier(0.4, 0, 0.2, 1);

    .overlay-content {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;
      color: white;

      .view-icon {
        font-size: 24px;
      }

      .view-text {
        font-size: 16px;
        font-weight: 500;
      }
    }
  }
}

.card-content {
  .board-title {
    font-size: 18px;
    font-weight: 600;
    color: var(--color-text, #262626);
    margin: 0 0 12px 0;
    line-height: 1.4;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
  }

  .board-meta {
    display: flex;
    gap: 16px;

    .meta-item {
      display: flex;
      align-items: center;
      gap: 4px;

      .meta-icon {
        font-size: 14px;
        color: var(--color-text-weak, #8c8c8c);
      }

      .meta-text {
        font-size: 14px;
        color: var(--color-text-weak, #8c8c8c);
        font-weight: 400;
      }
    }
  }
}
</style>