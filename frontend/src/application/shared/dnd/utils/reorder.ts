import type { CardVm, ColumnVm } from '@/application/boards'
import type { Id } from '../models'

function moveColumn(cols: ColumnVm[], from: number, to: number): ColumnVm[] {
  // debugger
  const next = cols.slice();
  const [item] = next.splice(from, 1);
  next.splice(to, 0, item);
  return normalizeColumnPositions(next);
}

function moveCard(
  cols: ColumnVm[],
  fromColumnId: Id,
  fromIndex: number,
  toColumnId: Id,
  toIndex: number
): ColumnVm[] {
  const next = cols.map(c => ({ ...c, cards: c.cards.slice() }));
  const fromCol = next.find(c => c.id === fromColumnId)!;
  const toCol = next.find(c => c.id === toColumnId)!;

  const [card] = fromCol.cards.splice(fromIndex, 1);
  toCol.cards.splice(toIndex, 0, card);

  return normalizeAllPositions(next);
}

function addCard(cols: ColumnVm[],
  card: CardVm,
  toColumnId: Id,
  toIndex: number
): ColumnVm[] {
  const next = cols.map(c => ({ ...c, cards: c.cards.slice() }));
  const toCol = next.find(c => c.id === toColumnId)!;
  toCol.cards.splice(toIndex, 0, card);
  return normalizeAllPositions(next);
}

function removeCard(cols: ColumnVm[],
  fromColumnId: Id,
  fromIndex: number
) {
  const next = cols.map(c => ({ ...c, cards: [...c.cards] }));
  // const next = cols
  const fromCol = next.find(c => c.id === fromColumnId)!;
  const [card] = fromCol.cards.splice(fromIndex, 1);
  return normalizeAllPositions(next);
}

function normalizeColumnPositions(cols: ColumnVm[]): ColumnVm[] {
  return cols.map((c, i) => ({ ...c, position: i + 1 }));
}

function normalizeAllPositions(cols: ColumnVm[]): ColumnVm[] {
  return cols.map((c, ci) => ({
    ...c,
    position: ci + 1,
    cards: c.cards.map((card, i) => ({ ...card, position: i + 1 })),
  }));
}

export { moveColumn, moveCard, addCard, removeCard }

