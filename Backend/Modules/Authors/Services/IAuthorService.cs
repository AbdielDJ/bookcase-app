using BackEnd.Modules.Authors.Dtos;

namespace BackEnd.Modules.Authors.Services;

public enum DeleteResult
{
    Deleted,
    NotFound,
    HasBooks,
}

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<AuthorDto> CreateAsync(AuthorRequestDto dto);
    Task<bool> UpdateAsync(int id, AuthorRequestDto dto);
    Task<DeleteResult> DeleteAsync(int id);
}
