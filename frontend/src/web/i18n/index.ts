import { createI18n } from 'vue-i18n'
import enRegistration from './en/registration/index.json'
import ruRegistration from './ru/registration.json'
import enLogin from './en/login.json'
import ruLogin from './ru/login.json'

const messages = {
  en: { registration: enRegistration, login: enLogin },
  ru: { registration: ruRegistration, login: ruLogin },
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
