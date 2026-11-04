import type { RouteRecordRaw } from 'vue-router'

export const booksRoutes: RouteRecordRaw[] = [
  {
    path: 'books',
    name: 'books',
    component: () => import('./views/BooksListView.vue'),
  },
  {
    path: 'books/new',
    name: 'book-create',
    component: () => import('./views/BookFormView.vue'),
  },
  {
    path: 'books/:id(\\d+)/edit',
    name: 'book-edit',
    component: () => import('./views/BookFormView.vue'),
  },
]
