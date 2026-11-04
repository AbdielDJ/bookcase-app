using BackEnd.Modules.Authors.Entities;

namespace BackEnd.Modules.Authors.Repositories;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task<bool> HasBooksAsync(int id);
    Task AddAsync(Author author);
    Task SaveAsync();
    Task DeleteAsync(Author author);
}
