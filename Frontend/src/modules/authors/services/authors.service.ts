import { http } from '@/core/api/http'
import type { Author, AuthorPayload } from '../types/author.types'

export const authorsService = {
  getAll: async () => (await http.get<Author[]>('/authors')).data,
  getById: async (id: number) => (await http.get<Author>(`/authors/${id}`)).data,
  create: async (payload: AuthorPayload) => (await http.post<Author>('/authors', payload)).data,
  update: (id: number, payload: AuthorPayload) => http.put(`/authors/${id}`, payload),
  remove: (id: number) => http.delete(`/authors/${id}`),
}
