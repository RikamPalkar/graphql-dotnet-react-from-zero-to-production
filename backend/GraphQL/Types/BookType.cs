using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.GraphQL.Types;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.Description("Represents a book in the library.");
        
        descriptor
            .Field(b => b.Id)
            .Description("The unique identifier of the book.");
            
        descriptor
            .Field(b => b.Title)
            .Description("The title of the book.");
            
        descriptor
            .Field(b => b.Description)
            .Description("A brief description or synopsis of the book.");
            
        descriptor
            .Field(b => b.Isbn)
            .Description("The International Standard Book Number.");
            
        descriptor
            .Field(b => b.PublishedYear)
            .Description("The year the book was published.");
            
        descriptor
            .Field(b => b.Genre)
            .Description("The genre of the book.");
            
        descriptor
            .Field(b => b.Price)
            .Description("The price of the book.");
            
        descriptor
            .Field(b => b.PageCount)
            .Description("The number of pages in the book.");
            
        descriptor
            .Field(b => b.IsAvailable)
            .Description("Whether the book is currently available.");
            
        descriptor
            .Field(b => b.Author)
            .Description("The author who wrote this book.");
            
        descriptor
            .Field(b => b.Reviews)
            .Description("Reviews for this book.");
            
        descriptor
            .Field("categories")
            .Type<ListType<CategoryType>>()
            .Resolve(async ctx =>
            {
                var book = ctx.Parent<Book>();
                var dbContext = ctx.Service<LibraryDbContext>();
                return await dbContext.BookCategories
                    .Where(bc => bc.BookId == book.Id)
                    .Select(bc => bc.Category!)
                    .ToListAsync();
            })
            .Description("Categories this book belongs to.");
            
        descriptor
            .Field("averageRating")
            .Type<FloatType>()
            .Resolve(async ctx =>
            {
                var book = ctx.Parent<Book>();
                var dbContext = ctx.Service<LibraryDbContext>();
                var ratings = await dbContext.Reviews
                    .Where(r => r.BookId == book.Id)
                    .Select(r => r.Rating)
                    .ToListAsync();
                return ratings.Count > 0 ? ratings.Average() : (double?)null;
            })
            .Description("The average rating of this book.");
            
        descriptor
            .Field("reviewCount")
            .Type<IntType>()
            .Resolve(async ctx =>
            {
                var book = ctx.Parent<Book>();
                var dbContext = ctx.Service<LibraryDbContext>();
                return await dbContext.Reviews.CountAsync(r => r.BookId == book.Id);
            })
            .Description("The total number of reviews for this book.");
    }
}
