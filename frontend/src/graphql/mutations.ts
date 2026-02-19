import { gql } from '@apollo/client';

export const ADD_AUTHOR = gql`
  mutation AddAuthor($input: AddAuthorInput!) {
    addAuthor(input: $input) {
      author {
        id
        name
        biography
        birthDate
        country
      }
    }
  }
`;

export const UPDATE_AUTHOR = gql`
  mutation UpdateAuthor($input: UpdateAuthorInput!) {
    updateAuthor(input: $input) {
      author {
        id
        name
        biography
        birthDate
        country
      }
      error
    }
  }
`;

export const DELETE_AUTHOR = gql`
  mutation DeleteAuthor($id: Int!) {
    deleteAuthor(id: $id) {
      success
      error
    }
  }
`;

export const ADD_BOOK = gql`
  mutation AddBook($input: AddBookInput!) {
    addBook(input: $input) {
      book {
        id
        title
        description
        isbn
        publishedYear
        genre
        price
        pageCount
        author {
          id
          name
        }
      }
      error
    }
  }
`;

export const UPDATE_BOOK = gql`
  mutation UpdateBook($input: UpdateBookInput!) {
    updateBook(input: $input) {
      book {
        id
        title
        description
        isbn
        publishedYear
        genre
        price
        pageCount
        isAvailable
      }
      error
    }
  }
`;

export const DELETE_BOOK = gql`
  mutation DeleteBook($id: Int!) {
    deleteBook(id: $id) {
      success
      error
    }
  }
`;

export const ADD_REVIEW = gql`
  mutation AddReview($input: AddReviewInput!) {
    addReview(input: $input) {
      review {
        id
        title
        content
        rating
        reviewerName
        createdAt
      }
      error
    }
  }
`;

export const DELETE_REVIEW = gql`
  mutation DeleteReview($id: Int!) {
    deleteReview(id: $id) {
      success
      error
    }
  }
`;

export const ADD_CATEGORY = gql`
  mutation AddCategory($input: AddCategoryInput!) {
    addCategory(input: $input) {
      category {
        id
        name
        description
      }
      error
    }
  }
`;

export const ADD_BOOK_TO_CATEGORY = gql`
  mutation AddBookToCategory($bookId: Int!, $categoryId: Int!) {
    addBookToCategory(bookId: $bookId, categoryId: $categoryId) {
      success
      error
    }
  }
`;
