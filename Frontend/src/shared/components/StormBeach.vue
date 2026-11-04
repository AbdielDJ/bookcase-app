<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref } from 'vue'

/**
 * Playa de noche bajo la tormenta:
 * cielo + luna tapada por nubes, faro con haz giratorio, mar con olas,
 * orilla con espuma, burbujas subiendo, lluvia (canvas) con salpicaduras,
 * y un relámpago de vez en cuando.
 */

const canvas = ref<HTMLCanvasElement | null>(null)
const flashing = ref(false)

// Burbujas: valores aleatorios generados una sola vez
const bubbles = Array.from({ length: 14 }, (_, id) => ({
  id,
  size: 10 + Math.random() * 62,
  left: Math.random() * 100,
  duration: 16 + Math.random() * 18,
  delay: -Math.random() * 34,
  sway: 18 + Math.random() * 46,
}))

// ---------- Lluvia ----------
interface Drop { x: number; y: number; ty: number; len: number; speed: number; depth: number }
interface Ripple { x: number; y: number; r: number; max: number }

const WIND = 0.22 // desplazamiento horizontal por cada píxel que cae
let ctx: CanvasRenderingContext2D | null = null
let drops: Drop[] = []
let ripples: Ripple[] = []
let w = 0
let h = 0
let raf = 0
let last = 0
let reduced = false
const timers: number[] = []

function newDrop(initial: boolean): Drop {
  const horizon = h * 0.56
  const depth = Math.random()
  const ty = horizon + (h - horizon) * (0.04 + depth * 0.96) // dónde "aterriza"
  const len = 10 + depth * 24
  return {
    depth,
    ty,
    len,
    speed: 650 + depth * 950,
    x: Math.random() * (w + h * WIND),
    y: initial ? Math.random() * ty : -len - Math.random() * h * 0.4,
  }
}

function resize() {
  const el = canvas.value
  if (!el || !ctx) return
  const dpr = Math.min(window.devicePixelRatio || 1, 2)
  w = window.innerWidth
  h = window.innerHeight
  el.width = Math.round(w * dpr)
  el.height = Math.round(h * dpr)
  ctx.setTransform(dpr, 0, 0, dpr, 0, 0)
  const count = Math.round(Math.min(420, (w * h) / 5500))
  drops = Array.from({ length: count }, () => newDrop(true))
  ripples = []
  if (reduced) draw()
}

function draw() {
  if (!ctx) return
  ctx.clearRect(0, 0, w, h)
  ctx.lineCap = 'round'

  for (const d of drops) {
    ctx.strokeStyle = `rgba(190, 222, 240, ${0.1 + d.depth * 0.36})`
    ctx.lineWidth = 0.6 + d.depth * 1.1
    ctx.beginPath()
    ctx.moveTo(d.x, d.y)
    ctx.lineTo(d.x + WIND * d.len, d.y - d.len)
    ctx.stroke()
  }

  ctx.lineWidth = 1
  for (const r of ripples) {
    ctx.strokeStyle = `rgba(200, 232, 245, ${0.5 * (1 - r.r / r.max)})`
    ctx.beginPath()
    ctx.ellipse(r.x, r.y, r.r, r.r * 0.32, 0, 0, Math.PI * 2)
    ctx.stroke()
  }
}

function frame(now: number) {
  const dt = Math.min((now - last) / 1000, 0.05)
  last = now

  for (const d of drops) {
    d.y += d.speed * dt
    d.x -= d.speed * dt * WIND
    if (d.y >= d.ty) {
      ripples.push({ x: d.x, y: d.ty, r: 1, max: 5 + d.depth * 20 })
      Object.assign(d, newDrop(false))
    }
  }
  if (ripples.length > 180) ripples.splice(0, ripples.length - 180)

  for (const r of ripples) r.r += dt * r.max * 1.6
  ripples = ripples.filter((r) => r.r < r.max)

  draw()
  raf = requestAnimationFrame(frame)
}

// ---------- Relámpago ----------
function scheduleFlash(wait: number) {
  timers.push(
    window.setTimeout(() => {
      flashing.value = true
      timers.push(
        window.setTimeout(() => {
          flashing.value = false
          scheduleFlash(7000 + Math.random() * 9000)
        }, 950),
      )
    }, wait),
  )
}

onMounted(() => {
  const el = canvas.value
  if (!el) return
  ctx = el.getContext('2d')
  if (!ctx) return

  reduced = window.matchMedia('(prefers-reduced-motion: reduce)').matches
  resize()
  window.addEventListener('resize', resize)

  if (reduced) return // sin movimiento: se deja un cuadro estático
  last = performance.now()
  raf = requestAnimationFrame(frame)
  scheduleFlash(3500)
})

onBeforeUnmount(() => {
  cancelAnimationFrame(raf)
  window.removeEventListener('resize', resize)
  timers.forEach((t) => window.clearTimeout(t))
})
</script>

