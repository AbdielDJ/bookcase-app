<script setup lang="ts">
import { RouterLink, RouterView, useRouter } from 'vue-router'
import { useAuthStore } from '@/modules/auth/stores/auth.store'

const auth = useAuthStore()
const router = useRouter()

async function logout() {
  await auth.logout()
  router.push({ name: 'login' })
}
</script>

<template>
  <header class="navbar">
    <strong>📚 Bookcase</strong>
    <nav>
      <RouterLink to="/books">Libros</RouterLink>
      <RouterLink to="/authors">Autores</RouterLink>
    </nav>
    <div class="user">
      <span>{{ auth.user?.name }}</span>
      <button class="btn-outline" @click="logout">Salir</button>
    </div>
  </header>
  <main class="container">
    <RouterView />
  </main>
</template>
