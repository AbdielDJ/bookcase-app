using BackEnd.Data;
using BackEnd.Modules.Books.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Modules.Books.Repositories;

public class BookRepository(AppDbContext db) : IBookRepository
{
    public Task<List<Book>> GetAllAsync() =>
        db.Books.AsNoTracking().Include(b => b.Author).OrderBy(b => b.Title).ToListAsync();

    public Task<Book?> GetByIdAsync(int id) =>
        db.Books.AsNoTracking().Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

    public Task<Book?> FindTrackedAsync(int id) =>
        db.Books.FirstOrDefaultAsync(b => b.Id == id);

    public Task<bool> AuthorExistsAsync(int authorId) =>
        db.Authors.AnyAsync(a => a.Id == authorId);

    public async Task AddAsync(Book book)
    {
        db.Books.Add(book);
        await db.SaveChangesAsync();
    }

    public Task SaveAsync() => db.SaveChangesAsync();

    public async Task DeleteAsync(Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
    }
}
