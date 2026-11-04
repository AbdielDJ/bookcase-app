import axios from 'axios'

export const http = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'https://localhost:7000/api',
  withCredentials: true, // necesario para enviar la cookie HttpOnly con el JWT
  headers: { 'Content-Type': 'application/json' },
})
