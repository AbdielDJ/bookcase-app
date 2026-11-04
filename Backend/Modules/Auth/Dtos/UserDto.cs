namespace BackEnd.Modules.Auth.Dtos;

// Lo que se devuelve al frontend: nunca incluye el hash de la contraseña
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
