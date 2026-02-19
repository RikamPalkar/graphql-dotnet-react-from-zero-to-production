using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.GraphQL.Types;

public class CategoryType : ObjectType<Category>
{
    protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
    {
        descriptor.Description("Represents a book category.");
        
        descriptor
            .Field(c => c.Id)
            .Description("The unique identifier of the category.");
            
        descriptor
            .Field(c => c.Name)
            .Description("The name of the category.");
            
        descriptor
            .Field(c => c.Description)
            .Description("A description of what books belong to this category.");
            
        descriptor
            .Field("books")
            .Type<ListType<BookType>>()
            .Resolve(async ctx =>
            {
                var category = ctx.Parent<Category>();
                var dbContext = ctx.Service<LibraryDbContext>();
                return await dbContext.BookCategories
                    .Where(bc => bc.CategoryId == category.Id)
                    .Select(bc => bc.Book!)
                    .ToListAsync();
            })
            .Description("Books in this category.");
            
        descriptor
            .Field("bookCount")
            .Type<IntType>()
            .Resolve(async ctx =>
            {
                var category = ctx.Parent<Category>();
                var dbContext = ctx.Service<LibraryDbContext>();
                return await dbContext.BookCategories.CountAsync(bc => bc.CategoryId == category.Id);
            })
            .Description("Number of books in this category.");
    }
}
