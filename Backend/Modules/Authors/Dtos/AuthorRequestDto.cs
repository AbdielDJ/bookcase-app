using System.ComponentModel.DataAnnotations;

namespace BackEnd.Modules.Authors.Dtos;

// Se usa para crear y para editar
public class AuthorRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string Name { get; set; } = string.Empty;
}
