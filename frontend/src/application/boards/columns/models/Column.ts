import type { ColumnDto } from '@/dataAccess/columns/models'
import type { CardVm } from '../cards'

/** View‑model for a column with its cards. */
export interface ColumnVm extends Omit<ColumnDto, 'cards'> {
  cards: CardVm[]
  /** UI state: collapsed lists are rendered with minified height. */
  collapsed: boolean
}