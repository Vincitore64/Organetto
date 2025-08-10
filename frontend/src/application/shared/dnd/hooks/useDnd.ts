import { computed, onBeforeUnmount, reactive, ref, type Ref } from 'vue'
import { useRafFn } from '@vueuse/core'
import { moveCard, moveColumn } from '../utils'

import type { DragSource, DragKind, Id } from '../models'
import { useMoveColumn, type ColumnVm } from '@/application/boards'
import _ from 'lodash'
import type { MoveColumnCommand } from '@/dataAccess/columns/models'

type Rect = DOMRect

export function useDnd(columnsRef: { value: ColumnVm[] }, boardIdFn: () => number) {
  const containerRef = ref<HTMLElement | null>(null) // horizontal scroll area for columns
  const columnEls = ref<HTMLElement[]>([])
  const cardElsMap = ref<Map<Id, HTMLElement[]>>(new Map())
  const moveState = useMoveColumn()

  const state = reactive({
    dragging: false,
    kind: null as DragKind | null,
    pointerX: 0,
    pointerY: 0,
    ghostW: 0,
    ghostH: 0,
    ghostOffsetX: 0,
    ghostOffsetY: 0,
    source: null as DragSource | null,
    overColumnIndex: -1,
    overCardIndex: -1, // insertion index between cards
    ghostEl: null as HTMLElement | null,
  })

  function registerContainer(el: HTMLElement | null) {
    containerRef.value = el
  }
  function registerColumnEl(i: number, el: HTMLElement | null) {
    if (!el) return
    columnEls.value[i] = el
  }
  function registerCardEls(columnId: Id, els: (HTMLElement | null)[]) {
    const arr = els.filter(Boolean) as HTMLElement[]
    cardElsMap.value.set(columnId, arr)
  }

  function getColumnRects(): Rect[] {
    return _(columnEls.value).map((el) => el?.getBoundingClientRect?.()).compact().value()
  }
  function getCardRects(columnId: Id): Rect[] {
    const arr = cardElsMap.value.get(columnId) ?? []
    return _(arr).map((el) => el?.getBoundingClientRect?.()).compact().value()
  }

  function findColumnIndexByX(x: number): number {
    const rects = getColumnRects()
    if (!rects.length) return -1
    let idx = rects.findIndex((r) => x < r.left + r.width / 2)
    if (idx === -1) idx = rects.length - 1
    return idx
  }

  function findCardInsertIndex(columnId: Id, y: number): number {
    const rects = getCardRects(columnId)
    if (!rects.length) return 0
    let idx = rects.findIndex((r) => y < r.top + r.height / 2)
    if (idx === -1) idx = rects.length
    return idx
  }

  // Autoscroll near edges
  const scrollSpeed = 18
  const edge = 64
  const { pause: pauseScroll, resume: resumeScroll } = useRafFn(() => {
    const c = containerRef.value
    if (!c || !state.dragging) return
    const cr = c.getBoundingClientRect()
    if (state.pointerX > cr.right - edge) c.scrollLeft += scrollSpeed
    else if (state.pointerX < cr.left + edge) c.scrollLeft -= scrollSpeed
    // vertical (inside a column) – if needed, add similar logic for inner scrollables
  })

  const  debouncedMoveColumnAsync = _.debounce((params: MoveColumnCommand) => {
    moveState.mutateAsync(params)
  }, 500)


  function onPointerMove(e: PointerEvent) {
    // debugger
    if (!state.dragging) return
    state.pointerX = e.clientX
    state.pointerY = e.clientY

    const cols = columnsRef.value
    if (state.kind === 'column') {
      const to = findColumnIndexByX(state.pointerX)
      if (to !== -1 && state.source?.columnIndex !== undefined) {
        const from = state.source.columnIndex
        if (from !== to) {
          // debugger
          const moved = moveColumn(cols, from, to)
          columnsRef.value = moved
          debouncedMoveColumnAsync({
            listId: moved[to].id,
            targetBoardId: boardIdFn(),
            leftSiblingId: moved[to - 1]?.id ?? null,
            rightSiblingId: moved[to + 1]?.id ?? null,
          })
          // update source columnIndex to the new position
          state.source = {
            ...state.source!,
            columnIndex: to,
            kind: 'column',
            columnId: moved[to].id,
          }
        }
      }
    } else if (state.kind === 'card') {
      const toColumnIdx = findColumnIndexByX(state.pointerX)
      state.overColumnIndex = toColumnIdx

      if (toColumnIdx !== -1) {
        const toColumnId = columnsRef.value[toColumnIdx].id
        const insertAt = findCardInsertIndex(toColumnId, state.pointerY)
        state.overCardIndex = insertAt

        const s = state.source!
        const fromColId = s.columnId
        const fromIdx = s.cardIndex!
        const toIdx = insertAt

        // Preview by actually moving items in local state
        const next = moveCard(columnsRef.value, fromColId, fromIdx, toColumnId, toIdx)
        columnsRef.value = next

        // Update source to reflect new position (so subsequent moves are relative)
        state.source = { kind: 'card', columnId: toColumnId, cardIndex: toIdx }
      }
    }
  }

  function onPointerUp() {
    // debugger
    if (!state.dragging) return
    cleanupListeners()
    state.dragging = false
    pauseScroll()
  }

  function onPointerDown(
    e: PointerEvent,
    kind: DragKind,
    payload: { columnId: Id; cardIndex?: number; columnIndex?: number },
    elForGhost: HTMLElement,
  ) {
    // debugger
    // Allow text selection with long press on mobile? For simplicity we start immediately.
    e.preventDefault();
    (e.target as HTMLElement).setPointerCapture?.(e.pointerId)

    state.dragging = true
    state.kind = kind
    state.pointerX = e.clientX
    state.pointerY = e.clientY
    state.source = {
      kind,
      columnId: payload.columnId,
      cardIndex: payload.cardIndex,
      columnIndex: payload.columnIndex,
    }

    const r = elForGhost.getBoundingClientRect()
    state.ghostW = r.width
    state.ghostH = r.height
    state.ghostOffsetX = e.clientX - r.left
    state.ghostOffsetY = e.clientY - r.top
    window.addEventListener('pointermove', onPointerMove, { passive: false })
    window.addEventListener('pointerup', onPointerUp)
    resumeScroll()
  }

  function cleanupListeners() {
    window.removeEventListener('pointermove', onPointerMove as any)
    window.removeEventListener('pointerup', onPointerUp as any)
  }

  onBeforeUnmount(() => {
    cleanupListeners()
    pauseScroll()
  })

  const ghostStyle = computed(() => {
    if (!state.dragging) return null
    return {
      left: `${state.pointerX - state.ghostOffsetX}px`,
      top: `${state.pointerY - state.ghostOffsetY}px`,
      width: `${state.ghostW}px`,
      height: `${state.ghostH}px`,
    }
  })

  return {
    // refs
    registerContainer: containerRef,
    registerColumnEl,
    registerCardEls,
    // actions
    onPointerDown,
    // state
    state,
    ghostStyle,
  }
}

export type UseDndReturn = ReturnType<typeof useDnd>
