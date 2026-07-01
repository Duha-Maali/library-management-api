# Library Management API

A backend training project built with ASP.NET Core Web API for managing books, categories, borrowers, users, and borrowing operations.

The project applies a three-layer architecture and includes JWT authentication, role-based authorization, Entity Framework Core, MySQL, AutoMapper, Serilog, the Repository Pattern, a service layer, middleware, and custom business exceptions.

> This project was developed to practice backend development concepts using ASP.NET Core Web API, including layered architecture, authentication, authorization, data access, logging, and API design.

## Features

### Book Management

- Add new books.
- Update book information.
- Delete books.
- Retrieve all books.
- Retrieve book details.
- Track total and borrowed copies.

### Category Management

- Create, update, delete, and retrieve book categories.
- Organize books by category.

### Borrower Management

- Create and manage borrower records.
- Retrieve borrower information and borrowing activity.

### Borrowing Operations

- Create borrowing records.
- Track borrowing and due dates.
- Process book returns.
- Manage borrowing status.
- View borrowing history.

### User and Access Management

- Register and authenticate users.
- Generate JWT access tokens.
- Protect endpoints using role-based authorization.

### Logging and Error Handling

- Record application activity using Serilog.
- Use middleware and custom business exceptions to handle application errors.

## Architecture

The project follows a three-layer architecture to separate responsibilities and keep the code organized.

### Presentation Layer

Responsible for handling HTTP requests and returning API responses.

It includes:

- Controllers
- Middleware
- Authentication and authorization configuration
- Application startup and dependency registration

### Business Layer

Contains the application’s business logic and validation rules.

It includes:

- Services
- Service interfaces
- View models
- AutoMapper profiles
- Custom business exceptions

### Data Layer

Responsible for database communication and data persistence.

It includes:

- Entity models
- Entity Framework Core `DbContext`
- Repository interfaces and implementations
- Entity configurations
- EF Core migrations

## Technology Stack

- **C#** — Main programming language.
- **ASP.NET Core Web API** — Building RESTful API endpoints.
- **Entity Framework Core** — Database access and object-relational mapping.
- **MySQL** — Relational database management system.
- **JWT Authentication** — User authentication and token generation.
- **Role-Based Authorization** — Restricting protected operations according to user roles.
- **AutoMapper** — Mapping between entities and view models.
- **Repository Pattern** — Separating data-access logic from business logic.
- **Serilog** — Structured application logging.
- **Swagger / OpenAPI** — API documentation and endpoint testing.

## Main Entities

### User

Represents an authenticated system user and stores account information and the assigned role.

### Category

Represents a category used to organize books in the library.

### Book

Represents a library book and includes information such as its title, author, category, total copies, and borrowed copies.

### Borrower

Represents a person who can borrow books from the library.

### Borrow

Represents a borrowing transaction between a borrower and a book.

A borrowing record includes:

- Borrow date
- Due date
- Return date
- Borrow status
- Related book
- Related borrower

## Authentication and Authorization

The API uses JWT-based authentication to protect secured endpoints.

After a successful login, the application generates a JWT access token containing user and role claims. The token must be included in the `Authorization` header when accessing protected endpoints:

```text
Authorization: Bearer <token>
```

Role-based authorization is used to restrict library-management operations to authorized users, such as librarians.

## API Areas

The API is organized into the following main areas:

### Users

Handles user registration, authentication, and JWT token generation.

### Books

Provides operations for creating, updating, deleting, and retrieving books.

### Categories

Provides operations for creating, updating, deleting, and retrieving book categories.

### Borrowers

Provides operations for creating, updating, deleting, and retrieving borrower records.

### Borrowing

Provides operations for:

- Creating borrowing records
- Returning borrowed books
- Tracking due dates
- Updating borrowing status
- Viewing borrowing history

## Project Structure

```text
LibraryManagementSystem/
├── LibraryManagement.Presentation/
│   ├── Controllers/
│   ├── Middlewares/
│   ├── Extensions/
│   └── Program.cs
│
├── LibraryManagement.Domain/
│   ├── Services/
│   ├── ViewModels/
│   ├── MappingProfiles/
│   ├── Exceptions/
│   └── Interfaces/
│
├── LibraryManagement.Infrastructure/
│   ├── Models/
│   ├── Repositories/
│   ├── Configurations/
│   ├── Migrations/
│   └── LibraryDbContext.cs
│
└── LibraryManagementSystem.sln
```

## How to Run

### Prerequisites

Make sure the following tools are installed:

- .NET SDK
- Visual Studio 2022 or later
- MySQL Server
- MySQL Workbench or another MySQL database-management tool

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/Duha-Maali/library-management-api.git
   ```

2. Open the project directory:

   ```bash
   cd LibraryManagementSystem
   ```

3. Open `LibraryManagementSystem.sln` in Visual Studio.

4. Configure the MySQL connection string and JWT settings.

5. Restore the project dependencies:

   ```bash
   dotnet restore
   ```

6. Apply the Entity Framework Core migrations to create the database:

   ```bash
   dotnet ef database update
   ```

7. Set `LibraryManagement.Presentation` as the startup project.

8. Run the application using Visual Studio or the .NET CLI.

9. Open the Swagger URL displayed when the application starts to explore and test the API endpoints.
