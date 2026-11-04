using BackEnd.Modules.Authors.Dtos;
using BackEnd.Modules.Authors.Entities;
using BackEnd.Modules.Authors.Repositories;

namespace BackEnd.Modules.Authors.Services;

public class AuthorService(IAuthorRepository repo) : IAuthorService
{
    public async Task<List<AuthorDto>> GetAllAsync()
    {
        var authors = await repo.GetAllAsync();
        return authors.Select(ToDto).ToList();
    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await repo.GetByIdAsync(id);
        return author is null ? null : ToDto(author);
    }

    public async Task<AuthorDto> CreateAsync(AuthorRequestDto dto)
    {
        var author = new Author { Name = dto.Name.Trim() };
        await repo.AddAsync(author);
        return ToDto(author);
    }

    public async Task<bool> UpdateAsync(int id, AuthorRequestDto dto)
    {
        var author = await repo.GetByIdAsync(id);
        if (author is null) return false;

        author.Name = dto.Name.Trim();
        await repo.SaveAsync();
        return true;
    }

    public async Task<DeleteResult> DeleteAsync(int id)
    {
        var author = await repo.GetByIdAsync(id);
        if (author is null) return DeleteResult.NotFound;
        if (await repo.HasBooksAsync(id)) return DeleteResult.HasBooks;

        await repo.DeleteAsync(author);
        return DeleteResult.Deleted;
    }

    private static AuthorDto ToDto(Author author) => new()
    {
        Id = author.Id,
        Name = author.Name,
    };
}
