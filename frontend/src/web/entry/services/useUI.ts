import type { App } from 'vue'
import Antd from 'ant-design-vue'

export function useUI(app: App) {
  return app.use(Antd)
}
