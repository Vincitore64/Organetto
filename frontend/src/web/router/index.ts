import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'

// All page components live under src/web/pages and are imported synchronously
import HomePage from '@/web/pages/HomePage.vue'
import LoginPage from '@/web/pages/auth/LoginPage.vue'
import RegisterPage from '@/web/pages/auth/RegistrationPage.vue'
import BoardsPage from '@/web/pages/boards/BoardsPage.vue'
import BoardPage from '@/web/pages/boards/BoardPage.vue'
import NotFoundPage from '@/web/pages/NotFoundPage.vue'
import { useAuthToken } from '@/application/authentication/hooks/useAuthToken'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: HomePage,
    meta: { requiresAuth: true },
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginPage,
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterPage,
  },
  {
    path: '/boards/:userId',
    name: 'Boards',
    component: BoardsPage,
    props: true,
    meta: { requiresAuth: true },
  },
  {
    path: '/boards/:userId/:id',
    name: 'Board',
    component: BoardPage,
    props: true,
    meta: { requiresAuth: true },
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: NotFoundPage,
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

// Global navigation guard to enforce authentication on protected routes
router.beforeEach((to, from, next) => {
  const tokenWrapper = useAuthToken()
  // const isAuthenticated = true
  if (to.meta.requiresAuth && !tokenWrapper.isAuthenticated.value) {
    next({ name: 'Login' })
  } else {
    next()
  }
})

export default router
