<template>
  <div class="avatar-group">
    <a-avatar
      v-for="(user, index) in visibleUsers"
      :key="user.id"
      :size="avatarSize"
      :src="user.avatar"
      :class="['avatar', { 'stacked': isStacked }]"
      :style="{ zIndex: visibleUsers.length - index }"
    >
      {{ user.name.charAt(0).toUpperCase() }}
    </a-avatar>
    
    <a-tooltip v-if="remainingCount > 0" :title="moreUsersTooltip">
      <a-avatar
        :size="avatarSize"
        :class="['avatar', 'more-avatar', { 'stacked': isStacked }]"
      >
        +{{ remainingCount }}
      </a-avatar>
    </a-tooltip>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Avatar as AAvatar } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'


interface User {
  id: string
  name: string
  avatar: string
}

interface Props {
  users: User[]
  maxVisible?: number
  size?: 'sm' | 'md' | 'lg'
}

const props = withDefaults(defineProps<Props>(), {
  maxVisible: 3,
  size: 'md'
})

const { t } = useI18n()

const visibleUsers = computed(() => 
  props.users.slice(0, props.maxVisible)
)

const remainingCount = computed(() => 
  Math.max(0, props.users.length - props.maxVisible)
)

const hiddenUsers = computed(() => {
  return props.users.slice(props.maxVisible)
})

const moreUsersTooltip = computed(() => {
  const names = hiddenUsers.value.map(user => user.name).join(', ')
  return `${t('board.avatarGroup.moreUsers')}: ${names}`
})

const isStacked = computed(() => props.users.length > 1)

const avatarSize = computed(() => {
  switch (props.size) {
    case 'sm': return 24
    case 'lg': return 40
    default: return 32
  }
})
</script>

<style scoped lang="scss">
.avatar-group {
  display: flex;
  align-items: center;
  gap: 4px;
  
  .avatar {
    border: 2px solid rgba(255, 255, 255, 0.9);
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(10px);
    transition: var(--transition-smooth);
    cursor: pointer;
    
    &:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      border-color: var(--color-primary-400);
    }
    
    &.stacked {
      margin-left: -8px;
      
      &:first-child {
        margin-left: 0;
      }
    }
    
    &.more-avatar {
      background: linear-gradient(135deg, var(--color-primary-500), var(--color-primary-600));
      color: white;
      font-weight: 600;
      font-size: 12px;
      
      &:hover {
        background: linear-gradient(135deg, var(--color-primary-600), var(--color-primary-700));
        transform: translateY(-2px) scale(1.05);
      }
    }
  }
  
  :deep(.ant-avatar) {
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 600;
    font-size: 14px;
    color: var(--color-text);
    
    img {
      object-fit: cover;
    }
  }
}

@media (max-width: 768px) {
  .avatar-group {
    gap: 2px;
    
    .avatar {
      &.stacked {
        margin-left: -6px;
      }
      
      &:hover {
        transform: translateY(-1px);
      }
    }
  }
}
</style>