import { ProvidedService } from '@/shared'
import type { App } from 'vue'
import { createPinia } from 'pinia'
import router from '@/web/router'
import { container } from 'tsyringe'
import { createLocalization } from '@/web/i18n'

/**
 * Initializes app with pinia and router
 * @param app The app to initialize
 */

export function useServices(app: App) {
  app.use(createPinia())
  app.use(router)
  const localization = createLocalization()
  app.use(localization)

  container.registerInstance(ProvidedService.Router, router)
  container.registerInstance(ProvidedService.Localization, localization)
  app.provide(ProvidedService.Router, container.resolve(ProvidedService.Router))

  return app
}
