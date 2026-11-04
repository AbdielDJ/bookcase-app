using BackEnd.Modules.Auth.Dtos;

namespace BackEnd.Modules.Auth.Services;

public interface IAuthService
{
    /// <summary>Crea el usuario. Devuelve null si el correo ya existe.</summary>
    Task<UserDto?> RegisterAsync(RegisterDto dto);

    /// <summary>Devuelve el JWT, o null si las credenciales son incorrectas.</summary>
    Task<string?> LoginAsync(LoginDto dto);

    Task<UserDto?> GetByIdAsync(int id);
}
