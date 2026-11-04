using System.ComponentModel.DataAnnotations;

namespace BackEnd.Modules.Auth.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    // Mínimo 8 caracteres, una mayúscula y un número (igual que en el frontend)
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,100}$",
        ErrorMessage = "La contraseña debe tener mínimo 8 caracteres, una mayúscula y un número")]
    public string Password { get; set; } = string.Empty;
}
