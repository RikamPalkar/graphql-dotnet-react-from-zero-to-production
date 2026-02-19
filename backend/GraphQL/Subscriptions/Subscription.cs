using LibraryApi.Models;

namespace LibraryApi.GraphQL.Subscriptions;

public class Subscription
{
    [Subscribe]
    [Topic]
    public Book OnBookAdded([EventMessage] Book book) => book;
    
    [Subscribe]
    [Topic($"{nameof(OnReviewAdded)}_{{bookId}}")]
    public Review OnReviewAdded(int bookId, [EventMessage] Review review) => review;
}
