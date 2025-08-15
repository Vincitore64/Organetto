import { createI18n } from 'vue-i18n'
import enRegistration from './en/registration/index.json'
import ruRegistration from './ru/registration.json'
import enLogin from './en/login.json'
import ruLogin from './ru/login.json'
import enMainLayout from './en/mainLayout.json'
import ruMainLayout from './ru/mainLayout.json'
import enBoards from './en/boards.json'
import ruBoards from './ru/boards.json'
import enBoard from './en/board.json'
import ruBoard from './ru/board.json'
import enAttachments from './en/attachments.json'
import ruAttachments from './ru/attachments.json'
import enCommon from './en/common.json'
import ruCommon from './ru/common.json'

const messages = {
  en: {
    registration: enRegistration,
    login: enLogin,
    mainLayout: enMainLayout,
    boards: enBoards,
    board: enBoard,
    attachments: enAttachments,
    common: enCommon,
  },  
  ru: {
    registration: ruRegistration,
    login: ruLogin,
    mainLayout: ruMainLayout,
    boards: ruBoards,
    board: ruBoard,
    attachments: ruAttachments,
    common: ruCommon,
  },
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
