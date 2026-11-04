<script setup lang="ts">
import { computed, ref } from 'vue'

defineOptions({ inheritAttrs: false })

const props = defineProps<{
  label: string
  type?: string
  error?: string
}>()

const model = defineModel<string>()

const id = 'f-' + Math.random().toString(36).slice(2, 9)
const isPassword = computed(() => props.type === 'password')
const visible = ref(false)
const inputType = computed(() => (isPassword.value && visible.value ? 'text' : (props.type ?? 'text')))
</script>

<template>
  <div class="g-field" :class="{ invalid: error }">
    <input
      :id="id"
      v-model="model"
      class="g-input"
      :class="{ 'g-input--pw': isPassword }"
      :type="inputType"
      placeholder=" "
      :aria-invalid="!!error"
      :aria-describedby="error ? id + '-err' : undefined"
      v-bind="$attrs"
    />
    <label class="g-label" :for="id">{{ label }}</label>

    <button
      v-if="isPassword"
      type="button"
      class="g-eye"
      :aria-label="visible ? 'Ocultar contraseña' : 'Mostrar contraseña'"
      @click="visible = !visible"
    >
      <svg viewBox="0 0 24 24" width="18" height="18" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
        <path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12z" />
        <circle cx="12" cy="12" r="3" />
        <path v-if="!visible" d="M3 3l18 18" />
      </svg>
    </button>

    <small v-if="error" :id="id + '-err'" class="g-error" role="alert">{{ error }}</small>
  </div>
</template>

<style scoped>
.g-field { position: relative; }

.g-input {
  width: 100%;
  height: 56px;
  padding: 22px 22px 6px;
  font: inherit;
  font-size: 1rem;
  color: #f2fbff;
  caret-color: #8fe9ff;
  background: rgba(255, 255, 255, 0.07);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 999px;
  box-shadow: inset 0 2px 10px rgba(0, 0, 0, 0.25), inset 0 -1px 0 rgba(255, 255, 255, 0.08);
  -webkit-backdrop-filter: blur(6px);
  backdrop-filter: blur(6px);
  outline: none;
  transition: border-color 0.25s, background 0.25s, box-shadow 0.25s;
}
.g-input--pw { padding-right: 56px; }
.g-input:hover { border-color: rgba(255, 255, 255, 0.32); }
.g-input:focus {
  border-color: rgba(143, 233, 255, 0.85);
  background: rgba(255, 255, 255, 0.11);
  box-shadow:
    inset 0 2px 10px rgba(0, 0, 0, 0.25),
    0 0 0 4px rgba(127, 227, 255, 0.16),
    0 0 26px rgba(127, 227, 255, 0.25);
}
/* el autocompletado de Chrome no debe pintar de blanco el campo */
.g-input:-webkit-autofill {
  -webkit-text-fill-color: #f2fbff;
  -webkit-box-shadow: 0 0 0 60px #123646 inset;
  transition: background-color 9999s ease-out;
}

.g-label {
  position: absolute;
  left: 22px;
  top: 28px;
  transform: translateY(-50%);
  transform-origin: left center;
  color: rgba(214, 234, 242, 0.7);
  pointer-events: none;
  transition: top 0.2s ease, transform 0.2s ease, color 0.2s ease;
}
.g-input:focus + .g-label,
.g-input:not(:placeholder-shown) + .g-label {
  top: 17px;
  transform: translateY(-50%) scale(0.78);
  color: #8fe9ff;
}

.g-eye {
  position: absolute;
  right: 10px;
  top: 10px;
  width: 36px;
  height: 36px;
  display: grid;
  place-items: center;
  border: 0;
  border-radius: 50%;
  cursor: pointer;
  color: #cfeef7;
  background: rgba(255, 255, 255, 0.08);
  transition: background 0.2s, color 0.2s;
}
.g-eye:hover { background: rgba(255, 255, 255, 0.16); color: #fff; }
.g-eye:focus-visible { outline: 2px solid #8fe9ff; outline-offset: 2px; }

.g-field.invalid .g-input { border-color: rgba(255, 150, 175, 0.8); }
.g-field.invalid .g-input:focus {
  box-shadow: inset 0 2px 10px rgba(0, 0, 0, 0.25), 0 0 0 4px rgba(255, 150, 175, 0.16);
}
.g-error {
  display: block;
  margin: 7px 0 0 22px;
  color: #ffb8c6;
  font-size: 0.82rem;
  line-height: 1.35;
}

@media (prefers-reduced-motion: reduce) {
  .g-input, .g-label, .g-eye { transition: none; }
}
</style>
