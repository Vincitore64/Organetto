<template>
  <div v-if="type === 'card'" :class="styles.cardGhost">
    <CardItem
      :card="item as Card"
      @click="() => {}"
    />
  </div>
  
  <div v-else-if="type === 'list'" :class="styles.listGhost">
    <header :class="styles.listHeader">
      <h3 :class="styles.listTitle">{{ (item as List).title }}</h3>
      <span :class="styles.cardCount">{{ (item as List).cards.length }}</span>
    </header>
    <main :class="styles.listPreview">
      <div
        v-for="card in previewCards"
        :key="card.id"
        :class="styles.cardPreview"
      >
        {{ card.title }}
      </div>
      <div
        v-if="remainingCardsCount > 0"
        :class="styles.moreCards"
      >
        {{ t('board.dragGhost.moreCards', { count: remainingCardsCount }) }}
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { Card, List } from '../../types/board'
import CardItem from './CardItem.vue'
import styles from './DragGhost.module.scss'

interface Props {
  type: 'card' | 'list'
  item: Card | List
}

const props = defineProps<Props>()
const { t } = useI18n()

const previewCards = computed(() => {
  if (props.type === 'list') {
    const list = props.item as List
    return list.cards.slice(0, 3)
  }
  return []
})

const remainingCardsCount = computed(() => {
  if (props.type === 'list') {
    const list = props.item as List
    return Math.max(0, list.cards.length - 3)
  }
  return 0
})
</script>

<style module="styles" lang="scss">
@import './DragGhost.module.scss';
</style>