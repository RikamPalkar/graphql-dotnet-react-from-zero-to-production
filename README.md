# GraphQL with .NET & React — From Zero to Production

> A complete, full-stack GraphQL reference implementation in .NET 8 & React
> built feature by feature, concept by concept, from first query to JWT-secured
> production patterns.

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![Language](https://img.shields.io/badge/Language-C%23-green)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Frontend](https://img.shields.io/badge/Frontend-React-61DAFB)](https://reactjs.org/)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)

---

## Purpose

REST is familiar. GraphQL is powerful. For many .NET developers, getting started with GraphQL can still feel unclear, especially when trying to connect all the pieces in a real project.

This repo is that resource.

A fully working Library Management System built with .NET 8 on the backend
& React on the frontend — covering every GraphQL concept from a basic query
all the way to JWT authentication, real-time WebSocket subscriptions,
pagination, & production error handling.

Companion code for the 5-part article series on C# Corner — already read
by thousands of developers:

- 📖 [Part 1 — Foundations & Library Backend](https://www.c-sharpcorner.com/article/graphql-with-net-react-from-zero-to-production-part-1-foundations-libra/)
- 📖 [Part 2 — Query Arguments, Aliases & Fragments](https://www.c-sharpcorner.com/article/graphql-with-net-react-part-2-query-arguments-aliases-fragments/)
- 📖 [Part 3 — Mutations: Create, Update & Delete](http://c-sharpcorner.com/article/graphql-with-net-react-part-3-mutations-create-update-delete/)
- 📖 [Part 4 — Real-Time Data with WebSockets](https://www.c-sharpcorner.com/article/graphql-with-net-react-part-4-real-time-data-with-web-sockets/)
- 📖 [Part 5 — Advanced Patterns: Pagination, Errors, JWT & Testing](https://www.c-sharpcorner.com/article/graphql-with-net-react-part-5-advanced-patterns-pagination-errors-jwt/)

---

## What We're Building

A **Library Management System** — a real domain with real complexity:

- Browse & search books with filtering & pagination
- View author profiles & their associated books
- Add reviews with real-time updates via WebSocket subscriptions
- Full CRUD for books, authors, & reviews
- JWT authentication & authorization
- Production-grade error handling

---

## What You'll Learn — Part by Part

| Part | Topic | What It Covers |
|---|---|---|
| 1 | Foundations | GraphQL vs REST, HotChocolate setup, first query, schema design |
| 2 | Queries | Arguments, aliases, fragments, query variables |
| 3 | Mutations | Create, update, delete — write operations done right |
| 4 | Subscriptions | Real-time data with WebSockets, live review feed |
| 5 | Advanced | Cursor pagination, global error handling, JWT auth, testing |

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 8, ASP.NET Core, HotChocolate |
| Frontend | React, Apollo Client |
| Real-Time | GraphQL Subscriptions over WebSockets |
| Auth | JWT Bearer Tokens |
| Database | EF Core (InMemory — swap to SQL with one line) |
| API Explorer | Banana Cake Pop (built into HotChocolate) |

---

## Folder Structuregraphql-dotnet-react-from-zero-to-production/

```
│
├── backend/
│   ├── Models/              # Domain entities: Book, Author, Review
│   ├── Data/                # EF Core DbContext (InMemory)
│   ├── GraphQL/
│   │   ├── Queries/         # Query resolvers
│   │   ├── Mutations/       # Mutation resolvers
│   │   ├── Subscriptions/   # Real-time subscription resolvers
│   │   └── Types/           # GraphQL type definitions
│   ├── Auth/                # JWT configuration & middleware
│   └── Program.cs           # App setup & HotChocolate registration
│
└── frontend/
├── src/
│   ├── components/      # Book, Author, Review UI components
│   ├── graphql/         # Queries, mutations & subscription definitions
│   ├── hooks/           # Apollo Client hooks
│   └── App.tsx          # Root component
└── package.json

```
---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)

### Run the Backend

```bashgit clone https://github.com/RikamPalkar/graphql-dotnet-react-from-zero-to-production.git
cd backend
dotnet run

Navigate to `http://localhost:5000/graphql` — Banana Cake Pop opens automatically.

```
### Run the Frontend

```bashcd frontend
npm install
npm run dev

```
Frontend runs at `http://localhost:5173`.

---

## Key Concepts Demonstrated

### 1. Your First GraphQL Query

```graphqlquery {
books {
id
title
author {
name
}
}
}

```
No over-fetching. No under-fetching. You get exactly what you ask for.

### 2. Mutations — Writing Data

```graphqlmutation {
addBook(input: {
title: "Clean Code"
authorId: "1"
}) {
id
title
}
}

```
### 3. Real-Time Subscriptions

```graphqlsubscription {
onReviewAdded {
bookId
rating
comment
}
}

```
WebSocket connection. Live updates. No polling.

### 4. Pagination — Production Style

```graphqlquery {
books(first: 10, after: "cursor==") {
edges {
node { title }
cursor
}
pageInfo {
hasNextPage
endCursor
}
}
}

```
Cursor-based pagination — the way production APIs do it.

### 5. JWT Authentication

```graphqlmutation {
login(input: { username: "admin", password: "secret" }) {
token
}
}

```
Token secured. Protected resolvers. Production-ready auth flow.

---

## GraphQL vs REST — The Real Difference

| Problem | REST | GraphQL |
|---|---|---|
| Over-fetching | Returns full object always | Returns only requested fields |
| Under-fetching | Multiple round trips | Single query, nested data |
| Real-time | Needs separate WebSocket setup | Built-in subscriptions |
| API versioning | /v1, /v2, /v3... | Schema evolution, no versioning |
| Documentation | Manual or Swagger | Self-documenting schema |

---

## When to Use GraphQL

✅ Complex frontend with multiple data relationships  
✅ Multiple clients (web, mobile) needing different data shapes  
✅ Real-time features are a requirement  
✅ Rapid frontend iteration without backend changes  

❌ Simple CRUD with 2–3 endpoints  
❌ File upload heavy applications  
❌ Teams unfamiliar with schema-first thinking  

---

## Read the Full Series

| Article | Reads |
|---|---|
| [Part 1 — Foundations](https://www.c-sharpcorner.com/article/graphql-with-net-react-from-zero-to-production-part-1-foundations-libra/) | 2,600+ |
| [Part 2 — Queries](https://www.c-sharpcorner.com/article/graphql-with-net-react-part-2-query-arguments-aliases-fragments/) | 800+ |
| [Part 3 — Mutations](http://c-sharpcorner.com/article/graphql-with-net-react-part-3-mutations-create-update-delete/) | 600+ |
| [Part 4 — Real-Time](https://www.c-sharpcorner.com/article/graphql-with-net-react-part-4-real-time-data-with-web-sockets/) | 585+ |
| [Part 5 — Advanced](https://www.c-sharpcorner.com/article/graphql-with-net-react-part-5-advanced-patterns-pagination-errors-jwt/) | 454+ |

---

## Contributing

Want to add more examples, fix issues, or extend the schema?

1. Fork the repo
2. Create a branch (`git checkout -b feature/your-idea`)
3. Commit & push
4. Open a Pull Request

Ideas: add DataLoader for N+1 prevention, Docker support, integration tests,
role-based authorization, persisted queries.

---

## License

MIT License — use this however you want.

---

## Connect

**Rikam Palkar** — Senior Software Engineer, Microsoft MVP

- 🌐 [rikampalkar.github.io](https://rikampalkar.github.io)
- 💼 [LinkedIn](https://www.linkedin.com/in/rikampalkar/)
- ✍️ [Medium](https://medium.com/@RikamPalkar)
- 🐙 [GitHub](https://github.com/RikamPalkar)

---

*Stop learning GraphQL in isolation. Build the whole stack.*
