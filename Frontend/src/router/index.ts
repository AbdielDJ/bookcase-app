import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '@/shared/layouts/MainLayout.vue'
import { authRoutes } from '@/modules/auth/routes'
import { booksRoutes } from '@/modules/books/routes'
import { authorsRoutes } from '@/modules/authors/routes'
import { useAuthStore } from '@/modules/auth/stores/auth.store'

declare module 'vue-router' {
  interface RouteMeta {
    requiresAuth?: boolean
    guestOnly?: boolean
  }
}

const router = createRouter({
  history: createWebHistory(),
  routes: [
    ...authRoutes,
    {
      path: '/',
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        { path: '', redirect: { name: 'books' } },
        ...booksRoutes,
        ...authorsRoutes,
      ],
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('@/shared/views/NotFoundView.vue'),
    },
  ],
})

router.beforeEach(async (to) => {
  const auth = useAuthStore()

  // La primera vez, preguntamos al backend si hay sesión (cookie válida)
  if (!auth.initialized) await auth.fetchUser()

  if (to.matched.some((r) => r.meta.requiresAuth) && !auth.isAuthenticated) {
    return { name: 'login', query: { redirect: to.fullPath } }
  }
  if (to.meta.guestOnly && auth.isAuthenticated) {
    return { name: 'books' }
  }
})

export default router
