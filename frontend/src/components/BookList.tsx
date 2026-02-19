import { useQuery } from '@apollo/client';
import { GET_BOOKS } from '../graphql/queries';
import { Book, Connection } from '../types';
import { useState } from 'react';

interface BooksData {
  books: Connection<Book>;
}

export function BookList({ onSelectBook }: { onSelectBook: (id: number) => void }) {
  const [pageSize] = useState(10);
  const { loading, error, data, fetchMore } = useQuery<BooksData>(GET_BOOKS, {
    variables: { first: pageSize },
  });

  if (loading && !data) return <div className="loading">Loading books...</div>;
  if (error) return <div className="error">Error: {error.message}</div>;

  const loadMore = () => {
    if (data?.books.pageInfo.hasNextPage) {
      fetchMore({
        variables: {
          first: pageSize,
          after: data.books.pageInfo.endCursor,
        },
      });
    }
  };

  const getRatingStars = (rating?: number) => {
    if (!rating) return 'No ratings';
    return '★'.repeat(Math.round(rating)) + '☆'.repeat(5 - Math.round(rating));
  };

  return (
    <div className="book-list">
      <h2>Books ({data?.books.totalCount || 0})</h2>
      <div className="books-grid">
        {data?.books.nodes.map((book) => (
          <div
            key={book.id}
            className="book-card"
            onClick={() => onSelectBook(book.id)}
          >
            <h3>{book.title}</h3>
            <p className="author">by {book.author?.name}</p>
            <p className="genre">{book.genre}</p>
            <p className="year">{book.publishedYear}</p>
            <p className="price">${book.price.toFixed(2)}</p>
            <div className="rating">
              <span className="stars">{getRatingStars(book.averageRating)}</span>
              <span className="count">({book.reviewCount} reviews)</span>
            </div>
            <span className={`availability ${book.isAvailable ? 'available' : 'unavailable'}`}>
              {book.isAvailable ? 'Available' : 'Unavailable'}
            </span>
          </div>
        ))}
      </div>
      {data?.books.pageInfo.hasNextPage && (
        <button className="load-more" onClick={loadMore}>
          Load More
        </button>
      )}
    </div>
  );
}
