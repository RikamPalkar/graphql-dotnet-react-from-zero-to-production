import { useQuery } from '@apollo/client';
import { GET_AUTHORS } from '../graphql/queries';
import { Author, Connection } from '../types';

interface AuthorsData {
  authors: Connection<Author>;
}

export function AuthorList({ onSelectAuthor }: { onSelectAuthor: (id: number) => void }) {
  const { loading, error, data, fetchMore } = useQuery<AuthorsData>(GET_AUTHORS, {
    variables: { first: 10 },
  });

  if (loading && !data) return <div className="loading">Loading authors...</div>;
  if (error) return <div className="error">Error: {error.message}</div>;

  const loadMore = () => {
    if (data?.authors.pageInfo.hasNextPage) {
      fetchMore({
        variables: {
          first: 10,
          after: data.authors.pageInfo.endCursor,
        },
      });
    }
  };

  return (
    <div className="author-list">
      <h2>Authors ({data?.authors.totalCount || 0})</h2>
      <div className="authors-grid">
        {data?.authors.nodes.map((author) => (
          <div
            key={author.id}
            className="author-card"
            onClick={() => onSelectAuthor(author.id)}
          >
            <h3>{author.name}</h3>
            <p className="country">{author.country}</p>
            <p className="bio">{author.biography?.substring(0, 100)}...</p>
            <div className="stats">
              <span>{author.bookCount} books</span>
              {author.averageBookRating && (
                <span>Avg: {author.averageBookRating.toFixed(1)} ★</span>
              )}
            </div>
          </div>
        ))}
      </div>
      {data?.authors.pageInfo.hasNextPage && (
        <button className="load-more" onClick={loadMore}>
          Load More
        </button>
      )}
    </div>
  );
}
