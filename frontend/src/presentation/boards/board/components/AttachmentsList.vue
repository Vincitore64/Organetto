<script setup lang="ts">
/**
 * AttachmentsList.vue
 * - Semantic HTML + CSS Grid
 * - SCSS with BEM naming
 * - Keyboard navigation & selection
 * - Inline SVG icon set with override
 */
import { ext, resolveIconCategory, formatDate, formatBytes } from '@/shared'
import { computed, onMounted, onUnmounted, ref } from 'vue'
import { tryI18n } from '@/shared'
import {
  IconDownload,
  IconTrash,
  fileIconMapper,
} from '@/presentation/shared'
import _ from 'lodash'


type AttachmentVm = {
  id: number
  filename: string
  fileUrl: string
  uploadedAt: Date
  uploaderId: number
  sizeBytes?: number
  mimeType?: string
  thumbnailUrl?: string
}

type Layout = 'list' | 'grid'

const props = withDefaults(defineProps<{
  items: AttachmentVm[]
  layout?: Layout
  showSize?: boolean
  showActions?: boolean
  iconSet?: 'custom'
  iconMap?: Record<string, any>
  truncate?: number
  loading?: boolean
  error?: string | null
  emptyState?: string
  selectable?: boolean
  disabled?: boolean
  ariaLabel?: string
  download?: boolean
  openInNewTab?: boolean
  virtualized?: boolean
}>(), {
  items: () => [],
  layout: 'list',
  showSize: false,
  showActions: true,
  iconSet: 'custom',
  iconMap: () => ({}),
  truncate: 28,
  loading: false,
  error: null,
  emptyState: 'No attachments yet',
  selectable: false,
  disabled: false,
  ariaLabel: 'Attachments',
  download: true,
  openInNewTab: true,
  virtualized: false
})

const emit = defineEmits<{
  (e: 'item-click', item: AttachmentVm): void
  (e: 'download', item: AttachmentVm): void
  (e: 'remove', item: AttachmentVm): void
  (e: 'selection-change', selectedIds: number[]): void
  (e: 'retry'): void
}>()

const { t } = tryI18n()

// ---------- Helpers ----------
const safeUrl = (u: string) => {
  try {
    const parsed = new URL(u, window.location.origin)
    return parsed.toString()
  } catch { return '#' }
}
const truncated = (text: string) => {
  return _.truncate(text, {
    length: !!props.truncate ? props.truncate : undefined,
  })
}

function resolvedIconComponent(item: AttachmentVm) {
  const category = resolveIconCategory(item.mimeType, item.filename)

  // Precise ext override first (key: "ext:csv")
  const extKey = `ext:${ext(item.filename)}`
  return props.iconMap[extKey] || props.iconMap[category] || fileIconMapper(category)
}
function emitClick(item: AttachmentVm) { emit('item-click', item) }

// ---------- Selection & keyboard ----------
const focusedIndex = ref(-1)
const selectedIdsSet = ref<Set<number>>(new Set())

function onItemClick(item: AttachmentVm, index: number, e: MouseEvent) {
  if (props.disabled) return
  focusedIndex.value = index
  if (props.selectable) {
    const set = new Set(selectedIdsSet.value)
    if (e.metaKey || e.ctrlKey) {
      set.has(item.id) ? set.delete(item.id) : set.add(item.id)
    } else if (e.shiftKey) {
      const [a, b] = [Math.min(focusedIndex.value, index), Math.max(focusedIndex.value, index)]
      const range = effectiveItems.value.slice(a, b + 1).map(x => x.id)
      range.forEach(id => set.add(id))
    } else {
      set.clear()
      set.add(item.id)
    }
    selectedIdsSet.value = set
    emit('selection-change', Array.from(set))
  } else {
    emitClick(item)
  }
}

function tabIndexFor(i: number) {
  return i === focusedIndex.value ? 0 : (focusedIndex.value === -1 && i === 0 ? 0 : -1)
}

function onKeydown(e: KeyboardEvent) {
  if (props.disabled || effectiveItems.value.length === 0) return
  const max = effectiveItems.value.length - 1
  if (e.key === 'Escape' && props.selectable) {
    selectedIdsSet.value = new Set()
    emit('selection-change', [])
    e.preventDefault()
    return
  }
  const step = props.layout === 'grid' ? gridColumns.value : 1
  if (['ArrowDown','ArrowRight'].includes(e.key)) {
    focusedIndex.value = Math.min(max, Math.max(0, (focusedIndex.value < 0 ? 0 : focusedIndex.value + step)))
    ensureFocusVisible()
    e.preventDefault()
  } else if (['ArrowUp','ArrowLeft'].includes(e.key)) {
    focusedIndex.value = Math.max(0, (focusedIndex.value < 0 ? 0 : focusedIndex.value - step))
    ensureFocusVisible()
    e.preventDefault()
  } else if (e.key === 'Enter' || e.key === ' ') {
    const item = effectiveItems.value[focusedIndex.value]
    if (item) emitClick(item)
    e.preventDefault()
  }
}

