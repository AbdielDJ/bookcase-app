import { http } from '@/core/api/http'
import type { LoginRequest, RegisterRequest, User } from '../types/auth.types'

export const authService = {
  login: (payload: LoginRequest) => http.post('/auth/login', payload),
  register: (payload: RegisterRequest) => http.post('/auth/register', payload),
  logout: () => http.post('/auth/logout'),
  me: async () => (await http.get<User>('/auth/me')).data,
}
