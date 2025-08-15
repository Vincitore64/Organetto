import path from 'path-browserify'
import { extByCategory, type IconCategory } from '../models'

const ext = (name?: string) => {
  if (!name) return ''
  return path.extname(name).slice(1).toLowerCase()

  // const i = name.lastIndexOf('.')

  // return i >= 0 ? name.slice(i+1).toLowerCase() : ''
}

function resolveIconCategory(mime?: string, filename?: string): IconCategory {
  const m = (mime ?? '').toLowerCase()
  const e = ext(filename)
  if (m.startsWith('image/') || extByCategory.image.includes(e)) return 'image'
  if (m.startsWith('video/') || extByCategory.video.includes(e)) return 'video'
  if (m.startsWith('audio/') || extByCategory.audio.includes(e)) return 'audio'
  if (m === 'application/pdf' || e === 'pdf') return 'pdf'
  for (const cat of ['archive','code','sheet','slides','doc'] as IconCategory[]) {
    if (extByCategory[cat].includes(e)) return cat
  }
  return 'file'
}

export {
  ext,
  resolveIconCategory
}