// ---------- Virtualization (list only) ----------
const scrollEl = ref<HTMLElement | null>(null)
const ROW_H = 56 // px assumed row height for list
const buffer = 6
const renderIndexStart = ref(0)
const renderCount = ref(30)

const effectiveItems = computed(() => props.items ?? [])
const totalHeight = computed(() => effectiveItems.value.length * ROW_H)
const topSpacerStyle = computed(() => ({ height: `${renderIndexStart.value * ROW_H}px` }))
const bottomSpacerStyle = computed(() => {
  const end = renderIndexStart.value + renderCount.value
  const rem = Math.max(0, effectiveItems.value.length - end)
  return { height: `${rem * ROW_H}px` }
})
const visibleItems = computed(() => {
  if (!props.virtualized || props.layout !== 'list') return effectiveItems.value
  return effectiveItems.value.slice(renderIndexStart.value, renderIndexStart.value + renderCount.value)
})

function onScroll() {
  if (!scrollEl.value || !props.virtualized || props.layout !== 'list') return
  const st = scrollEl.value.scrollTop
  const height = scrollEl.value.clientHeight
  const start = Math.max(0, Math.floor(st / ROW_H) - buffer)
  const count = Math.ceil(height / ROW_H) + buffer * 2
  renderIndexStart.value = start
  renderCount.value = count
}
function ensureFocusVisible() {
  if (!scrollEl.value || !props.virtualized || props.layout !== 'list') return
  const y = focusedIndex.value * ROW_H
  const top = scrollEl.value.scrollTop
  const bottom = top + scrollEl.value.clientHeight
  if (y < top) scrollEl.value.scrollTo({ top: y })
  else if (y + ROW_H > bottom) scrollEl.value.scrollTo({ top: y - scrollEl.value.clientHeight + ROW_H })
}

onMounted(() => {
  if (scrollEl.value) scrollEl.value.addEventListener('scroll', onScroll, { passive: true })
})
onUnmounted(() => {
  if (scrollEl.value) scrollEl.value.removeEventListener('scroll', onScroll as any)
})

// ---------- Grid columns (for keyboard step) ----------
const gridColumns = computed(() => {
  // Keep in sync with CSS grid auto-fit min width
  return props.layout === 'grid' ?  Math.max(1, Math.floor((scrollEl.value?.clientWidth ?? 0) / 240)) : 1
})


</script>
<template>
  <section
    class="attachments"
    :class="[
      `attachments--${layout}`,
      { 'attachments--disabled': disabled, 'attachments--loading': loading }
    ]"
    :aria-label="ariaLabel"
    :role="layout === 'grid' ? 'grid' : 'list'"
  >
    <!-- Error -->
    <div v-if="error && !loading" class="attachments__error" role="alert">
      <slot name="error">
        <span class="attachments__error-text">{{ error }}</span>
        <button
          class="attachments__error-retry"
          type="button"
          @click="emit('retry')"
        >
          {{ t('common.retry', 'Retry') }}
        </button>
      </slot>
    </div>

    <!-- Loading skeletons -->
    <div v-if="loading" class="attachments__skeletons">
      <div
        v-for="n in 6"
        :key="n"
        class="attachments__skeleton"
        :class="`attachments__skeleton--${layout}`"
        aria-hidden="true"
      />
    </div>

    <!-- Empty -->
    <div v-else-if="!error && effectiveItems.length === 0" class="attachments__empty">
      <slot name="empty">
        {{ emptyState }}
      </slot>
    </div>

    <!-- List/Grid -->
    <div
      v-else
      class="attachments__container"
      ref="scrollEl"
      @keydown="onKeydown"
    >
      <!-- Virtualized spacer (list mode only) -->
      <div v-if="virtualized && layout === 'list'" :style="topSpacerStyle" />

      <div
        v-for="(item, idx) in visibleItems"
        :key="item.id"
        class="attachments__item"
        :class="{
          'attachments__item--selected': selectedIdsSet.has(item.id),
          'attachments__item--disabled': disabled
        }"
        :role="layout === 'grid' ? 'gridcell' : 'listitem'"
        :aria-selected="selectable ? selectedIdsSet.has(item.id) : undefined"
        :tabindex="tabIndexFor(renderIndexStart + idx)"
        @click="onItemClick(item, renderIndexStart + idx, $event)"
        @mousedown.prevent
        @focus="focusedIndex = renderIndexStart + idx"
        @keyup.enter.prevent="emitClick(item)"
        @keyup.space.prevent="emitClick(item)"
      >
        <!-- Icon / thumbnail -->
        <div class="attachments__icon">
          <slot name="icon" :item="item">
            <component :is="resolvedIconComponent(item)" class="attachments__icon-svg" aria-hidden="true" />
          </slot>
        </div>

        <!-- Content -->
        <div class="attachments__content">
          <div class="attachments__name" :title="item.filename">
            {{ truncated(item.filename) }}
          </div>
          <div class="attachments__meta">
            <span class="attachments__meta-date">{{ formatDate(item.uploadedAt) }}</span>
            <span v-if="showSize && item.sizeBytes != null" class="attachments__meta-size">• {{ formatBytes(item.sizeBytes) }}</span>
          </div>
        </div>

        <!-- Actions -->
        <div v-if="showActions" class="attachments__actions">
          <slot name="actions" :item="item">
            <a
              class="attachments__action attachments__action--download"
              :href="safeUrl(item.fileUrl)"
              :download="download ? item.filename : undefined"
              :target="openInNewTab ? '_blank' : undefined"
              :rel="openInNewTab ? 'noopener noreferrer' : undefined"
              @click.stop="emit('download', item)"
              :aria-label="t('attachments.download', 'Download') + ' ' + item.filename"
            >
              <IconDownload class="attachments__action-icon" aria-hidden="true" />
            </a>
            <button
              class="attachments__action attachments__action--remove"
              type="button"
              @click.stop="emit('remove', item)"
              :aria-label="t('attachments.remove', 'Remove') + ' ' + item.filename"
            >
              <IconTrash class="attachments__action-icon" aria-hidden="true" />
            </button>
          </slot>
        </div>
      </div>

      <!-- Virtualized spacer (list mode only) -->
      <div v-if="virtualized && layout === 'list'" :style="bottomSpacerStyle" />
    </div>
  </section>
