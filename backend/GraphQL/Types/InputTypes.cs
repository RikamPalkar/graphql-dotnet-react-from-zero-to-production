namespace LibraryApi.GraphQL.Types;

public record AddAuthorInput(
    string Name,
    string? Biography,
    DateTime BirthDate,
    string? Country
);

public record UpdateAuthorInput(
    int Id,
    string? Name,
    string? Biography,
    DateTime? BirthDate,
    string? Country
);

public record AddBookInput(
    string Title,
    string? Description,
    string? Isbn,
    int PublishedYear,
    string? Genre,
    decimal Price,
    int PageCount,
    int AuthorId,
    List<int>? CategoryIds
);

public record UpdateBookInput(
    int Id,
    string? Title,
    string? Description,
    string? Isbn,
    int? PublishedYear,
    string? Genre,
    decimal? Price,
    int? PageCount,
    bool? IsAvailable
);

public record AddReviewInput(
    int BookId,
    string Title,
    string Content,
    int Rating,
    string ReviewerName
);

public record AddCategoryInput(
    string Name,
    string? Description
);

public record BookFilterInput(
    string? Title,
    string? Genre,
    int? MinYear,
    int? MaxYear,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsAvailable,
    int? AuthorId
);

public record BookSortInput(
    BookSortField Field,
    SortDirection Direction = SortDirection.Asc
);

public enum BookSortField
{
    Title,
    PublishedYear,
    Price,
    PageCount,
    CreatedAt
}

public enum SortDirection
{
    Asc,
    Desc
}
