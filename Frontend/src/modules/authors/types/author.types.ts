export interface Author {
  id: number
  name: string
}

export type AuthorPayload = Omit<Author, 'id'>
