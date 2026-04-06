<script setup lang="ts">
import { reactive, ref } from 'vue'

type Feedback = { kind: 'success' | 'error' | 'info'; text: string }

const form = reactive({
  nombre: '',
  email: '',
  telefono: '',
  nombreFinca: '',
  hectareas: '' as string,
})

const fieldErrors = reactive<Record<string, string>>({})
const submitting = ref(false)
const feedback = ref<Feedback | null>(null)

function clearFieldError(key: string) {
  delete fieldErrors[key]
}

function validateClient(): boolean {
  Object.keys(fieldErrors).forEach((k) => delete fieldErrors[k])
  let ok = true
  if (!form.nombre.trim()) {
    fieldErrors.nombre = 'Campo obligatorio'
    ok = false
  }
  if (!form.email.trim()) {
    fieldErrors.email = 'Campo obligatorio'
    ok = false
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email.trim())) {
    fieldErrors.email = 'Email no válido'
    ok = false
  }
  if (!form.telefono.trim()) {
    fieldErrors.telefono = 'Campo obligatorio'
    ok = false
  }
  if (!form.nombreFinca.trim()) {
    fieldErrors.nombreFinca = 'Campo obligatorio'
    ok = false
  }
  const h = Number(form.hectareas)
  if (form.hectareas === '' || Number.isNaN(h) || h <= 0) {
    fieldErrors.hectareas = 'Indique un valor mayor a cero'
    ok = false
  }
  return ok
}

async function enviar() {
  feedback.value = null
  if (!validateClient()) {
    feedback.value = { kind: 'error', text: 'Revise los campos marcados.' }
    return
  }

  submitting.value = true
  try {
    const res = await fetch('/api/clientes', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
      body: JSON.stringify({
        nombre: form.nombre.trim(),
        email: form.email.trim(),
        telefono: form.telefono.trim(),
        nombreFinca: form.nombreFinca.trim(),
        hectareas: Number(form.hectareas),
      }),
    })

    if (res.status === 201) {
      feedback.value = { kind: 'success', text: 'Cliente registrado correctamente.' }
      form.nombre = ''
      form.email = ''
      form.telefono = ''
      form.nombreFinca = ''
      form.hectareas = ''
      Object.keys(fieldErrors).forEach((k) => delete fieldErrors[k])
      return
    }

    if (res.status === 409) {
      const body = await res.json().catch(() => ({}))
      const msg = typeof body?.mensaje === 'string' ? body.mensaje : 'El email ya está registrado.'
      feedback.value = { kind: 'error', text: msg }
      return
    }

    if (res.status === 400) {
      const body = await res.json().catch(() => null)
      let extra = ''
      if (body?.errors && typeof body.errors === 'object') {
        const errs = body.errors as Record<string, string[]>
        extra = Object.entries(errs)
          .map(([k, v]) => `${k}: ${v.join(', ')}`)
          .join(' · ')
      }
      feedback.value = {
        kind: 'error',
        text: extra ? `Validación del servidor: ${extra}` : 'No se pudo validar el formulario en el servidor.',
      }
      return
    }

    feedback.value = { kind: 'error', text: `Error inesperado (${res.status}).` }
  } catch {
    feedback.value = {
      kind: 'error',
      text: 'No hay conexión con la API. ¿Está ejecutándose el backend en http://localhost:5131?',
    }
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <div class="page">
    <header class="hero">
      <p class="eyebrow">CRM PRUEBA FULLSTACK</p>
      <h1>Registro de clientes y parcelas</h1>
      <p class="lede">
        PoC: alta de productores (contacto, finca, hectáreas) contra la Web API .NET 8.
      </p>
    </header>

    <main class="card">
      <form class="form" @submit.prevent="enviar" novalidate>
        <div class="field">
          <label for="nombre">Nombre completo</label>
          <input
            id="nombre"
            v-model="form.nombre"
            type="text"
            autocomplete="name"
            :class="{ invalid: fieldErrors.nombre }"
            @input="clearFieldError('nombre')"
          />
          <span v-if="fieldErrors.nombre" class="hint err">{{ fieldErrors.nombre }}</span>
        </div>

        <div class="field">
          <label for="email">Email</label>
          <input
            id="email"
            v-model="form.email"
            type="email"
            autocomplete="email"
            :class="{ invalid: fieldErrors.email }"
            @input="clearFieldError('email')"
          />
          <span v-if="fieldErrors.email" class="hint err">{{ fieldErrors.email }}</span>
        </div>

        <div class="field">
          <label for="telefono">Teléfono</label>
          <input
            id="telefono"
            v-model="form.telefono"
            type="tel"
            autocomplete="tel"
            :class="{ invalid: fieldErrors.telefono }"
            @input="clearFieldError('telefono')"
          />
          <span v-if="fieldErrors.telefono" class="hint err">{{ fieldErrors.telefono }}</span>
        </div>

        <div class="field">
          <label for="finca">Nombre de la finca</label>
          <input
            id="finca"
            v-model="form.nombreFinca"
            type="text"
            :class="{ invalid: fieldErrors.nombreFinca }"
            @input="clearFieldError('nombreFinca')"
          />
          <span v-if="fieldErrors.nombreFinca" class="hint err">{{ fieldErrors.nombreFinca }}</span>
        </div>

        <div class="field">
          <label for="hectareas">Hectáreas</label>
          <input
            id="hectareas"
            v-model="form.hectareas"
            type="number"
            step="0.01"
            min="0"
            inputmode="decimal"
            :class="{ invalid: fieldErrors.hectareas }"
            @input="clearFieldError('hectareas')"
          />
          <span v-if="fieldErrors.hectareas" class="hint err">{{ fieldErrors.hectareas }}</span>
        </div>

        <p v-if="feedback" class="banner" :data-kind="feedback.kind">
          {{ feedback.text }}
        </p>

        <button type="submit" class="submit" :disabled="submitting">
          {{ submitting ? 'Enviando…' : 'Registrar cliente' }}
        </button>
      </form>
    </main>
  </div>
</template>
