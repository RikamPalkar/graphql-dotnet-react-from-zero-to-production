import { ApolloProvider, useSubscription } from '@apollo/client';
import { client } from './graphql/client';
import { BookList } from './components/BookList';
import { BookDetail } from './components/BookDetail';
import { AuthorList } from './components/AuthorList';
import { Statistics } from './components/Statistics';
import { SearchBooks } from './components/SearchBooks';
import { AddBook } from './components/AddBook';
import { ON_BOOK_ADDED } from './graphql/subscriptions';
import { useState } from 'react';
import './App.css';

type View = 'books' | 'authors' | 'search' | 'add' | 'book-detail';

function AppContent() {
  const [currentView, setCurrentView] = useState<View>('books');
  const [selectedBookId, setSelectedBookId] = useState<number | null>(null);
  const [notification, setNotification] = useState<string | null>(null);

  useSubscription(ON_BOOK_ADDED, {
    onData: ({ data }) => {
      if (data.data?.onBookAdded) {
        setNotification(`New book added: "${data.data.onBookAdded.title}"`);
        setTimeout(() => setNotification(null), 5000);
      }
    },
  });

  const handleSelectBook = (id: number) => {
    setSelectedBookId(id);
    setCurrentView('book-detail');
  };

  const handleBack = () => {
    setSelectedBookId(null);
    setCurrentView('books');
  };

  return (
    <div className="app">
      <header>
        <h1>GraphQL Library</h1>
        <nav>
          <button
            className={currentView === 'books' ? 'active' : ''}
            onClick={() => { setCurrentView('books'); setSelectedBookId(null); }}
          >
            Books
          </button>
          <button
            className={currentView === 'authors' ? 'active' : ''}
            onClick={() => setCurrentView('authors')}
          >
            Authors
          </button>
          <button
            className={currentView === 'search' ? 'active' : ''}
            onClick={() => setCurrentView('search')}
          >
            Search
          </button>
          <button
            className={currentView === 'add' ? 'active' : ''}
            onClick={() => setCurrentView('add')}
          >
            Add Book
          </button>
        </nav>
      </header>

      {notification && (
        <div className="notification">
          {notification}
        </div>
      )}

      <main>
        <aside>
          <Statistics />
        </aside>
        <section className="content">
          {currentView === 'books' && (
            <BookList onSelectBook={handleSelectBook} />
          )}
          {currentView === 'book-detail' && selectedBookId && (
            <BookDetail bookId={selectedBookId} onBack={handleBack} />
          )}
          {currentView === 'authors' && (
            <AuthorList onSelectAuthor={(id) => console.log('Author:', id)} />
          )}
          {currentView === 'search' && (
            <SearchBooks onSelectBook={handleSelectBook} />
          )}
          {currentView === 'add' && (
            <AddBook onSuccess={() => setCurrentView('books')} />
          )}
        </section>
      </main>
    </div>
  );
}

function App() {
  return (
    <ApolloProvider client={client}>
      <AppContent />
    </ApolloProvider>
  );
}

export default App;
