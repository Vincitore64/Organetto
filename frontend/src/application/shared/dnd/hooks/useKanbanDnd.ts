import { computed, onBeforeUnmount, reactive, type Ref, ref, watch } from 'vue'
import { useDebounceFn } from '@vueuse/core'
import type { CardVm, ColumnVm } from '@/application/boards'
// import { addCard, moveCard, moveColumn, removeCard } from '../utils'
import _ from 'lodash'
import { updateCollectionItem } from '@/shared'

// REMEMBER: FOR DRAGABLE ALWAYS NEED A LOCAL STATE!!!!!!!!!

export type Id = number

export interface UseKanbanDndOptions {
  debounceMs?: number
  columnHandle?: string
  cardHandle?: string
  onPersistColumns?: (cols: ColumnVm[]) => Promise<unknown> | void
  onPersistCardMove?: (p: { cardId: Id; fromColumnId: Id; toColumnId: Id; toIndex: number; columns: ColumnVm[] }) => Promise<unknown> | void
  onErrorRollback?: (prev: ColumnVm[]) => void
}

export interface UseKanbanDndReturn {
  // Bindings for <draggable> at columns level:
  columnDraggableBind: Ref<Record<string, unknown>>;
  // Bindings for each column's card list:
  cardDraggableBind: (col: ColumnVm) => Record<string, unknown>;
  // Apply to the card list container to encode column id safely:
  cardListAttrs: (col: ColumnVm) => Record<string, string | number>;
  // utils
  flushPending(): void;  // flush debounced saves
}

export function useKanbanDnd(originColumnsRef: Ref<ColumnVm[]>, opts: UseKanbanDndOptions = {}) : UseKanbanDndReturn {
  // local state
  const columns = ref(originColumnsRef.value)

  watch(originColumnsRef, (v) => {
    debugger
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
  const persistColumns = useDebounceFn(async () => {
    // normalizeColumns(columns.value)
    console.log(_.cloneDeep(columns.value))
    try {
      await opts.onPersistColumns?.(columns.value)
    } catch {
      // rollback
      if (opts.onErrorRollback) opts.onErrorRollback(state.snapshot)
      else columns.value = cloneBoard(state.snapshot)
    }
  }, conf.debounceMs)

  const persistCardMove = useDebounceFn(async (payload: { cardId: Id; fromColumnId: Id; toColumnId: Id; toIndex: number }) => {
    try {
      await opts.onPersistCardMove?.({ ...payload, columns: columns.value })
    } catch {
      debugger
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
      persistColumns()
      originColumnsRef.value = columns.value
    }, // ensure final call
    onMove(...params: unknown[]) {
      console.log('onMove', params)
    },
    // tip: @change for columns exists but @end is sufficient to persist new order
    tag: 'ul', // keep list semantics
    class: 'columns-grid lists-container', // lists-container
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
        // columnState.cards = v
        // columns.value = columns.value.map(c =>
        //   c.id === col.id ? { ...c, cards: v.slice() } : c
        // )
        // updateCollectionItem(columns,
        //   column => column.id === col.id,
        //   colClone => {
        //     colClone.cards = v
        //   })
        // columns.value = _.cloneDeep(columns.value)


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
          normalizeCards(columnState)
          persistColumns() // positions changed within same column (optional)
          // const moved = moveCard(columns.value, col.id, e.moved.oldIndex, col.id, e.moved.newIndex)
          // columns.value = moved
        }
        if (e?.added) {
          const card: CardVm = e.added.element
          const toIndex: number = e.added.newIndex
          const toColumnId: Id = columnState.id
          // find fromColumnId using DOM containers (reliable with Sortable)
          const fromColumn = state.snapshot.find(c => c.cards.some(k => k.id === card.id))
          if (!fromColumn) throw new Error()
          const fromColumnId = fromColumn?.id
          const fromIndex = fromColumn?.cards.findIndex(k => k.id === card.id)
          // normalizeCards(col)
          persistCardMove({ cardId: card.id, fromColumnId: Number(fromColumnId ?? ''), toColumnId, toIndex })
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
