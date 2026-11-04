<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RouterLink } from 'vue-router'
import ConfirmDialog from '@/shared/components/ConfirmDialog.vue'
import { getErrorMessage } from '@/core/api/errors'
import { authorsService } from '../services/authors.service'
import type { Author } from '../types/author.types'

const authors = ref<Author[]>([])
const loading = ref(true)
const error = ref('')
const actionError = ref('')
const toDelete = ref<Author | null>(null)

onMounted(async () => {
  try {
    authors.value = await authorsService.getAll()
  } catch (e) {
    error.value = getErrorMessage(e, 'No se pudieron cargar los autores.')
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
    await authorsService.remove(target.id)
    authors.value = authors.value.filter((a) => a.id !== target.id)
  } catch (e) {
    actionError.value = getErrorMessage(e, 'No se pudo eliminar el autor.')
  }
}
</script>

<template>
  <section>
    <div class="page-head">
      <h1>Autores</h1>
      <RouterLink :to="{ name: 'author-create' }" class="btn">Nuevo autor</RouterLink>
    </div>

    <p v-if="loading">Cargando...</p>
    <p v-else-if="error" class="error">{{ error }}</p>
    <template v-else>
      <p v-if="actionError" class="error">{{ actionError }}</p>
      <p v-if="!authors.length">Aún no hay autores registrados.</p>
      <table v-else>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th class="actions-cell">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="author in authors" :key="author.id">
            <td>{{ author.id }}</td>
            <td>{{ author.name }}</td>
            <td class="actions-cell">
              <div class="row-actions">
                <RouterLink :to="{ name: 'author-edit', params: { id: author.id } }" class="btn-outline btn-sm">
                  Editar
                </RouterLink>
                <button type="button" class="btn btn-danger btn-sm" @click="toDelete = author">Eliminar</button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </template>

    <ConfirmDialog
      :open="toDelete !== null"
      title="Eliminar autor"
      :message="`¿Seguro que quieres eliminar a ${toDelete?.name ?? ''}? Esta acción no se puede deshacer.`"
      @confirm="confirmDelete"
      @cancel="toDelete = null"
    />
  </section>
</template>
