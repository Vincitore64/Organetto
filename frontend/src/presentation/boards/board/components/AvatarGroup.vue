<template>
  <section :class="[styles.group, styles[size]]">
    <div
      v-for="(user, index) in visibleUsers"
      :key="user.id"
      :class="styles.avatar"
      :style="{ zIndex: visibleUsers.length - index }"
      :title="user.name"
    >
      <a-avatar
        :src="user.avatar"
        :alt="user.name"
        :size="avatarSize"
      >
        {{ user.name.charAt(0).toUpperCase() }}
      </a-avatar>
    </div>
    
    <div
      v-if="remainingCount > 0"
      :class="[styles.avatar, styles.overflow]"
      :title="t('board.avatarGroup.moreUsers', { count: remainingCount })"
    >
      <a-avatar :size="avatarSize">
        +{{ remainingCount }}
      </a-avatar>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Avatar as AAvatar } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import styles from './AvatarGroup.module.scss'

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

const avatarSize = computed(() => {
  switch (props.size) {
    case 'sm': return 24
    case 'lg': return 40
    default: return 32
  }
})
</script>

<style module="styles" lang="scss">
@use './AvatarGroup.module.scss';
</style>