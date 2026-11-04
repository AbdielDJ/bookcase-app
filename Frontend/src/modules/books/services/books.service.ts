import { http } from '@/core/api/http'
import type { Book, BookPayload } from '../types/book.types'

export const booksService = {
  getAll: async () => (await http.get<Book[]>('/books')).data,
  getById: async (id: number) => (await http.get<Book>(`/books/${id}`)).data,
  create: async (payload: BookPayload) => (await http.post<Book>('/books', payload)).data,
  update: (id: number, payload: BookPayload) => http.put(`/books/${id}`, payload),
  remove: (id: number) => http.delete(`/books/${id}`),
}
