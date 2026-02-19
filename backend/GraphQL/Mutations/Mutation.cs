using HotChocolate.Subscriptions;
using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.GraphQL.Types;
using LibraryApi.GraphQL.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.GraphQL.Mutations;

public class Mutation
{
    public async Task<AddAuthorPayload> AddAuthor(LibraryDbContext context, AddAuthorInput input)
    {
        var author = new Author
        {
            Name = input.Name,
            Biography = input.Biography,
            BirthDate = input.BirthDate,
            Country = input.Country
        };
        
        context.Authors.Add(author);
        await context.SaveChangesAsync();
        
        return new AddAuthorPayload(author);
    }
    
    public async Task<UpdateAuthorPayload> UpdateAuthor(LibraryDbContext context, UpdateAuthorInput input)
    {
        var author = await context.Authors.FindAsync(input.Id);
        
        if (author == null)
            return new UpdateAuthorPayload(null, "Author not found");
            
        if (input.Name != null) author.Name = input.Name;
        if (input.Biography != null) author.Biography = input.Biography;
        if (input.BirthDate.HasValue) author.BirthDate = input.BirthDate.Value;
        if (input.Country != null) author.Country = input.Country;
        
        await context.SaveChangesAsync();
        
        return new UpdateAuthorPayload(author, null);
    }
    
    public async Task<DeletePayload> DeleteAuthor(LibraryDbContext context, int id)
    {
        var author = await context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (author == null)
            return new DeletePayload(false, "Author not found");
            
        if (author.Books.Any())
            return new DeletePayload(false, "Cannot delete author with existing books");
            
        context.Authors.Remove(author);
        await context.SaveChangesAsync();
        
        return new DeletePayload(true, null);
    }
    
    public async Task<AddBookPayload> AddBook(
        LibraryDbContext context,
        [Service] ITopicEventSender eventSender,
        AddBookInput input)
    {
        var authorExists = await context.Authors.AnyAsync(a => a.Id == input.AuthorId);
        if (!authorExists)
            return new AddBookPayload(null, "Author not found");
            
        var book = new Book
        {
            Title = input.Title,
            Description = input.Description,
            Isbn = input.Isbn,
            PublishedYear = input.PublishedYear,
            Genre = input.Genre,
            Price = input.Price,
            PageCount = input.PageCount,
            AuthorId = input.AuthorId
        };
        
        context.Books.Add(book);
        await context.SaveChangesAsync();
        
        if (input.CategoryIds?.Any() == true)
        {
            var bookCategories = input.CategoryIds
                .Select(cid => new BookCategory { BookId = book.Id, CategoryId = cid });
            context.BookCategories.AddRange(bookCategories);
            await context.SaveChangesAsync();
        }
        
        await eventSender.SendAsync(nameof(Subscription.OnBookAdded), book);
        
        return new AddBookPayload(book, null);
    }
    
    public async Task<UpdateBookPayload> UpdateBook(LibraryDbContext context, UpdateBookInput input)
    {
        var book = await context.Books.FindAsync(input.Id);
        
        if (book == null)
            return new UpdateBookPayload(null, "Book not found");
            
        if (input.Title != null) book.Title = input.Title;
        if (input.Description != null) book.Description = input.Description;
        if (input.Isbn != null) book.Isbn = input.Isbn;
        if (input.PublishedYear.HasValue) book.PublishedYear = input.PublishedYear.Value;
        if (input.Genre != null) book.Genre = input.Genre;
        if (input.Price.HasValue) book.Price = input.Price.Value;
        if (input.PageCount.HasValue) book.PageCount = input.PageCount.Value;
        if (input.IsAvailable.HasValue) book.IsAvailable = input.IsAvailable.Value;
        
        await context.SaveChangesAsync();
        
        return new UpdateBookPayload(book, null);
    }
    
    public async Task<DeletePayload> DeleteBook(LibraryDbContext context, int id)
    {
        var book = await context.Books.FindAsync(id);
        
        if (book == null)
            return new DeletePayload(false, "Book not found");
            
        var reviews = context.Reviews.Where(r => r.BookId == id);
        context.Reviews.RemoveRange(reviews);
        
        var bookCategories = context.BookCategories.Where(bc => bc.BookId == id);
        context.BookCategories.RemoveRange(bookCategories);
        
        context.Books.Remove(book);
        await context.SaveChangesAsync();
        
        return new DeletePayload(true, null);
    }
    
    public async Task<AddReviewPayload> AddReview(
        LibraryDbContext context,
        [Service] ITopicEventSender eventSender,
        AddReviewInput input)
    {
        if (input.Rating < 1 || input.Rating > 5)
            return new AddReviewPayload(null, "Rating must be between 1 and 5");
            
        var bookExists = await context.Books.AnyAsync(b => b.Id == input.BookId);
        if (!bookExists)
            return new AddReviewPayload(null, "Book not found");
            
        var review = new Review
        {
            BookId = input.BookId,
            Title = input.Title,
            Content = input.Content,
            Rating = input.Rating,
            ReviewerName = input.ReviewerName
        };
        
        context.Reviews.Add(review);
        await context.SaveChangesAsync();
        
        await eventSender.SendAsync($"{nameof(Subscription.OnReviewAdded)}_{input.BookId}", review);
        
        return new AddReviewPayload(review, null);
    }
    
    public async Task<DeletePayload> DeleteReview(LibraryDbContext context, int id)
    {
        var review = await context.Reviews.FindAsync(id);
        
        if (review == null)
            return new DeletePayload(false, "Review not found");
            
        context.Reviews.Remove(review);
        await context.SaveChangesAsync();
        
        return new DeletePayload(true, null);
    }
    
    public async Task<AddCategoryPayload> AddCategory(LibraryDbContext context, AddCategoryInput input)
    {
        var exists = await context.Categories.AnyAsync(c => c.Name == input.Name);
        if (exists)
            return new AddCategoryPayload(null, "Category already exists");
            
        var category = new Category
        {
            Name = input.Name,
            Description = input.Description
        };
        
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        
        return new AddCategoryPayload(category, null);
    }
    
    public async Task<AddBookToCategoryPayload> AddBookToCategory(
        LibraryDbContext context,
        int bookId,
        int categoryId)
    {
        var bookExists = await context.Books.AnyAsync(b => b.Id == bookId);
        var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId);
        
        if (!bookExists)
            return new AddBookToCategoryPayload(false, "Book not found");
        if (!categoryExists)
            return new AddBookToCategoryPayload(false, "Category not found");
            
        var alreadyExists = await context.BookCategories
            .AnyAsync(bc => bc.BookId == bookId && bc.CategoryId == categoryId);
            
        if (alreadyExists)
            return new AddBookToCategoryPayload(false, "Book already in category");
            
        context.BookCategories.Add(new BookCategory 
        { 
            BookId = bookId, 
            CategoryId = categoryId 
        });
        await context.SaveChangesAsync();
        
        return new AddBookToCategoryPayload(true, null);
    }
}

public record AddAuthorPayload(Author Author);
public record UpdateAuthorPayload(Author? Author, string? Error);
public record AddBookPayload(Book? Book, string? Error);
public record UpdateBookPayload(Book? Book, string? Error);
public record AddReviewPayload(Review? Review, string? Error);
public record AddCategoryPayload(Category? Category, string? Error);
public record AddBookToCategoryPayload(bool Success, string? Error);
public record DeletePayload(bool Success, string? Error);
