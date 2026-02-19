using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.GraphQL.Types;

public class ReviewType : ObjectType<Review>
{
    protected override void Configure(IObjectTypeDescriptor<Review> descriptor)
    {
        descriptor.Description("Represents a review for a book.");
        
        descriptor
            .Field(r => r.Id)
            .Description("The unique identifier of the review.");
            
        descriptor
            .Field(r => r.Title)
            .Description("The title of the review.");
            
        descriptor
            .Field(r => r.Content)
            .Description("The full content of the review.");
            
        descriptor
            .Field(r => r.Rating)
            .Description("The rating given (1-5).");
            
        descriptor
            .Field(r => r.ReviewerName)
            .Description("The name of the person who wrote the review.");
            
        descriptor
            .Field(r => r.CreatedAt)
            .Description("When the review was created.");
            
        descriptor
            .Field(r => r.Book)
            .Description("The book this review is for.");
    }
}
