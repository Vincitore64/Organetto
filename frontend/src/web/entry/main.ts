import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from '@/web/entry/components/App.vue'
import router from '../router'
import { useUI } from './services/useUI'
import 'ant-design-vue/dist/reset.css'
import '@/assets/main.css'

const app = useUI(createApp(App))

app.use(createPinia())
app.use(router)

app.mount('#app')