</template>
<style scoped lang="scss">
/* BEM: attachments */
.attachments {
  display: grid;
  gap: 0.75rem;

  &--disabled { opacity: 0.6; pointer-events: none; }
  &--list .attachments__container { display: grid; grid-auto-rows: 56px; }
  &--grid .attachments__container { display: grid; grid-template-columns: repeat(auto-fill, minmax(240px, 1fr)); gap: 0.75rem; }

  &__error {
    background: rgba(255, 0, 0, 0.08);
    border: 1px solid rgba(255,0,0,0.25);
    padding: 0.5rem 0.75rem;
    border-radius: 12px;
    display: grid;
    grid-auto-flow: column;
    align-items: center;
    justify-content: start;
    gap: 0.5rem;
  }
  &__error-retry {
    border: 0;
    background: transparent;
    cursor: pointer;
    text-decoration: underline;
  }

  &__skeletons {
    display: grid;
    gap: 0.5rem;
  }
  &__skeleton {
    border-radius: 12px;
    background: linear-gradient(90deg, #eee 25%, #f6f6f6 37%, #eee 63%);
    background-size: 400% 100%;
    animation: attachments-shimmer 1.2s infinite;
    &--list { height: 56px; }
    &--grid { height: 120px; }
  }
  @keyframes attachments-shimmer { 0%{background-position: 100% 0} 100%{background-position: 0 0} }

  &__empty { color: #666; padding: 0.5rem; }

  &__container {
    overflow: auto;
    max-height: 480px; /* adjust as needed */
  }

  &__item {
    display: grid;
    grid-template-columns: 32px 1fr auto;
    align-items: center;
    gap: 0.5rem;
    padding: 0.5rem 0.5rem;
    border-radius: 12px;
    border: 1px solid rgba(0,0,0,0.06);
    background: rgba(255,255,255,0.6);
    transition: box-shadow .15s ease, background .15s ease;

    &:hover { box-shadow: 0 1px 10px rgba(0,0,0,0.05); background: rgba(255,255,255,0.9); }
    &:focus, &:focus-within { outline: none; box-shadow: 0 0 0 2px rgba(99, 102, 241, .35) inset; }
    &--selected { box-shadow: 0 0 0 2px rgba(59, 130, 246, .5) inset; }
    &--disabled { opacity: .6; pointer-events: none; }
  }

  &--grid &__item {
    grid-template-columns: 1fr;
    grid-template-rows: auto auto auto;
    min-height: 120px;
  }

  &__icon {
    width: 24px; height: 24px; display: grid; place-items: center; justify-self: center;
  }
  &__icon-svg { width: 20px; height: 20px; font-size: 20px; }

  &__content {
    display: grid;
    gap: 0.125rem;
    align-content: center;
  }
  &__name {
    font-weight: 600;
    overflow: hidden; text-overflow: ellipsis; white-space: nowrap;
  }
  &__meta {
    font-size: 0.85rem;
    color: #666;
    display: inline-flex; gap: 0.5rem;
  }

  &__actions {
    display: inline-grid; grid-auto-flow: column; gap: 0.25rem;
  }
  &__action {
    border: none;
    background: transparent;
    cursor: pointer;
    padding: 0.25rem;
    border-radius: 10px;
    &:hover { background: rgba(0,0,0,0.05); }
    &-icon { width: 18px; height: 18px; font-size: 18px; }
  }
}
</style>
