namespace LibraryApi.Models;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Isbn { get; set; }
    public int PublishedYear { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
    public int PageCount { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
    
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}