<template>
  <div class="storm" aria-hidden="true">
    <div class="sky" />
    <div class="moon" />
    <div class="cloud cloud--a" />
    <div class="cloud cloud--b" />
    <div class="cloud cloud--c" />

    <div class="lighthouse">
      <div class="lighthouse__beam"><span /></div>
      <svg viewBox="0 0 44 110" xmlns="http://www.w3.org/2000/svg">
        <defs>
          <radialGradient id="lh-glow">
            <stop offset="0" stop-color="#ffe3a3" stop-opacity="0.85" />
            <stop offset="1" stop-color="#ffe3a3" stop-opacity="0" />
          </radialGradient>
        </defs>
        <path d="M2 110 L8 98 L36 98 L42 110 Z" fill="#02060b" />
        <path d="M15 98 L18.5 34 H25.5 L29 98 Z" fill="#050b13" />
        <path d="M16.9 52 H27.1 L27.7 60 H16.3 Z" fill="#0d1b2a" />
        <path d="M15.9 74 H28.1 L28.8 82 H15.2 Z" fill="#0d1b2a" />
        <rect x="14.5" y="31" width="15" height="3" fill="#050b13" />
        <circle cx="22" cy="26.5" r="10" fill="url(#lh-glow)" />
        <rect x="18" y="22" width="8" height="9" fill="#ffd98a" />
        <path d="M16 22 L22 12 L28 22 Z" fill="#050b13" />
        <rect x="21.5" y="7" width="1" height="6" fill="#050b13" />
      </svg>
    </div>

    <div class="sea">
      <div class="glint" />
      <div class="wave wave--back">
        <svg viewBox="0 0 1200 120" preserveAspectRatio="none">
          <path d="M0 60 C100 20 200 20 300 60 S500 100 600 60 S800 20 900 60 S1100 100 1200 60 V120 H0 Z" />
        </svg>
      </div>
      <div class="wave wave--front">
        <svg viewBox="0 0 1200 120" preserveAspectRatio="none">
          <path d="M0 60 C100 20 200 20 300 60 S500 100 600 60 S800 20 900 60 S1100 100 1200 60 V120 H0 Z" />
        </svg>
      </div>
    </div>

    <div class="shore">
      <div class="shore__foam">
        <svg viewBox="0 0 1200 120" preserveAspectRatio="none">
          <path d="M0 60 C100 20 200 20 300 60 S500 100 600 60 S800 20 900 60 S1100 100 1200 60 V120 H0 Z" />
        </svg>
      </div>
      <div class="shore__edge">
        <svg viewBox="0 0 1200 120" preserveAspectRatio="none">
          <path d="M0 60 C100 20 200 20 300 60 S500 100 600 60 S800 20 900 60 S1100 100 1200 60 V120 H0 Z" />
        </svg>
      </div>
      <div class="shore__sand" />
    </div>

    <span
      v-for="b in bubbles"
      :key="b.id"
      class="bubble"
      :style="{
        width: b.size + 'px',
        height: b.size + 'px',
        left: b.left + '%',
        '--dur': b.duration + 's',
        '--delay': b.delay + 's',
        '--sway': b.sway + 'px',
      }"
    />

    <canvas ref="canvas" class="rain" />
    <div class="flash" :class="{ on: flashing }" />
    <div class="vignette" />
  </div>
</template>

<style scoped>
.storm {
  position: fixed;
  inset: 0;
  z-index: 0;
  overflow: hidden;
  background: #03060c;
  pointer-events: none;
}
.storm > * { position: absolute; }

