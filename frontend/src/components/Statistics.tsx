import { useQuery } from '@apollo/client';
import { GET_STATISTICS } from '../graphql/queries';
import { LibraryStatistics } from '../types';

interface StatsData {
  statistics: LibraryStatistics;
}

export function Statistics() {
  const { loading, error, data } = useQuery<StatsData>(GET_STATISTICS);

  if (loading) return <div className="loading">Loading statistics...</div>;
  if (error) return <div className="error">Error: {error.message}</div>;

  const stats = data?.statistics;

  return (
    <div className="statistics">
      <h2>Library Statistics</h2>
      <div className="stats-grid">
        <div className="stat-card">
          <span className="stat-value">{stats?.totalBooks}</span>
          <span className="stat-label">Total Books</span>
        </div>
        <div className="stat-card">
          <span className="stat-value">{stats?.availableBooks}</span>
          <span className="stat-label">Available Books</span>
        </div>
        <div className="stat-card">
          <span className="stat-value">{stats?.totalAuthors}</span>
          <span className="stat-label">Authors</span>
        </div>
        <div className="stat-card">
          <span className="stat-value">{stats?.totalReviews}</span>
          <span className="stat-label">Reviews</span>
        </div>
        <div className="stat-card">
          <span className="stat-value">{stats?.totalCategories}</span>
          <span className="stat-label">Categories</span>
        </div>
        <div className="stat-card">
          <span className="stat-value">{stats?.averageBookRating?.toFixed(1)} ★</span>
          <span className="stat-label">Avg Rating</span>
        </div>
      </div>
    </div>
  );
}
