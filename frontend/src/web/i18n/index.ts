import { createI18n } from 'vue-i18n'
import enRegistration from './en/registration/index.json'
import ruRegistration from './ru/registration.json'

const messages = {
  en: { registration: enRegistration },
  ru: { registration: ruRegistration },
}

export function createLocalization() {
  const i18n = createI18n({
    legacy: false,
    locale: 'en',
    fallbackLocale: 'en',
    messages,
  })
  return i18n
}
