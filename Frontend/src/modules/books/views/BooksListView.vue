<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RouterLink } from 'vue-router'
import ConfirmDialog from '@/shared/components/ConfirmDialog.vue'
import { getErrorMessage } from '@/core/api/errors'
import { booksService } from '../services/books.service'
import type { Book } from '../types/book.types'

const books = ref<Book[]>([])
const loading = ref(true)
const error = ref('')
const actionError = ref('')
const toDelete = ref<Book | null>(null)

onMounted(async () => {
  try {
    books.value = await booksService.getAll()
  } catch (e) {
    error.value = getErrorMessage(e, 'No se pudieron cargar los libros.')
  } finally {
    loading.value = false
  }
})

async function confirmDelete() {
  const target = toDelete.value
  toDelete.value = null
  if (!target) return

  actionError.value = ''
  try {
    await booksService.remove(target.id)
    books.value = books.value.filter((b) => b.id !== target.id)
  } catch (e) {
    actionError.value = getErrorMessage(e, 'No se pudo eliminar el libro.')
  }
}
</script>

<template>
  <section>
    <div class="page-head">
      <h1>Libros</h1>
      <RouterLink :to="{ name: 'book-create' }" class="btn">Nuevo libro</RouterLink>
    </div>

    <p v-if="loading">Cargando...</p>
    <p v-else-if="error" class="error">{{ error }}</p>
    <template v-else>
      <p v-if="actionError" class="error">{{ actionError }}</p>
      <p v-if="!books.length">Aún no hay libros registrados.</p>
      <table v-else>
        <thead>
          <tr>
            <th>ID</th>
            <th>Título</th>
            <th>Autor</th>
            <th class="actions-cell">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="book in books" :key="book.id">
            <td>{{ book.id }}</td>
            <td>{{ book.title }}</td>
            <td>{{ book.authorName ?? book.authorId }}</td>
            <td class="actions-cell">
              <div class="row-actions">
                <RouterLink :to="{ name: 'book-edit', params: { id: book.id } }" class="btn-outline btn-sm">
                  Editar
                </RouterLink>
                <button type="button" class="btn btn-danger btn-sm" @click="toDelete = book">Eliminar</button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </template>

    <ConfirmDialog
      :open="toDelete !== null"
      title="Eliminar libro"
      :message="`¿Seguro que quieres eliminar «${toDelete?.title ?? ''}»? Esta acción no se puede deshacer.`"
      @confirm="confirmDelete"
      @cancel="toDelete = null"
    />
  </section>
</template>
