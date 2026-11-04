<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { RouterLink, useRoute, useRouter } from 'vue-router'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import BaseField from '@/shared/components/BaseField.vue'
import { getErrorMessage } from '@/core/api/errors'
import { authorsService } from '@/modules/authors/services/authors.service'
import type { Author } from '@/modules/authors/types/author.types'
import { bookSchema } from '../schemas/book.schema'
import { booksService } from '../services/books.service'

const route = useRoute()
const router = useRouter()

const id = computed(() => (route.params.id ? Number(route.params.id) : null))
const isEdit = computed(() => id.value !== null)

const authors = ref<Author[]>([])
const loading = ref(true)
const serverError = ref('')

// authorId = 0 representa "ningún autor seleccionado" (el esquema exige un número positivo)
const { handleSubmit, errors, defineField, isSubmitting, setValues } = useForm({
  validationSchema: toTypedSchema(bookSchema),
  initialValues: { title: '', authorId: 0 },
})

const [title, titleAttrs] = defineField('title')
const [authorId, authorIdAttrs] = defineField('authorId')

onMounted(async () => {
  try {
    authors.value = await authorsService.getAll()
    if (id.value !== null) {
      const book = await booksService.getById(id.value)
      setValues({ title: book.title, authorId: book.authorId })
    }
  } catch (e) {
    serverError.value = getErrorMessage(e, 'No se pudieron cargar los datos.')
  } finally {
    loading.value = false
  }
})

const onSubmit = handleSubmit(async (values) => {
  serverError.value = ''
  try {
    if (id.value !== null) await booksService.update(id.value, values)
    else await booksService.create(values)
    router.push({ name: 'books' })
  } catch (e) {
    serverError.value = getErrorMessage(e, 'No se pudo guardar el libro.')
  }
})
</script>

<template>
  <section>
    <div class="page-head">
      <h1>{{ isEdit ? 'Editar libro' : 'Nuevo libro' }}</h1>
    </div>

    <p v-if="loading">Cargando...</p>
    <form v-else class="form-card" novalidate @submit.prevent="onSubmit">
      <BaseField v-model="title" v-bind="titleAttrs" label="Título" :error="errors.title" />

      <div class="field">
        <label for="book-author">Autor</label>
        <select
          id="book-author"
          v-model.number="authorId"
          v-bind="authorIdAttrs"
          :class="{ invalid: errors.authorId }"
        >
          <option :value="0" disabled>Selecciona un autor</option>
          <option v-for="author in authors" :key="author.id" :value="author.id">
            {{ author.name }}
          </option>
        </select>
        <small v-if="errors.authorId" class="error" role="alert">{{ errors.authorId }}</small>
        <small v-if="!authors.length" class="hint-text">
          Aún no hay autores. <RouterLink :to="{ name: 'author-create' }">Crea uno primero</RouterLink>.
        </small>
      </div>

      <p v-if="serverError" class="error">{{ serverError }}</p>

      <div class="form-actions">
        <RouterLink :to="{ name: 'books' }" class="btn-outline">Cancelar</RouterLink>
        <button type="submit" class="btn" :disabled="isSubmitting">
          {{ isSubmitting ? 'Guardando...' : 'Guardar' }}
        </button>
      </div>
    </form>
  </section>
</template>
