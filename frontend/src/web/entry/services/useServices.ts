import { ProvidedService } from '@/shared'
import type { App } from 'vue'
import { createPinia } from 'pinia'
import router from '@/web/router'
import { container } from 'tsyringe'
import { createLocalization } from '@/web/i18n'
import { attachAuthInterceptor, createAxios } from '@/dataAccess/shared/http'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import { useAuthStore } from '@/application/authentication/stores/authStore'

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

  // Axios singleton
  const axiosInstance = createAxios('https://localhost:44322') // import.meta.env.VITE_API_BASE_URL
  attachAuthInterceptor(axiosInstance)
  container.registerInstance(ProvidedService.AxiosInstance, axiosInstance)

  container.registerSingleton(ApiClient)

  // AuthStore factory (lazy)
  container.register(ProvidedService.AuthStore, {
    useFactory: () => useAuthStore(container.resolve(ApiClient)),
  })

  app.provide(ProvidedService.Container, container)

  return app
}
