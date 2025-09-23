const formatBytes = (bytes?: number) => {
  if (bytes == null) return ''
  if (bytes < 1024) return `${bytes} B`
  const units = ['KB','MB','GB','TB']
  let i = -1, n = bytes
  do { n /= 1024; i++ } while (n >= 1024 && i < units.length-1)
  return `${Math.round(n*10)/10} ${units[i]}`
}
const formatDate = (d: Date) =>
  new Intl.DateTimeFormat(undefined, { dateStyle: 'medium', timeStyle: 'short' }).format(d)

export {
  formatBytes,
  formatDate
}
