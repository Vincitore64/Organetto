import { computed, onBeforeUnmount, reactive, type Ref, ref, watch } from 'vue'
import { useDebounceFn } from '@vueuse/core'
import type { CardVm, ColumnVm } from '@/application/boards'
import _ from 'lodash'

// REMEMBER: FOR DRAGABLE ALWAYS NEED A LOCAL STATE!!!!!!!!!

type Id = number

interface MoveColumnPayload {
  /**
   * Column identifier.
   */
  listId: number
  /**
   * Target board identifier.
   */
  targetBoardId: number
  /**
   * Left sibling identifier.
   */
  leftSiblingId: number
  /**
   * Right sibling identifier.
   */
  rightSiblingId: number
}

interface MoveCardPayload {
  cardId: number
  targetColumnId: number
  leftSiblingId: number
  rightSiblingId: number
}

interface UseKanbanDndOptions {
  debounceMs?: number
  columnHandle?: string
  cardHandle?: string
  onPersistColumns?: (payload: MoveColumnPayload) => Promise<unknown> | void
  onPersistCardMove?: (payload: MoveCardPayload) => Promise<unknown> | void
  onErrorRollback?: (prev: ColumnVm[]) => void
}

interface UseKanbanDndReturn {
  // Bindings for <draggable> at columns level:
  columnDraggableBind: Ref<Record<string, unknown>>;
  // Bindings for each column's card list:
  cardDraggableBind: (col: ColumnVm) => Record<string, unknown>;
  // Apply to the card list container to encode column id safely:
  cardListAttrs: (col: ColumnVm) => Record<string, string | number>;
  // utils
  flushPending(): void;  // flush debounced saves
}

