using BackEnd.Modules.Authors.Entities;

namespace BackEnd.Modules.Books.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}
