import type { App } from 'vue'
import Antd from 'ant-design-vue'

/**
 * Configures the Vue app to use Ant Design Vue UI components.
 * @param app The Vue app instance to configure
 * @returns The Vue app instance with Ant Design Vue components installed
 */

export function useUI(app: App) {
  return app.use(Antd)
}
