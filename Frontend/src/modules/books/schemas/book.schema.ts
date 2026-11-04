import { z } from 'zod'

export const bookSchema = z.object({
  title: z.string().trim().min(1, 'El título es obligatorio').max(150, 'Máximo 150 caracteres'),
  authorId: z
    .number({ required_error: 'Selecciona un autor', invalid_type_error: 'Selecciona un autor' })
    .int()
    .positive('Selecciona un autor'),
})
