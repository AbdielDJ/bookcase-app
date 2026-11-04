<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, useRoute, useRouter } from 'vue-router'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import AuthShell from '@/shared/layouts/AuthShell.vue'
import AppInput from '@/shared/components/AppInput.vue'
import GlassButton from '@/shared/components/GlassButton.vue'
import { loginSchema } from '../schemas/auth.schema'
import { useAuthStore } from '../stores/auth.store'

const auth = useAuthStore()
const router = useRouter()
const route = useRoute()
const serverError = ref('')

const { handleSubmit, errors, defineField, isSubmitting } = useForm({
  validationSchema: toTypedSchema(loginSchema),
})

const [email, emailAttrs] = defineField('email')
const [password, passwordAttrs] = defineField('password')

const onSubmit = handleSubmit(async (values) => {
  serverError.value = ''
  try {
    await auth.login(values)
    router.push((route.query.redirect as string) || { name: 'books' })
  } catch {
    serverError.value = 'Correo o contraseña incorrectos.'
  }
})
</script>

<template>
  <AuthShell title="Entra a Bookcase" subtitle="Tu biblioteca te espera, aunque afuera llueva.">
    <form class="auth-form" novalidate @submit.prevent="onSubmit">
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
        autocomplete="current-password"
        :error="errors.password"
      />

      <p v-if="serverError" class="auth-alert" role="alert">{{ serverError }}</p>

      <GlassButton type="submit" :disabled="isSubmitting">
        {{ isSubmitting ? 'Entrando...' : 'Entrar' }}
      </GlassButton>
    </form>

    <template #footer>
      ¿No tienes cuenta? <RouterLink to="/register">Crea una</RouterLink>
    </template>
  </AuthShell>
</template>
