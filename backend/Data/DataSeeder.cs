using LibraryApi.Models;

namespace LibraryApi.Data;

public static class DataSeeder
{
    public static void Seed(LibraryDbContext context)
    {
        if (context.Authors.Any()) return;
        
        var categories = new List<Category>
        {
            new() { Name = "Fiction", Description = "Literary works of imagination" },
            new() { Name = "Science Fiction", Description = "Fiction based on scientific discoveries" },
            new() { Name = "Fantasy", Description = "Fiction set in imaginary worlds" },
            new() { Name = "Mystery", Description = "Fiction involving crime and detective work" },
            new() { Name = "Non-Fiction", Description = "Factual works" },
            new() { Name = "Biography", Description = "Life stories of real people" },
            new() { Name = "Self-Help", Description = "Personal development books" },
            new() { Name = "History", Description = "Historical events and analysis" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();
        
        var authors = new List<Author>
        {
            new() 
            { 
                Name = "George Orwell", 
                Biography = "English novelist and essayist, best known for his allegorical novella Animal Farm and dystopian novel 1984.",
                BirthDate = new DateTime(1903, 6, 25),
                Country = "United Kingdom"
            },
            new() 
            { 
                Name = "Jane Austen", 
                Biography = "English novelist known primarily for her six major novels, which interpret, critique and comment upon the British landed gentry.",
                BirthDate = new DateTime(1775, 12, 16),
                Country = "United Kingdom"
            },
            new() 
            { 
                Name = "Isaac Asimov", 
                Biography = "American writer and professor of biochemistry, known for his works of science fiction and popular science.",
                BirthDate = new DateTime(1920, 1, 2),
                Country = "United States"
            },
            new() 
            { 
                Name = "Agatha Christie", 
                Biography = "English writer known for her detective novels and short stories, particularly those revolving around Hercule Poirot.",
                BirthDate = new DateTime(1890, 9, 15),
                Country = "United Kingdom"
            },
            new() 
            { 
                Name = "J.R.R. Tolkien", 
                Biography = "English writer, poet, philologist, and academic, best known as the author of The Hobbit and The Lord of the Rings.",
                BirthDate = new DateTime(1892, 1, 3),
                Country = "United Kingdom"
            }
        };
        context.Authors.AddRange(authors);
        context.SaveChanges();
        
        var books = new List<Book>
        {
            new() 
            { 
                Title = "1984", 
                Description = "A dystopian social science fiction novel and cautionary tale about the dangers of totalitarianism.",
                Isbn = "978-0451524935",
                PublishedYear = 1949,
                Genre = "Dystopian",
                Price = 15.99m,
                PageCount = 328,
                AuthorId = authors[0].Id
            },
            new() 
            { 
                Title = "Animal Farm", 
                Description = "An allegorical novella reflecting events leading up to the Russian Revolution.",
                Isbn = "978-0451526342",
                PublishedYear = 1945,
                Genre = "Political Satire",
                Price = 12.99m,
                PageCount = 112,
                AuthorId = authors[0].Id
            },
            new() 
            { 
                Title = "Pride and Prejudice", 
                Description = "A romantic novel following the emotional development of protagonist Elizabeth Bennet.",
                Isbn = "978-0141439518",
                PublishedYear = 1813,
                Genre = "Romance",
                Price = 10.99m,
                PageCount = 432,
                AuthorId = authors[1].Id
            },
            new() 
            { 
                Title = "Foundation", 
                Description = "A science fiction novel about the fall of a Galactic Empire and the efforts to preserve knowledge.",
                Isbn = "978-0553293357",
                PublishedYear = 1951,
                Genre = "Science Fiction",
                Price = 16.99m,
                PageCount = 244,
                AuthorId = authors[2].Id
            },
            new() 
            { 
                Title = "I, Robot", 
                Description = "A collection of science fiction short stories about robots and the Three Laws of Robotics.",
                Isbn = "978-0553294385",
                PublishedYear = 1950,
                Genre = "Science Fiction",
                Price = 14.99m,
                PageCount = 224,
                AuthorId = authors[2].Id
            },
            new() 
            { 
                Title = "Murder on the Orient Express", 
                Description = "A detective novel featuring Belgian detective Hercule Poirot.",
                Isbn = "978-0062693662",
                PublishedYear = 1934,
                Genre = "Mystery",
                Price = 15.99m,
                PageCount = 256,
                AuthorId = authors[3].Id
            },
            new() 
            { 
                Title = "The Hobbit", 
                Description = "A fantasy novel about the adventures of hobbit Bilbo Baggins.",
                Isbn = "978-0547928227",
                PublishedYear = 1937,
                Genre = "Fantasy",
                Price = 14.99m,
                PageCount = 300,
                AuthorId = authors[4].Id
            },
            new() 
            { 
                Title = "The Fellowship of the Ring", 
                Description = "The first volume of The Lord of the Rings epic fantasy novel.",
                Isbn = "978-0547928210",
                PublishedYear = 1954,
                Genre = "Fantasy",
                Price = 18.99m,
                PageCount = 423,
                AuthorId = authors[4].Id
            }
        };
        context.Books.AddRange(books);
        context.SaveChanges();
        
        var bookCategories = new List<BookCategory>
        {
            new() { BookId = books[0].Id, CategoryId = categories[0].Id },
            new() { BookId = books[0].Id, CategoryId = categories[1].Id },
            new() { BookId = books[1].Id, CategoryId = categories[0].Id },
            new() { BookId = books[2].Id, CategoryId = categories[0].Id },
            new() { BookId = books[3].Id, CategoryId = categories[1].Id },
            new() { BookId = books[4].Id, CategoryId = categories[1].Id },
            new() { BookId = books[5].Id, CategoryId = categories[3].Id },
            new() { BookId = books[6].Id, CategoryId = categories[2].Id },
            new() { BookId = books[7].Id, CategoryId = categories[2].Id }
        };
        context.BookCategories.AddRange(bookCategories);
        context.SaveChanges();
        
        var reviews = new List<Review>
        {
            new() 
            { 
                Title = "A Masterpiece of Dystopian Literature",
                Content = "1984 is a haunting and thought-provoking novel that remains relevant decades after its publication.",
                Rating = 5,
                ReviewerName = "John Smith",
                BookId = books[0].Id
            },
            new() 
            { 
                Title = "Essential Reading",
                Content = "Every person should read this book. It's a warning about the dangers of totalitarianism.",
                Rating = 5,
                ReviewerName = "Emily Johnson",
                BookId = books[0].Id
            },
            new() 
            { 
                Title = "Brilliant Allegory",
                Content = "Animal Farm uses animals to brilliantly critique political systems.",
                Rating = 4,
                ReviewerName = "Michael Brown",
                BookId = books[1].Id
            },
            new() 
            { 
                Title = "Timeless Romance",
                Content = "Pride and Prejudice is witty, romantic, and endlessly entertaining.",
                Rating = 5,
                ReviewerName = "Sarah Davis",
                BookId = books[2].Id
            },
            new() 
            { 
                Title = "Epic Sci-Fi",
                Content = "Foundation sets the standard for space opera and science fiction world-building.",
                Rating = 5,
                ReviewerName = "David Wilson",
                BookId = books[3].Id
            },
            new() 
            { 
                Title = "Fascinating Robot Stories",
                Content = "I, Robot explores the ethics of artificial intelligence in compelling ways.",
                Rating = 4,
                ReviewerName = "Lisa Anderson",
                BookId = books[4].Id
            },
            new() 
            { 
                Title = "Classic Mystery",
                Content = "Murder on the Orient Express has one of the most surprising endings in detective fiction.",
                Rating = 5,
                ReviewerName = "Robert Taylor",
                BookId = books[5].Id
            },
            new() 
            { 
                Title = "Adventure at Its Best",
                Content = "The Hobbit is a wonderful adventure story suitable for all ages.",
                Rating = 5,
                ReviewerName = "Jennifer Martinez",
                BookId = books[6].Id
            }
        };
        context.Reviews.AddRange(reviews);
        context.SaveChanges();
    }
}
