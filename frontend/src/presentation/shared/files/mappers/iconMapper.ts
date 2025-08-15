import type { MapperFn } from '@/application/shared/mappers'
import type { IconCategory } from '@/shared'
import { IconAudio, IconCode, IconDoc, IconFile, IconImage, IconPdf, IconPpt, IconSheet, IconVideo, IconZip } from '../components'

const defaultIconMap: Record<IconCategory, any> = {
  file: IconFile, image: IconImage, video: IconVideo, audio: IconAudio,
  pdf: IconPdf, archive: IconZip, code: IconCode, sheet: IconSheet,
  slides: IconPpt, doc: IconDoc
}

const fileIconMapper : MapperFn<IconCategory, any> = (category) => {
  return defaultIconMap[category] ?? defaultIconMap.file
}

export { fileIconMapper }

