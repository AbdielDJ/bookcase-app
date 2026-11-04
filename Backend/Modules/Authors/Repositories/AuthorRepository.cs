using BackEnd.Data;
using BackEnd.Modules.Authors.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Modules.Authors.Repositories;

public class AuthorRepository(AppDbContext db) : IAuthorRepository
{
    public Task<List<Author>> GetAllAsync() =>
        db.Authors.AsNoTracking().OrderBy(a => a.Name).ToListAsync();

    // Con seguimiento: sirve tanto para leer como para editar o borrar
    public Task<Author?> GetByIdAsync(int id) =>
        db.Authors.FirstOrDefaultAsync(a => a.Id == id);

    public Task<bool> HasBooksAsync(int id) =>
        db.Books.AnyAsync(b => b.AuthorId == id);

    public async Task AddAsync(Author author)
    {
        db.Authors.Add(author);
        await db.SaveChangesAsync();
    }

    public Task SaveAsync() => db.SaveChangesAsync();

    public async Task DeleteAsync(Author author)
    {
        db.Authors.Remove(author);
        await db.SaveChangesAsync();
    }
}
