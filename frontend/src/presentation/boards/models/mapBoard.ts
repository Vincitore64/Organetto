import type { BoardDto } from '@/dataAccess/boards/models'
import type { BoardPageView } from './types'
import { defaultThumbnailHref } from './static'
import dayjs from 'dayjs'

export function mapToBoardPageView(b: BoardDto): BoardPageView {
  return {
    id: b.id,
    title: b.title ?? '',
    // description: b.description ?? '',
    thumbnailUrl: defaultThumbnailHref,
    // createdAt: b.createdAt,
    updatedAt: dayjs(b.updatedAt).format('L LT'),
    updatedText: dayjs(b.updatedAt).fromNow(),
    // isArchived: b.isArchived,
  }
}
