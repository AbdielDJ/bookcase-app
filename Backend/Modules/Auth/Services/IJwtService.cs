using BackEnd.Modules.Auth.Entities;

namespace BackEnd.Modules.Auth.Services;

public interface IJwtService
{
    int ExpiresMinutes { get; }
    string GenerateToken(User user);
}