function useKanbanDnd(originColumnsRef: Ref<ColumnVm[]>, boardId: Ref<number>, opts: UseKanbanDndOptions) : UseKanbanDndReturn {
  // local state
  const columns = ref(originColumnsRef.value)

  watch(originColumnsRef, (v) => {
    columns.value = v
  })


  const conf = {
    debounceMs: opts.debounceMs ?? 400,
    columnHandle: opts.columnHandle ?? '.col-handle',
    cardHandle: opts.cardHandle ?? '.card-handle',
  }

  const state = reactive({
    snapshot: [] as ColumnVm[],        // deep-ish snapshot for rollback
  })

  function columnFromLocalState(id: Id) {
    return columns.value.find(c => c.id === id)
  }


  // --- helpers ---
  function cloneBoard(src: ColumnVm[]): ColumnVm[] {
    return src.map(c => ({ ...c, cards: c.cards.map(card => ({ ...card })) }))
  }
  function normalizeColumns(cols: ColumnVm[]) {
    cols.forEach((c, i) => { c.position = i + 1 })
  }
  function normalizeCards(col: ColumnVm) {
    col.cards.forEach((card, i) => (card.position = i + 1))
  }

  // --- debounced persistence ---
  const persistColumns = useDebounceFn(async (columnIndex: number) => {
    // normalizeColumns(columns.value)
    const column = columns.value[columnIndex]
    const payload: MoveColumnPayload = {
      listId: column.id,
      targetBoardId: boardId.value,
      leftSiblingId: columns.value[columnIndex - 1]?.id ?? null,
      rightSiblingId: columns.value[columnIndex + 1]?.id ?? null,
    }
    try {
      await opts.onPersistColumns?.(payload)
    } catch {
      // rollback
      if (opts.onErrorRollback) opts.onErrorRollback(state.snapshot)
      else columns.value = cloneBoard(state.snapshot)
    }
  }, conf.debounceMs)

  const persistCardMove = useDebounceFn(async (p: { toColumnId: Id; toIndex: number }) => { // cardId: Id; fromColumnId: Id; 
    try {
      // debugger
      const column = _(columns.value).find(c => c.id === p.toColumnId)
      if (!column) return
      const payload: MoveCardPayload = {
        cardId: column.cards[p.toIndex].id,
        targetColumnId: p.toColumnId,
        leftSiblingId: column.cards[p.toIndex - 1]?.id ?? null,
        rightSiblingId: column.cards[p.toIndex + 1]?.id ?? null,
      }

      await opts.onPersistCardMove?.(payload)
    } catch {
      // debugger
      if (opts.onErrorRollback) opts.onErrorRollback(state.snapshot)
      else columns.value = cloneBoard(state.snapshot)
    }
  }, conf.debounceMs)

  function takeSnapshot() {
    state.snapshot = cloneBoard(columns.value)
  }

  const columnDraggableBind = computed(() => ({
    get modelValue() { return columns.value },

    'onUpdate:modelValue': (v: ColumnVm[]) => {
      columns.value = v
    },
    itemKey: 'id',
    group: 'columns',
    animation: 180,
    ghostClass: 'ghost',
    // start/end for snapshot/flush
    onStart: () => takeSnapshot(),
    onEnd: (e: CustomEvent & { oldIndex: number, newIndex: number }) => {
      console.log('onEnd', e)
      persistColumns(e.newIndex)
      originColumnsRef.value = columns.value
    }, // ensure final call
    onMove(...params: unknown[]) {
      console.log('onMove', params)
    },
    // tip: @change for columns exists but @end is sufficient to persist new order
    tag: 'ul', // keep list semantics
    class: 'kanban-columns-grid', // lists-container
  }))

  function cardListAttrs(col: ColumnVm) {
    // used internally to decode from/to column id without leaking dataset details in the view code
    return { 'data-column-id': String(col.id) }
  }

  function cardDraggableBind(columnOrigin: ColumnVm) {
    // debugger
    let columnState = columnFromLocalState(columnOrigin.id) // Because we don't know if col is in local state yet
    if (!columnState) {
      console.warn('cardDraggableBind: no column found in local state')
      columnState = columnOrigin
    }
    return {
      get modelValue() { return columnState.cards },
      'onUpdate:modelValue': (v: CardVm[]) => {
        columnState.cards = v
      },
      itemKey: 'id',
      group: 'cards',
      // handle: conf.cardHandle,
      animation: 150,
      // fallbackOnBody: true,
      swapThreshold: 0.65,
      ghostClass: 'ghost',
      tag: 'ol',
      class: 'cards',
      ...cardListAttrs(columnState),
      // richer diff than @end; fires for add/remove/move
      onStart: () => takeSnapshot(),
      onChange: (e: any) => {
        // Normalize only changed lists for perf
        if (e?.moved) {
          // normalizeCards(columnState)
          const toIndex: number = e.moved.newIndex
          const toColumnId: Id = columnState.id
          persistCardMove({ toColumnId, toIndex })
        }
        if (e?.added) {
          // const card: CardVm = e.added.element
          const toIndex: number = e.added.newIndex
          const toColumnId: Id = columnState.id
          // find fromColumnId using DOM containers (reliable with Sortable)
          // const fromColumn = state.snapshot.find(c => c.cards.some(k => k.id === card.id))
          // if (!fromColumn) throw new Error()
          // const fromColumnId = fromColumn?.id
          // const fromIndex = fromColumn?.cards.findIndex(k => k.id === card.id)
          // normalizeCards(col)
          persistCardMove({ toColumnId, toIndex })
          // const added = addCard(columns.value, card, toColumnId, toIndex)
          // columns.value = moveCard(columns.value, fromColumnId, fromIndex, toColumnId, toIndex)

        }
        if (e?.removed) {
          // normalize source column too (it is 'this' column when removing)
          // columns.value = removeCard(columns.value, col.id, e.removed.oldIndex)

          // normalizeCards(col)
        }
      },
      onEnd: (e: CustomEvent & { oldIndex: number, newIndex: number }) => {
        console.log('onEnd', columnState, columnState, e)
        originColumnsRef.value = columns.value

      }, // persistCardMove.flush(); persistColumns.flush()
    } as Record<string, unknown>
  }

  // const cardDraggableBind = computed(() => cardDraggableBindFn)

  function flushPending() {
    
  }

  onBeforeUnmount(() => {
    flushPending()
  })

  return { columnDraggableBind, cardDraggableBind, cardListAttrs, flushPending }
}

export { useKanbanDnd, type UseKanbanDndOptions, type UseKanbanDndReturn }

