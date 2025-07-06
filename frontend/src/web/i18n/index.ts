import { createI18n } from 'vue-i18n'
import enRegistration from './en/registration/index.json'
import ruRegistration from './ru/registration.json'
import enLogin from './en/login.json'
import ruLogin from './ru/login.json'
import enMainLayout from './en/mainLayout.json'
import ruMainLayout from './ru/mainLayout.json'
import enBoards from './en/boards.json'
import ruBoards from './ru/boards.json'

const messages = {
  en: { registration: enRegistration, login: enLogin, mainLayout: enMainLayout, boards: enBoards },
  ru: { registration: ruRegistration, login: ruLogin, mainLayout: ruMainLayout, boards: ruBoards },
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
