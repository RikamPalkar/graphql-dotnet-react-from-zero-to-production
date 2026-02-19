using HotChocolate.Data;
using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.GraphQL.Queries;

public class Query
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Book> GetBooks(LibraryDbContext context)
    {
        return context.Books.AsNoTracking();
    }
    
    public async Task<Book?> GetBookById(LibraryDbContext context, int id)
    {
        return await context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }
    
    public async Task<Book?> GetBookByIsbn(LibraryDbContext context, string isbn)
    {
        return await context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Isbn == isbn);
    }
    
    public async Task<IEnumerable<Book>> SearchBooks(LibraryDbContext context, string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await context.Books
            .AsNoTracking()
            .Where(b => 
                b.Title.ToLower().Contains(term) ||
                (b.Description != null && b.Description.ToLower().Contains(term)) ||
                (b.Genre != null && b.Genre.ToLower().Contains(term)))
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Book>> GetFilteredBooks(
        LibraryDbContext context,
        BookFilterInput? filter,
        BookSortInput? sort,
        int? skip,
        int? take)
    {
        var query = context.Books.AsNoTracking();
        
        if (filter != null)
        {
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(b => b.Title.Contains(filter.Title));
            if (!string.IsNullOrEmpty(filter.Genre))
                query = query.Where(b => b.Genre == filter.Genre);
            if (filter.MinYear.HasValue)
                query = query.Where(b => b.PublishedYear >= filter.MinYear);
            if (filter.MaxYear.HasValue)
                query = query.Where(b => b.PublishedYear <= filter.MaxYear);
            if (filter.MinPrice.HasValue)
                query = query.Where(b => b.Price >= filter.MinPrice);
            if (filter.MaxPrice.HasValue)
                query = query.Where(b => b.Price <= filter.MaxPrice);
            if (filter.IsAvailable.HasValue)
                query = query.Where(b => b.IsAvailable == filter.IsAvailable);
            if (filter.AuthorId.HasValue)
                query = query.Where(b => b.AuthorId == filter.AuthorId);
        }
        
        if (sort != null)
        {
            query = sort.Field switch
            {
                BookSortField.Title => sort.Direction == SortDirection.Asc 
                    ? query.OrderBy(b => b.Title) 
                    : query.OrderByDescending(b => b.Title),
                BookSortField.PublishedYear => sort.Direction == SortDirection.Asc 
                    ? query.OrderBy(b => b.PublishedYear) 
                    : query.OrderByDescending(b => b.PublishedYear),
                BookSortField.Price => sort.Direction == SortDirection.Asc 
                    ? query.OrderBy(b => b.Price) 
                    : query.OrderByDescending(b => b.Price),
                BookSortField.PageCount => sort.Direction == SortDirection.Asc 
                    ? query.OrderBy(b => b.PageCount) 
                    : query.OrderByDescending(b => b.PageCount),
                BookSortField.CreatedAt => sort.Direction == SortDirection.Asc 
                    ? query.OrderBy(b => b.CreatedAt) 
                    : query.OrderByDescending(b => b.CreatedAt),
                _ => query
            };
        }
        
        if (skip.HasValue)
            query = query.Skip(skip.Value);
        if (take.HasValue)
            query = query.Take(take.Value);
            
        return await query.ToListAsync();
    }
    
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Author> GetAuthors(LibraryDbContext context)
    {
        return context.Authors.AsNoTracking();
    }
    
    public async Task<Author?> GetAuthorById(LibraryDbContext context, int id)
    {
        return await context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Review> GetReviews(LibraryDbContext context)
    {
        return context.Reviews.AsNoTracking();
    }
    
    public async Task<IEnumerable<Review>> GetReviewsByBookId(LibraryDbContext context, int bookId)
    {
        return await context.Reviews
            .AsNoTracking()
            .Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
    
    [UseFiltering]
    [UseSorting]
    public IQueryable<Category> GetCategories(LibraryDbContext context)
    {
        return context.Categories.AsNoTracking();
    }
    
    public async Task<Category?> GetCategoryById(LibraryDbContext context, int id)
    {
        return await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<LibraryStatistics> GetStatistics(LibraryDbContext context)
    {
        return new LibraryStatistics
        {
            TotalBooks = await context.Books.CountAsync(),
            TotalAuthors = await context.Authors.CountAsync(),
            TotalReviews = await context.Reviews.CountAsync(),
            TotalCategories = await context.Categories.CountAsync(),
            AverageBookRating = await context.Reviews.AnyAsync() 
                ? await context.Reviews.AverageAsync(r => r.Rating) 
                : 0,
            AvailableBooks = await context.Books.CountAsync(b => b.IsAvailable)
        };
    }
}

public class LibraryStatistics
{
    public int TotalBooks { get; set; }
    public int TotalAuthors { get; set; }
    public int TotalReviews { get; set; }
    public int TotalCategories { get; set; }
    public double AverageBookRating { get; set; }
    public int AvailableBooks { get; set; }
}
