export interface Book {
  id: number
  title: string
  authorId: number
  authorName?: string
}

export type BookPayload = Omit<Book, 'id' | 'authorName'>
