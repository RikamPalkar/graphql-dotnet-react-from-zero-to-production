namespace LibraryApi.Models;

public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Biography { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