/* ---- Cielo, luna y nubes ---- */
.sky {
  inset: 0 0 44% 0;
  background: linear-gradient(to bottom, #02040a 0%, #07101d 45%, #0f2433 85%, #17394a 100%);
}
.moon {
  left: 72%;
  top: 9%;
  width: clamp(70px, 9vw, 130px);
  aspect-ratio: 1;
  border-radius: 50%;
  background: radial-gradient(circle at 38% 36%, #f4f9fc, #c9d9e4 60%, #9fb4c4);
  box-shadow: 0 0 60px 20px rgba(190, 220, 240, 0.28), 0 0 160px 60px rgba(120, 170, 210, 0.18);
  opacity: 0.8;
}
.cloud {
  border-radius: 50%;
  filter: blur(28px);
  background: radial-gradient(closest-side, rgba(7, 12, 20, 0.92), rgba(7, 12, 20, 0));
  animation: drift linear infinite alternate;
}
.cloud--a { left: -10%; top: 2%; width: 70vw; height: 24vh; animation-duration: 70s; }
.cloud--b { left: 42%; top: 9%; width: 60vw; height: 20vh; animation-duration: 90s; }
.cloud--c { left: 5%; top: 20%; width: 50vw; height: 14vh; animation-duration: 110s; opacity: 0.8; }

@keyframes drift {
  from { transform: translateX(-6vw); }
  to { transform: translateX(8vw); }
}

/* ---- Faro ---- */
.lighthouse {
  left: 12%;
  bottom: 44%;
  height: clamp(90px, 17vh, 160px);
  aspect-ratio: 44 / 110;
}
.lighthouse svg { width: 100%; height: 100%; display: block; }
.lighthouse__beam {
  position: absolute;
  left: 50%;
  top: 24%;
  width: 0;
  height: 0;
}
.lighthouse__beam span {
  position: absolute;
  left: 0;
  top: -40px;
  width: 46vw;
  height: 80px;
  transform-origin: 0 50%;
  background: linear-gradient(to right, rgba(255, 226, 160, 0.5), rgba(255, 226, 160, 0) 90%);
  clip-path: polygon(0 46%, 100% 0, 100% 100%, 0 54%);
  filter: blur(2px);
  animation: beam 9s ease-in-out infinite alternate;
}
@keyframes beam {
  from { transform: scaleX(1); }
  to { transform: scaleX(-1); }
}

/* ---- Mar ---- */
.sea {
  left: 0;
  right: 0;
  top: 56%;
  bottom: 0;
  background: linear-gradient(to bottom, #123a4e 0%, #0a2736 30%, #040f18 100%);
}
.sea > * { position: absolute; }
.glint {
  left: calc(72% + clamp(35px, 4.5vw, 65px));
  transform: translateX(-50%);
  top: 0;
  width: 110px;
  height: 34%;
  background: repeating-linear-gradient(to bottom, rgba(210, 230, 242, 0.55) 0 2px, transparent 2px 10px);
  -webkit-mask-image: radial-gradient(ellipse 50% 100% at 50% 0, #000, transparent);
  mask-image: radial-gradient(ellipse 50% 100% at 50% 0, #000, transparent);
  animation: shimmer 3s linear infinite;
}
@keyframes shimmer {
  from { background-position: 0 0; opacity: 0.9; }
  50% { opacity: 0.55; }
  to { background-position: 0 20px; opacity: 0.9; }
}
.wave {
  left: 0;
  width: 200%;
  height: 46px;
  animation: sway linear infinite;
}
.wave svg, .shore svg { width: 100%; height: 100%; display: block; }
.wave--back { top: -18px; opacity: 0.5; animation-duration: 22s; }
.wave--back svg { fill: #1a566e; }
.wave--front { top: -6px; opacity: 0.9; animation-duration: 14s; animation-direction: reverse; }
.wave--front svg { fill: #0c2e3f; }

@keyframes sway {
  from { transform: translateX(0); }
  to { transform: translateX(-50%); }
}

/* ---- Orilla ---- */
.shore {
  left: 0;
  right: 0;
  bottom: 0;
  height: 21%;
  transform-origin: bottom;
  animation: tide 7s ease-in-out infinite;
}
.shore > * { position: absolute; left: 0; }
.shore__foam, .shore__edge { width: 200%; height: 30px; animation: sway 18s linear infinite; }
.shore__foam { top: -31px; animation-direction: reverse; animation-duration: 24s; }
.shore__foam svg { fill: rgba(220, 242, 248, 0.38); }
.shore__edge { top: -26px; }
.shore__edge svg { fill: #2d241b; }
.shore__sand {
  inset: 0;
  background: linear-gradient(to bottom, #2d241b 0%, #1a140f 55%, #0d0a07 100%);
}
@keyframes tide {
  0%, 100% { transform: scaleY(1); }
  50% { transform: scaleY(1.12); }
}

/* ---- Burbujas ---- */
.bubble {
  bottom: -80px;
  border-radius: 50%;
  border: 1px solid rgba(190, 240, 255, 0.35);
  background: radial-gradient(
    circle at 30% 28%,
    rgba(255, 255, 255, 0.55) 0,
    rgba(255, 255, 255, 0.08) 22%,
    rgba(120, 210, 240, 0.06) 60%,
    rgba(255, 255, 255, 0.14) 100%
  );
  box-shadow: inset 0 0 12px rgba(150, 230, 255, 0.18), 0 0 14px rgba(120, 210, 240, 0.12);
  animation: rise var(--dur) linear var(--delay) infinite;
}
@keyframes rise {
  0% { transform: translate3d(0, 0, 0); opacity: 0; }
  8% { opacity: 0.9; }
  50% { transform: translate3d(var(--sway), -55vh, 0); }
  92% { opacity: 0.9; }
  100% { transform: translate3d(calc(var(--sway) * -1), -112vh, 0); opacity: 0; }
}

/* ---- Lluvia, relámpago y viñeta ---- */
.rain { inset: 0; width: 100%; height: 100%; }
.flash {
  inset: 0;
  opacity: 0;
  mix-blend-mode: screen;
  background: radial-gradient(ellipse 70% 55% at 35% 0, rgba(205, 228, 255, 0.65), rgba(150, 190, 230, 0.18) 55%, transparent 80%);
}
.flash.on { animation: flash 0.95s ease-out; }
@keyframes flash {
  0% { opacity: 0; }
  8% { opacity: 0.9; }
  16% { opacity: 0.1; }
  26% { opacity: 0.75; }
  100% { opacity: 0; }
}
.vignette {
  inset: 0;
  background: radial-gradient(ellipse at 50% 45%, transparent 35%, rgba(0, 0, 0, 0.6) 100%);
}

@media (prefers-reduced-motion: reduce) {
  .storm *, .storm *::before { animation: none !important; }
  .bubble { display: none; }
}
</style>
