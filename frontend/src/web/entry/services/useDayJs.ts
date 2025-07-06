import dayjs from 'dayjs'
import relativeTime from 'dayjs/plugin/relativeTime'
import localizedFormat from 'dayjs/plugin/localizedFormat'

export function useDayJs() {
  dayjs.extend(relativeTime)
  dayjs.extend(localizedFormat)
}
