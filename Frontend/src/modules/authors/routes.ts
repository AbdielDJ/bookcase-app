import type { RouteRecordRaw } from 'vue-router'

export const authorsRoutes: RouteRecordRaw[] = [
  {
    path: 'authors',
    name: 'authors',
    component: () => import('./views/AuthorsListView.vue'),
  },
  {
    path: 'authors/new',
    name: 'author-create',
    component: () => import('./views/AuthorFormView.vue'),
  },
  {
    path: 'authors/:id(\\d+)/edit',
    name: 'author-edit',
    component: () => import('./views/AuthorFormView.vue'),
  },
]
