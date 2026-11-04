using BackEnd.Modules.Books.Dtos;
using BackEnd.Modules.Books.Entities;
using BackEnd.Modules.Books.Repositories;

namespace BackEnd.Modules.Books.Services;

public class BookService(IBookRepository repo) : IBookService
{
    public async Task<List<BookDto>> GetAllAsync()
    {
        var books = await repo.GetAllAsync();
        return books.Select(ToDto).ToList();
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await repo.GetByIdAsync(id);
        return book is null ? null : ToDto(book);
    }

    public async Task<BookDto?> CreateAsync(BookRequestDto dto)
    {
        if (!await repo.AuthorExistsAsync(dto.AuthorId)) return null;

        var book = new Book { Title = dto.Title.Trim(), AuthorId = dto.AuthorId };
        await repo.AddAsync(book);

        // Se vuelve a leer para devolver también el nombre del autor
        var saved = await repo.GetByIdAsync(book.Id);
        return saved is null ? null : ToDto(saved);
    }

    public async Task<BookUpdateResult> UpdateAsync(int id, BookRequestDto dto)
    {
        var book = await repo.FindTrackedAsync(id);
        if (book is null) return BookUpdateResult.NotFound;
        if (!await repo.AuthorExistsAsync(dto.AuthorId)) return BookUpdateResult.AuthorNotFound;

        book.Title = dto.Title.Trim();
        book.AuthorId = dto.AuthorId;
        await repo.SaveAsync();
        return BookUpdateResult.Updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await repo.FindTrackedAsync(id);
        if (book is null) return false;

        await repo.DeleteAsync(book);
        return true;
    }

    private static BookDto ToDto(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        AuthorId = book.AuthorId,
        AuthorName = book.Author?.Name ?? string.Empty,
    };
}
