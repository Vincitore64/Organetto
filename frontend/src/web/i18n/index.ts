import { createI18n } from 'vue-i18n'
import registration from './en/registration/index.json'

const messages = {
  en: { registration },
}

export function createLocalization() {
  const i18n = createI18n({
    locale: 'en',
    fallbackLocale: 'en',
    messages,
  })
  return i18n
}
