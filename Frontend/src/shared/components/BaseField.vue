<script setup lang="ts">
defineOptions({ inheritAttrs: false })

defineProps<{
  label: string
  type?: string
  error?: string
}>()

const model = defineModel<string>()
const id = 'bf-' + Math.random().toString(36).slice(2, 9)
</script>

<template>
  <div class="field">
    <label :for="id">{{ label }}</label>
    <input
      :id="id"
      v-model="model"
      :type="type ?? 'text'"
      :class="{ invalid: error }"
      :aria-invalid="!!error"
      v-bind="$attrs"
    />
    <small v-if="error" class="error" role="alert">{{ error }}</small>
  </div>
</template>
