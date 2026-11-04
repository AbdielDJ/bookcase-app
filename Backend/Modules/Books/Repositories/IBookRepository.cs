using BackEnd.Modules.Books.Entities;

namespace BackEnd.Modules.Books.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);          // lectura (incluye el autor)
    Task<Book?> FindTrackedAsync(int id);      // para editar o borrar
    Task<bool> AuthorExistsAsync(int authorId);
    Task AddAsync(Book book);
    Task SaveAsync();
    Task DeleteAsync(Book book);
}
