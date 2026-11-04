<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { RouterLink, useRoute, useRouter } from 'vue-router'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import BaseField from '@/shared/components/BaseField.vue'
import { getErrorMessage } from '@/core/api/errors'
import { authorSchema } from '../schemas/author.schema'
import { authorsService } from '../services/authors.service'

const route = useRoute()
const router = useRouter()

const id = computed(() => (route.params.id ? Number(route.params.id) : null))
const isEdit = computed(() => id.value !== null)

const loading = ref(false)
const serverError = ref('')

const { handleSubmit, errors, defineField, isSubmitting, setValues } = useForm({
  validationSchema: toTypedSchema(authorSchema),
})

const [name, nameAttrs] = defineField('name')

onMounted(async () => {
  if (id.value === null) return
  loading.value = true
  try {
    const author = await authorsService.getById(id.value)
    setValues({ name: author.name })
  } catch (e) {
    serverError.value = getErrorMessage(e, 'No se pudo cargar el autor.')
  } finally {
    loading.value = false
  }
})

const onSubmit = handleSubmit(async (values) => {
  serverError.value = ''
  try {
    if (id.value !== null) await authorsService.update(id.value, values)
    else await authorsService.create(values)
    router.push({ name: 'authors' })
  } catch (e) {
    serverError.value = getErrorMessage(e, 'No se pudo guardar el autor.')
  }
})
</script>

<template>
  <section>
    <div class="page-head">
      <h1>{{ isEdit ? 'Editar autor' : 'Nuevo autor' }}</h1>
    </div>

    <p v-if="loading">Cargando...</p>
    <form v-else class="form-card" novalidate @submit.prevent="onSubmit">
      <BaseField v-model="name" v-bind="nameAttrs" label="Nombre" :error="errors.name" />

      <p v-if="serverError" class="error">{{ serverError }}</p>

      <div class="form-actions">
        <RouterLink :to="{ name: 'authors' }" class="btn-outline">Cancelar</RouterLink>
        <button type="submit" class="btn" :disabled="isSubmitting">
          {{ isSubmitting ? 'Guardando...' : 'Guardar' }}
        </button>
      </div>
    </form>
  </section>
</template>
