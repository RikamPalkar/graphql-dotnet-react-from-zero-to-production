import { useMutation, useQuery } from '@apollo/client';
import { ADD_BOOK } from '../graphql/mutations';
import { GET_AUTHORS, GET_CATEGORIES, GET_BOOKS } from '../graphql/queries';
import { Author, Category, Connection } from '../types';
import { useState } from 'react';

interface AuthorsData {
  authors: Connection<Author>;
}

interface CategoriesData {
  categories: Category[];
}

export function AddBook({ onSuccess }: { onSuccess: () => void }) {
  const [form, setForm] = useState({
    title: '',
    description: '',
    isbn: '',
    publishedYear: new Date().getFullYear(),
    genre: '',
    price: 0,
    pageCount: 0,
    authorId: 0,
    categoryIds: [] as number[],
  });

  const { data: authorsData } = useQuery<AuthorsData>(GET_AUTHORS, {
    variables: { first: 100 },
  });
  const { data: categoriesData } = useQuery<CategoriesData>(GET_CATEGORIES);

  const [addBook, { loading, error }] = useMutation(ADD_BOOK, {
    refetchQueries: [{ query: GET_BOOKS, variables: { first: 10 } }],
    onCompleted: () => {
      onSuccess();
    },
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await addBook({
      variables: {
        input: form,
      },
    });
  };

  const handleCategoryToggle = (categoryId: number) => {
    setForm((prev) => ({
      ...prev,
      categoryIds: prev.categoryIds.includes(categoryId)
        ? prev.categoryIds.filter((id) => id !== categoryId)
        : [...prev.categoryIds, categoryId],
    }));
  };

  return (
    <div className="add-book">
      <h2>Add New Book</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Title</label>
          <input
            type="text"
            value={form.title}
            onChange={(e) => setForm({ ...form, title: e.target.value })}
            required
          />
        </div>

        <div className="form-group">
          <label>Author</label>
          <select
            value={form.authorId}
            onChange={(e) => setForm({ ...form, authorId: Number(e.target.value) })}
            required
          >
            <option value={0}>Select an author</option>
            {authorsData?.authors.nodes.map((author) => (
              <option key={author.id} value={author.id}>
                {author.name}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label>Description</label>
          <textarea
            value={form.description}
            onChange={(e) => setForm({ ...form, description: e.target.value })}
          />
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>ISBN</label>
            <input
              type="text"
              value={form.isbn}
              onChange={(e) => setForm({ ...form, isbn: e.target.value })}
            />
          </div>

          <div className="form-group">
            <label>Genre</label>
            <input
              type="text"
              value={form.genre}
              onChange={(e) => setForm({ ...form, genre: e.target.value })}
            />
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Published Year</label>
            <input
              type="number"
              value={form.publishedYear}
              onChange={(e) => setForm({ ...form, publishedYear: Number(e.target.value) })}
              required
            />
          </div>

          <div className="form-group">
            <label>Page Count</label>
            <input
              type="number"
              value={form.pageCount}
              onChange={(e) => setForm({ ...form, pageCount: Number(e.target.value) })}
              required
            />
          </div>

          <div className="form-group">
            <label>Price ($)</label>
            <input
              type="number"
              step="0.01"
              value={form.price}
              onChange={(e) => setForm({ ...form, price: Number(e.target.value) })}
              required
            />
          </div>
        </div>

        <div className="form-group">
          <label>Categories</label>
          <div className="categories-checkboxes">
            {categoriesData?.categories.map((category) => (
              <label key={category.id} className="checkbox-label">
                <input
                  type="checkbox"
                  checked={form.categoryIds.includes(category.id)}
                  onChange={() => handleCategoryToggle(category.id)}
                />
                {category.name}
              </label>
            ))}
          </div>
        </div>

        {error && <div className="error">{error.message}</div>}

        <button type="submit" disabled={loading}>
          {loading ? 'Adding...' : 'Add Book'}
        </button>
      </form>
    </div>
  );
}
