import { useLazyQuery } from '@apollo/client';
import { SEARCH_BOOKS } from '../graphql/queries';
import { Book } from '../types';
import { useState } from 'react';

interface SearchData {
  searchBooks: Book[];
}

export function SearchBooks({ onSelectBook }: { onSelectBook: (id: number) => void }) {
  const [searchTerm, setSearchTerm] = useState('');
  const [search, { loading, error, data }] = useLazyQuery<SearchData>(SEARCH_BOOKS);

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    if (searchTerm.trim()) {
      search({ variables: { searchTerm } });
    }
  };

  return (
    <div className="search-books">
      <h2>Search Books</h2>
      <form onSubmit={handleSearch} className="search-form">
        <input
          type="text"
          placeholder="Search by title, description, or genre..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <button type="submit" disabled={loading}>
          {loading ? 'Searching...' : 'Search'}
        </button>
      </form>

      {error && <div className="error">Error: {error.message}</div>}

      {data && (
        <div className="search-results">
          <p>{data.searchBooks.length} results found</p>
          <div className="books-grid">
            {data.searchBooks.map((book) => (
              <div
                key={book.id}
                className="book-card"
                onClick={() => onSelectBook(book.id)}
              >
                <h3>{book.title}</h3>
                <p className="author">by {book.author?.name}</p>
                <p className="genre">{book.genre}</p>
                <p className="year">{book.publishedYear}</p>
                {book.averageRating && (
                  <p className="rating">{book.averageRating.toFixed(1)} ★</p>
                )}
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
