import 'reflect-metadata'
import { createApp } from 'vue'

import App from '@/web/entry/components/App.vue'
import { useUI } from './services/useUI'
import 'ant-design-vue/dist/reset.css'
import '@/presentation/shared/assets/styles/index.scss'
import { useServices } from './services/useServices'

const app = useServices(useUI(createApp(App)))

app.mount('#app')
