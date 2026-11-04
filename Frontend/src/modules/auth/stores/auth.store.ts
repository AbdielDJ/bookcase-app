import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { authService } from '../services/auth.service'
import type { LoginRequest, RegisterRequest, User } from '../types/auth.types'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const initialized = ref(false)
  const isAuthenticated = computed(() => !!user.value)

  async function fetchUser() {
    try {
      user.value = await authService.me()
    } catch {
      user.value = null
    } finally {
      initialized.value = true
    }
  }

  async function login(payload: LoginRequest) {
    await authService.login(payload)
    user.value = await authService.me()
  }

  async function register(payload: RegisterRequest) {
    await authService.register(payload)
  }

  async function logout() {
    try {
      await authService.logout()
    } finally {
      user.value = null
    }
  }

  return { user, initialized, isAuthenticated, fetchUser, login, register, logout }
})
