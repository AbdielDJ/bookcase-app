<script setup lang="ts">
import { ref } from 'vue'

interface Ripple { id: number; x: number; y: number }

const ripples = ref<Ripple[]>([])
let seq = 0

// Al presionar, nace un anillo en el punto exacto del toque, como una burbuja que se expande
function onPointerDown(e: PointerEvent) {
  const rect = (e.currentTarget as HTMLElement).getBoundingClientRect()
  const id = ++seq
  ripples.value.push({ id, x: e.clientX - rect.left, y: e.clientY - rect.top })
  window.setTimeout(() => {
    ripples.value = ripples.value.filter((r) => r.id !== id)
  }, 800)
}
</script>

<template>
  <button class="g-btn" @pointerdown="onPointerDown">
    <span class="g-btn__label"><slot /></span>
    <span
      v-for="r in ripples"
      :key="r.id"
      class="g-btn__ripple"
      :style="{ left: r.x + 'px', top: r.y + 'px' }"
    />
  </button>
</template>

<style scoped>
.g-btn {
  position: relative;
  overflow: hidden;
  height: 56px;
  width: 100%;
  border: 0;
  border-radius: 999px;
  cursor: pointer;
  font: inherit;
  font-size: 1.02rem;
  font-weight: 600;
  letter-spacing: 0.01em;
  color: #052431;
  background: linear-gradient(135deg, #8aeaff 0%, #4fb4f0 100%);
  box-shadow:
    inset 0 -6px 14px rgba(20, 90, 140, 0.35),
    0 10px 30px rgba(79, 180, 240, 0.35),
    0 0 0 1px rgba(255, 255, 255, 0.25);
  transition: transform 0.2s ease, box-shadow 0.25s ease, filter 0.25s ease;
}
/* reflejo brillante de burbuja */
.g-btn::before {
  content: '';
  position: absolute;
  inset: 4px 14% auto 14%;
  height: 46%;
  border-radius: 999px;
  background: linear-gradient(rgba(255, 255, 255, 0.65), rgba(255, 255, 255, 0.04));
  pointer-events: none;
}
.g-btn:hover:not(:disabled) {
  transform: translateY(-1px);
  filter: brightness(1.06);
  box-shadow:
    inset 0 -6px 14px rgba(20, 90, 140, 0.35),
    0 14px 38px rgba(79, 180, 240, 0.5),
    0 0 0 1px rgba(255, 255, 255, 0.35);
}
.g-btn:active:not(:disabled) { transform: scale(0.985); }
.g-btn:focus-visible { outline: 3px solid rgba(255, 255, 255, 0.85); outline-offset: 3px; }
.g-btn:disabled { opacity: 0.6; cursor: not-allowed; }

.g-btn__label { position: relative; }
.g-btn__ripple {
  position: absolute;
  width: 14px;
  height: 14px;
  margin: -7px 0 0 -7px;
  border-radius: 50%;
  border: 2px solid rgba(255, 255, 255, 0.85);
  pointer-events: none;
  animation: pop 0.75s ease-out forwards;
}
@keyframes pop {
  from { transform: scale(0.4); opacity: 0.9; }
  to { transform: scale(22); opacity: 0; }
}

@media (prefers-reduced-motion: reduce) {
  .g-btn, .g-btn__ripple { transition: none; animation: none; }
  .g-btn__ripple { display: none; }
}
</style>
