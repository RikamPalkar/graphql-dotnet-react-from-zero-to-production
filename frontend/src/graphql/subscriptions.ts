import { gql } from '@apollo/client';

export const ON_BOOK_ADDED = gql`
  subscription OnBookAdded {
    onBookAdded {
      id
      title
      description
      publishedYear
      genre
      price
      author {
        id
        name
      }
    }
  }
`;

export const ON_REVIEW_ADDED = gql`
  subscription OnReviewAdded($bookId: Int!) {
    onReviewAdded(bookId: $bookId) {
      id
      title
      content
      rating
      reviewerName
      createdAt
    }
  }
`;
