using BackEnd.Modules.Auth.Dtos;
using BackEnd.Modules.Auth.Entities;
using BackEnd.Modules.Auth.Repositories;

namespace BackEnd.Modules.Auth.Services;

public class AuthService(IUserRepository users, IJwtService jwt) : IAuthService
{
    public async Task<UserDto?> RegisterAsync(RegisterDto dto)
    {
        var email = Normalize(dto.Email);
        if (await users.EmailExistsAsync(email)) return null;

        var user = new User
        {
            Name = dto.Name.Trim(),
            Email = email,
            // BCrypt genera la sal y la guarda dentro del propio hash
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        };

        await users.AddAsync(user);
        return ToDto(user);
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await users.GetByEmailAsync(Normalize(dto.Email));
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        return jwt.GenerateToken(user);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await users.GetByIdAsync(id);
        return user is null ? null : ToDto(user);
    }

    private static string Normalize(string email) => email.Trim().ToLowerInvariant();

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
    };
}
