<script setup lang="ts">
import { ref, watch } from 'vue'

const props = defineProps<{
  open: boolean
  title: string
  message: string
  confirmLabel?: string
}>()

const emit = defineEmits<{
  confirm: []
  cancel: []
}>()

const dialog = ref<HTMLDialogElement | null>(null)

watch(
  () => props.open,
  (open) => {
    const el = dialog.value
    if (!el) return
    if (open && !el.open) el.showModal()
    if (!open && el.open) el.close()
  },
  { flush: 'post' },
)
</script>

<template>
  <dialog ref="dialog" class="confirm" @cancel.prevent="emit('cancel')" @click.self="emit('cancel')">
    <h2>{{ title }}</h2>
    <p>{{ message }}</p>
    <div class="confirm__actions">
      <button type="button" class="btn-outline" @click="emit('cancel')">Cancelar</button>
      <button type="button" class="btn btn-danger" @click="emit('confirm')">
        {{ confirmLabel ?? 'Eliminar' }}
      </button>
    </div>
  </dialog>
</template>
