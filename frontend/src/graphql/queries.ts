import { gql } from '@apollo/client';

export const BOOK_FRAGMENT = gql`
  fragment BookDetails on Book {
    id
    title
    description
    isbn
    publishedYear
    genre
    price
    pageCount
    isAvailable
    createdAt
  }
`;

export const AUTHOR_FRAGMENT = gql`
  fragment AuthorDetails on Author {
    id
    name
    biography
    birthDate
    country
  }
`;

export const REVIEW_FRAGMENT = gql`
  fragment ReviewDetails on Review {
    id
    title
    content
    rating
    reviewerName
    createdAt
  }
`;

export const GET_BOOKS = gql`
  ${BOOK_FRAGMENT}
  query GetBooks($first: Int, $after: String) {
    books(first: $first, after: $after) {
      pageInfo {
        hasNextPage
        hasPreviousPage
        startCursor
        endCursor
      }
      totalCount
      nodes {
        ...BookDetails
        author {
          id
          name
        }
        averageRating
        reviewCount
      }
    }
  }
`;

export const GET_BOOK_BY_ID = gql`
  ${BOOK_FRAGMENT}
  ${REVIEW_FRAGMENT}
  query GetBookById($id: Int!) {
    bookById(id: $id) {
      ...BookDetails
      author {
        id
        name
        biography
        country
      }
      reviews {
        ...ReviewDetails
      }
      categories {
        id
        name
      }
      averageRating
      reviewCount
    }
  }
`;

export const SEARCH_BOOKS = gql`
  ${BOOK_FRAGMENT}
  query SearchBooks($searchTerm: String!) {
    searchBooks(searchTerm: $searchTerm) {
      ...BookDetails
      author {
        id
        name
      }
      averageRating
    }
  }
`;

export const GET_FILTERED_BOOKS = gql`
  ${BOOK_FRAGMENT}
  query GetFilteredBooks(
    $filter: BookFilterInput
    $sort: BookSortInput
    $skip: Int
    $take: Int
  ) {
    filteredBooks(filter: $filter, sort: $sort, skip: $skip, take: $take) {
      ...BookDetails
      author {
        id
        name
      }
      averageRating
    }
  }
`;

export const GET_AUTHORS = gql`
  ${AUTHOR_FRAGMENT}
  query GetAuthors($first: Int, $after: String) {
    authors(first: $first, after: $after) {
      pageInfo {
        hasNextPage
        endCursor
      }
      totalCount
      nodes {
        ...AuthorDetails
        bookCount
        averageBookRating
      }
    }
  }
`;

export const GET_AUTHOR_BY_ID = gql`
  ${AUTHOR_FRAGMENT}
  ${BOOK_FRAGMENT}
  query GetAuthorById($id: Int!) {
    authorById(id: $id) {
      ...AuthorDetails
      books {
        ...BookDetails
        averageRating
      }
      bookCount
      averageBookRating
    }
  }
`;

export const GET_CATEGORIES = gql`
  query GetCategories {
    categories {
      id
      name
      description
      bookCount
    }
  }
`;

export const GET_STATISTICS = gql`
  query GetStatistics {
    statistics {
      totalBooks
      totalAuthors
      totalReviews
      totalCategories
      averageBookRating
      availableBooks
    }
  }
`;

export const GET_REVIEWS_BY_BOOK = gql`
  ${REVIEW_FRAGMENT}
  query GetReviewsByBook($bookId: Int!) {
    reviewsByBookId(bookId: $bookId) {
      ...ReviewDetails
    }
  }
`;
