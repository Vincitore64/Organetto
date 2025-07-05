import type { BoardDto } from '@/dataAccess/boards/models'
import type { BoardPageView } from './types'
import { defaultThumbnailHref } from './static'

export function mapToBoardPageView(b: BoardDto): BoardPageView {
  return {
    id: b.id,
    title: b.title ?? '',
    // description: b.description ?? '',
    thumbnailUrl: defaultThumbnailHref,
    // createdAt: b.createdAt,
    // updatedAt: b.updatedAt,
    // isArchived: b.isArchived,
  }
}
