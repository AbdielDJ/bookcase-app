using System.ComponentModel.DataAnnotations;

namespace BackEnd.Modules.Books.Dtos;

// Se usa para crear y para editar
public class BookRequestDto
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "El título no puede pasar de 150 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un autor")]
    public int AuthorId { get; set; }
}
