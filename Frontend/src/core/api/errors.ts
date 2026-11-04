import axios from 'axios'

/** Saca un mensaje legible de un error de axios (el `message` que manda el backend, o el primer error de validación). */
export function getErrorMessage(error: unknown, fallback: string): string {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data as
      | { message?: string; errors?: Record<string, string[]> }
      | undefined

    if (data?.message) return data.message
    if (data?.errors) {
      const first = Object.values(data.errors)[0]?.[0]
      if (first) return first
    }
  }
  return fallback
}
