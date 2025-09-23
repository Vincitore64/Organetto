type Id = string | number;

type DragKind = 'column' | 'card';

interface DragSource {
  kind: DragKind;
  columnId: Id;
  cardIndex?: number;     // only for kind='card'
  columnIndex?: number;   // only for kind='column'
}

export { type Id, type DragKind, type DragSource }


