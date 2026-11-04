<script setup lang="ts">
import StormBeach from '../components/StormBeach.vue'

defineProps<{
  title: string
  subtitle?: string
}>()
</script>

<template>
  <div class="auth-root">
    <StormBeach />
    <main class="auth-stage">
      <section class="auth-card" aria-labelledby="auth-title">
        <header>
          <h1 id="auth-title" class="auth-title">{{ title }}</h1>
          <p v-if="subtitle" class="auth-sub">{{ subtitle }}</p>
        </header>

        <slot />

        <footer v-if="$slots.footer" class="auth-foot">
          <slot name="footer" />
        </footer>
      </section>
    </main>
  </div>
</template>

<style>
/* Estilos globales con prefijo auth- para que apliquen al contenido de los slots */
@property --rim {
  syntax: '<angle>';
  inherits: false;
  initial-value: 0deg;
}

.auth-stage {
  position: relative;
  z-index: 1;
  min-height: 100dvh;
  display: grid;
  place-items: center;
  padding: 32px 16px;
  font-family: 'Instrument Sans', system-ui, -apple-system, 'Segoe UI', sans-serif;
}

/* La burbuja: un cristal con borde irisado que "respira" */
.auth-card {
  position: relative;
  width: min(100%, 430px);
  padding: clamp(28px, 6vw, 44px) clamp(24px, 6vw, 40px) clamp(26px, 5vw, 36px);
  color: #f2fbff;
  background: linear-gradient(145deg, rgba(255, 255, 255, 0.15), rgba(255, 255, 255, 0.04));
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 50px 54px 46px 56px / 54px 46px 56px 50px;
  -webkit-backdrop-filter: blur(18px) saturate(140%);
  backdrop-filter: blur(18px) saturate(140%);
  box-shadow:
    inset 0 1px 0 rgba(255, 255, 255, 0.35),
    inset 0 -24px 48px rgba(127, 227, 255, 0.07),
    0 30px 80px rgba(0, 0, 0, 0.55);
  animation:
    auth-rise 1.1s cubic-bezier(0.2, 0.8, 0.2, 1) both,
    auth-breathe 14s ease-in-out 1.1s infinite;
}
/* borde irisado que gira */
.auth-card::before {
  content: '';
  position: absolute;
  inset: 0;
  padding: 1.5px;
  border-radius: inherit;
  background: conic-gradient(
    from var(--rim),
    rgba(127, 227, 255, 0),
    rgba(127, 227, 255, 0.8),
    rgba(255, 160, 230, 0.55),
    rgba(255, 236, 160, 0.5),
    rgba(127, 227, 255, 0) 70%
  );
  -webkit-mask: linear-gradient(#000 0 0) content-box, linear-gradient(#000 0 0);
  -webkit-mask-composite: xor;
  mask: linear-gradient(#000 0 0) content-box exclude, linear-gradient(#000 0 0);
  pointer-events: none;
  animation: auth-rim 9s linear infinite;
}
/* destello de burbuja arriba a la izquierda */
.auth-card::after {
  content: '';
  position: absolute;
  top: 16px;
  left: 26px;
  width: 70px;
  height: 22px;
  border-radius: 50%;
  background: linear-gradient(rgba(255, 255, 255, 0.55), rgba(255, 255, 255, 0));
  transform: rotate(-24deg);
  filter: blur(1px);
  opacity: 0.7;
  pointer-events: none;
}

@keyframes auth-rise {
  from { opacity: 0; transform: translateY(46px) scale(0.92); }
  to { opacity: 1; transform: none; }
}
@keyframes auth-breathe {
  0%, 100% { border-radius: 50px 54px 46px 56px / 54px 46px 56px 50px; }
  50% { border-radius: 56px 46px 54px 48px / 46px 56px 48px 54px; }
}
@keyframes auth-rim {
  to { --rim: 360deg; }
}

.auth-title {
  margin: 0;
  font-family: 'Fraunces', Georgia, 'Times New Roman', serif;
  font-size: clamp(1.9rem, 5vw, 2.4rem);
  font-weight: 560;
  line-height: 1.1;
  letter-spacing: -0.01em;
  color: #f2fbff;
  text-shadow: 0 2px 22px rgba(127, 227, 255, 0.35);
}
.auth-sub {
  margin: 0.6rem 0 0;
  max-width: 34ch;
  font-size: 0.97rem;
  line-height: 1.5;
  color: rgba(214, 234, 242, 0.75);
}

.auth-form {
  display: grid;
  gap: 1.1rem;
  margin-top: 1.7rem;
}
.auth-alert {
  margin: 0;
  padding: 0.7rem 1rem;
  border-radius: 18px;
  font-size: 0.9rem;
  line-height: 1.4;
  color: #ffd3dc;
  background: rgba(255, 120, 150, 0.14);
  border: 1px solid rgba(255, 150, 175, 0.35);
}

.auth-foot {
  margin-top: 1.5rem;
  text-align: center;
  font-size: 0.93rem;
  color: rgba(214, 234, 242, 0.78);
}
.auth-foot a {
  color: #8fe9ff;
  text-decoration: none;
  border-bottom: 1px solid rgba(143, 233, 255, 0.4);
  transition: color 0.2s, border-color 0.2s;
}
.auth-foot a:hover { color: #d2f8ff; border-color: rgba(210, 248, 255, 0.7); }
.auth-foot a:focus-visible { outline: 2px solid #8fe9ff; outline-offset: 3px; border-radius: 4px; }

@supports not ((backdrop-filter: blur(1px)) or (-webkit-backdrop-filter: blur(1px))) {
  .auth-card { background: rgba(10, 30, 42, 0.86); }
}

@media (prefers-reduced-motion: reduce) {
  .auth-card, .auth-card::before { animation: none; }
}
</style>
