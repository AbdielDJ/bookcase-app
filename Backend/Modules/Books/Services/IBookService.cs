using BackEnd.Modules.Books.Dtos;

namespace BackEnd.Modules.Books.Services;

public enum BookUpdateResult
{
    Updated,
    NotFound,
    AuthorNotFound,
}

public interface IBookService
{
    Task<List<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(int id);

    /// <summary>Devuelve null si el autor indicado no existe.</summary>
    Task<BookDto?> CreateAsync(BookRequestDto dto);

    Task<BookUpdateResult> UpdateAsync(int id, BookRequestDto dto);
    Task<bool> DeleteAsync(int id);
}
