namespace LibraryApi.Models;

public class Review
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public int Rating { get; set; }
    public required string ReviewerName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int BookId { get; set; }
    public Book? Book { get; set; }
}
