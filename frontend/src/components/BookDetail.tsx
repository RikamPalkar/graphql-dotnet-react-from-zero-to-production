import { useQuery, useMutation, useSubscription } from '@apollo/client';
import { GET_BOOK_BY_ID, GET_REVIEWS_BY_BOOK } from '../graphql/queries';
import { ADD_REVIEW } from '../graphql/mutations';
import { ON_REVIEW_ADDED } from '../graphql/subscriptions';
import { Book, Review } from '../types';
import { useState, useEffect } from 'react';

interface BookData {
  bookById: Book;
}

export function BookDetail({ bookId, onBack }: { bookId: number; onBack: () => void }) {
  const [reviewForm, setReviewForm] = useState({
    title: '',
    content: '',
    rating: 5,
    reviewerName: '',
  });
  const [reviews, setReviews] = useState<Review[]>([]);

  const { loading, error, data } = useQuery<BookData>(GET_BOOK_BY_ID, {
    variables: { id: bookId },
  });

  const [addReview, { loading: addingReview }] = useMutation(ADD_REVIEW, {
    refetchQueries: [{ query: GET_BOOK_BY_ID, variables: { id: bookId } }],
  });

  const { data: subscriptionData } = useSubscription(ON_REVIEW_ADDED, {
    variables: { bookId },
  });

  useEffect(() => {
    if (data?.bookById?.reviews) {
      setReviews(data.bookById.reviews);
    }
  }, [data]);

  useEffect(() => {
    if (subscriptionData?.onReviewAdded) {
      setReviews((prev) => {
        const exists = prev.some((r) => r.id === subscriptionData.onReviewAdded.id);
        if (exists) return prev;
        return [subscriptionData.onReviewAdded, ...prev];
      });
    }
  }, [subscriptionData]);

  if (loading) return <div className="loading">Loading book details...</div>;
  if (error) return <div className="error">Error: {error.message}</div>;
  if (!data?.bookById) return <div className="error">Book not found</div>;

  const book = data.bookById;

  const handleSubmitReview = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addReview({
        variables: {
          input: {
            bookId,
            ...reviewForm,
          },
        },
      });
      setReviewForm({ title: '', content: '', rating: 5, reviewerName: '' });
    } catch (err) {
      console.error('Error adding review:', err);
    }
  };

  const getRatingStars = (rating: number) => {
    return '★'.repeat(rating) + '☆'.repeat(5 - rating);
  };

  return (
    <div className="book-detail">
      <button className="back-btn" onClick={onBack}>
        ← Back to Books
      </button>

      <div className="book-info">
        <h1>{book.title}</h1>
        <p className="author-link">by {book.author?.name}</p>

        <div className="book-meta">
          <span className="genre">{book.genre}</span>
          <span className="year">{book.publishedYear}</span>
          <span className="pages">{book.pageCount} pages</span>
          <span className="price">${book.price.toFixed(2)}</span>
          <span className={`availability ${book.isAvailable ? 'available' : 'unavailable'}`}>
            {book.isAvailable ? 'Available' : 'Unavailable'}
          </span>
        </div>

        <p className="isbn">ISBN: {book.isbn}</p>
        <p className="description">{book.description}</p>

        <div className="categories">
          <strong>Categories: </strong>
          {book.categories?.map((cat) => (
            <span key={cat.id} className="category-tag">
              {cat.name}
            </span>
          ))}
        </div>

        <div className="rating-summary">
          <span className="stars">{getRatingStars(Math.round(book.averageRating || 0))}</span>
          <span className="average">{book.averageRating?.toFixed(1) || 'N/A'}</span>
          <span className="count">({book.reviewCount} reviews)</span>
        </div>
      </div>

      <div className="author-info">
        <h2>About the Author</h2>
        <p><strong>{book.author?.name}</strong></p>
        <p>{book.author?.biography}</p>
        {book.author?.country && <p>Country: {book.author.country}</p>}
      </div>

      <div className="reviews-section">
        <h2>Reviews</h2>

        <form className="review-form" onSubmit={handleSubmitReview}>
          <h3>Write a Review</h3>
          <input
            type="text"
            placeholder="Your name"
            value={reviewForm.reviewerName}
            onChange={(e) => setReviewForm({ ...reviewForm, reviewerName: e.target.value })}
            required
          />
          <input
            type="text"
            placeholder="Review title"
            value={reviewForm.title}
            onChange={(e) => setReviewForm({ ...reviewForm, title: e.target.value })}
            required
          />
          <textarea
            placeholder="Your review"
            value={reviewForm.content}
            onChange={(e) => setReviewForm({ ...reviewForm, content: e.target.value })}
            required
          />
          <div className="rating-input">
            <label>Rating: </label>
            {[1, 2, 3, 4, 5].map((r) => (
              <button
                key={r}
                type="button"
                className={reviewForm.rating >= r ? 'active' : ''}
                onClick={() => setReviewForm({ ...reviewForm, rating: r })}
              >
                ★
              </button>
            ))}
          </div>
          <button type="submit" disabled={addingReview}>
            {addingReview ? 'Submitting...' : 'Submit Review'}
          </button>
        </form>

        <div className="reviews-list">
          {reviews.map((review) => (
            <div key={review.id} className="review-card">
              <div className="review-header">
                <span className="reviewer">{review.reviewerName}</span>
                <span className="review-rating">{getRatingStars(review.rating)}</span>
              </div>
              <h4>{review.title}</h4>
              <p>{review.content}</p>
              <span className="review-date">
                {new Date(review.createdAt).toLocaleDateString()}
              </span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
