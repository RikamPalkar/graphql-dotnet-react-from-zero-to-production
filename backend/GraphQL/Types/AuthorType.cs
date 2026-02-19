using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.GraphQL.Types;

public class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        descriptor.Description("Represents an author who writes books.");
        
        descriptor
            .Field(a => a.Id)
            .Description("The unique identifier of the author.");
            
        descriptor
            .Field(a => a.Name)
            .Description("The full name of the author.");
            
        descriptor
            .Field(a => a.Biography)
            .Description("A brief biography of the author.");
            
        descriptor
            .Field(a => a.BirthDate)
            .Description("The birth date of the author.");
            
        descriptor
            .Field(a => a.Country)
            .Description("The country of origin of the author.");
            
        descriptor
            .Field(a => a.Books)
            .Description("The list of books written by this author.");
            
        descriptor
            .Field("bookCount")
            .Type<IntType>()
            .Resolve(async ctx =>
            {
                var author = ctx.Parent<Author>();
                var dbContext = ctx.Service<LibraryDbContext>();
                return await dbContext.Books.CountAsync(b => b.AuthorId == author.Id);
            })
            .Description("The total number of books written by this author.");
            
        descriptor
            .Field("averageBookRating")
            .Type<FloatType>()
            .Resolve(async ctx =>
            {
                var author = ctx.Parent<Author>();
                var dbContext = ctx.Service<LibraryDbContext>();
                var ratings = await dbContext.Reviews
                    .Where(r => r.Book!.AuthorId == author.Id)
                    .Select(r => r.Rating)
                    .ToListAsync();
                return ratings.Count > 0 ? ratings.Average() : (double?)null;
            })
            .Description("The average rating across all books by this author.");
    }
}
