export interface Author {
  id: number;
  name: string;
  biography?: string;
  birthDate: string;
  country?: string;
  books?: Book[];
  bookCount?: number;
  averageBookRating?: number;
}

export interface Book {
  id: number;
  title: string;
  description?: string;
  isbn?: string;
  publishedYear: number;
  genre?: string;
  price: number;
  pageCount: number;
  isAvailable: boolean;
  createdAt: string;
  author?: Author;
  reviews?: Review[];
  categories?: Category[];
  averageRating?: number;
  reviewCount?: number;
}

export interface Review {
  id: number;
  title: string;
  content: string;
  rating: number;
  reviewerName: string;
  createdAt: string;
  book?: Book;
}

export interface Category {
  id: number;
  name: string;
  description?: string;
  books?: Book[];
  bookCount?: number;
}

export interface LibraryStatistics {
  totalBooks: number;
  totalAuthors: number;
  totalReviews: number;
  totalCategories: number;
  averageBookRating: number;
  availableBooks: number;
}

export interface PageInfo {
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  startCursor?: string;
  endCursor?: string;
}

export interface Connection<T> {
  pageInfo: PageInfo;
  totalCount: number;
  nodes: T[];
}
