using BackEnd.Modules.Books.Entities;

namespace BackEnd.Modules.Authors.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Book> Books { get; set; } = new();
}
