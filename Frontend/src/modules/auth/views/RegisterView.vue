<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import AuthShell from '@/shared/layouts/AuthShell.vue'
import AppInput from '@/shared/components/AppInput.vue'
import GlassButton from '@/shared/components/GlassButton.vue'
import { registerSchema } from '../schemas/auth.schema'
import { useAuthStore } from '../stores/auth.store'

const auth = useAuthStore()
const router = useRouter()
const serverError = ref('')

const { handleSubmit, errors, defineField, isSubmitting } = useForm({
  validationSchema: toTypedSchema(registerSchema),
})

const [name, nameAttrs] = defineField('name')
const [email, emailAttrs] = defineField('email')
const [password, passwordAttrs] = defineField('password')
const [confirmPassword, confirmPasswordAttrs] = defineField('confirmPassword')

const onSubmit = handleSubmit(async (values) => {
  serverError.value = ''
  try {
    await auth.register({
      name: values.name,
      email: values.email,
      password: values.password,
    })
    router.push({ name: 'login' })
  } catch {
    serverError.value = 'No se pudo crear la cuenta. Intenta con otro correo.'
  }
})
</script>

<template>
  <AuthShell title="Crea tu cuenta" subtitle="Un rincón seco para guardar tus libros.">
    <form class="auth-form" novalidate @submit.prevent="onSubmit">
      <AppInput
        v-model="name"
        v-bind="nameAttrs"
        label="Nombre"
        autocomplete="name"
        :error="errors.name"
      />
      <AppInput
        v-model="email"
        v-bind="emailAttrs"
        label="Correo"
        type="email"
        autocomplete="email"
        :error="errors.email"
      />
      <AppInput
        v-model="password"
        v-bind="passwordAttrs"
        label="Contraseña"
        type="password"
        autocomplete="new-password"
        :error="errors.password"
      />
      <AppInput
        v-model="confirmPassword"
        v-bind="confirmPasswordAttrs"
        label="Confirmar contraseña"
        type="password"
        autocomplete="new-password"
        :error="errors.confirmPassword"
      />

      <p v-if="serverError" class="auth-alert" role="alert">{{ serverError }}</p>

      <GlassButton type="submit" :disabled="isSubmitting">
        {{ isSubmitting ? 'Creando...' : 'Crear cuenta' }}
      </GlassButton>
    </form>

    <template #footer>
      ¿Ya tienes cuenta? <RouterLink to="/login">Inicia sesión</RouterLink>
    </template>
  </AuthShell>
</template>
