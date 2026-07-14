# Restful API Bookstore

A learning project exploring the fundamentals of building a RESTful web API using .NET 8 and ASP.NET Core. This project demonstrates core concepts including authentication, session management, and CRUD operations.

## 📚 Project Overview

This is my starting learning curve on developing a backend RESTful API using .NET. The project implements:
- User authentication with MD5 password hashing
- In-memory session management (server-side)
- CRUD operations for a category management system
- Middleware-based authentication similar to Laravel's approach

## 🛠️ Tech Stack

- **Framework**: .NET 8 with ASP.NET Core
- **Database**: MySQL
- **ORM**: Entity Framework Core
- **Tools**: Swagger/OpenAPI for API documentation
- **Language**: C#

## 📋 Database Schema

### Users Table
```sql
CREATE TABLE user (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    deleted_at DATETIME DEFAULT NULL
);
```

### Categories Table
```sql
CREATE TABLE category (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    deleted_at DATETIME DEFAULT NULL
);
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- MySQL Server
- Visual Studio or Visual Studio Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/geoffembuscad0/Restful-API-Bookstore.git
   cd Restful-API-Bookstore
   ```

2. **Update database connection**
   
   Edit `appsettings.json` with your MySQL credentials:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=test_bookstore;User=root;Password=your_password;"
   }
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Create the database and tables**
   
   Run the SQL schema scripts or use Entity Framework migrations if configured.

5. **Run the application**
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` and Swagger UI at `https://localhost:5001/swagger`

## 📡 API Endpoints

### Authentication Endpoints (Public)

#### Register User
```
POST /api/auth/register
Content-Type: application/json

{
  "username": "testuser",
  "password": "password123"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "username": "testuser",
  "token": "550e8400-e29b-41d4-a716-446655440000",
  "message": "User registered successfully"
}
```

#### Login User
```
POST /api/auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "username": "testuser",
  "token": "550e8400-e29b-41d4-a716-446655440000",
  "message": "Login successful"
}
```

#### Logout User
```
POST /api/auth/logout
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "message": "Logout successful"
}
```

### Category Endpoints (Protected)

All category endpoints require authentication via Bearer token in the Authorization header.

#### Get All Categories
```
GET /api/categories
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "title": "Science Fiction",
    "createdAt": "2026-07-14T09:28:29Z",
    "deletedAt": null
  },
  {
    "id": 2,
    "title": "Mystery",
    "createdAt": "2026-07-14T09:29:15Z",
    "deletedAt": null
  }
]
```

#### Get Category by ID
```
GET /api/categories/{id}
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Science Fiction",
  "createdAt": "2026-07-14T09:28:29Z",
  "deletedAt": null
}
```

#### Create Category
```
POST /api/categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Fantasy"
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "title": "Fantasy",
  "createdAt": "2026-07-14T09:30:00Z",
  "deletedAt": null
}
```

#### Update Category
```
PUT /api/categories/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Updated Category Title"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Updated Category Title",
  "createdAt": "2026-07-14T09:28:29Z",
  "deletedAt": null
}
```

#### Delete Category (Soft Delete)
```
DELETE /api/categories/{id}
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "message": "Category deleted successfully"
}
```

## 🔐 Authentication & Authorization

### How It Works

1. **Registration & Login**: Users register with username and password. Password is hashed using MD5 before storage.
2. **Session Token**: Upon successful login or registration, a session token (UUID) is generated and stored in-memory on the server.
3. **Request Authentication**: All protected endpoints require the Bearer token in the Authorization header:
   ```
   Authorization: Bearer {token}
   ```
4. **Middleware Validation**: The `AuthenticationMiddleware` intercepts all requests and validates the token before allowing access to protected endpoints.
5. **Logout**: Removes the session token from server memory, invalidating any future requests with that token.

### Public vs Protected Endpoints

**Public Endpoints** (no token required):
- `POST /api/auth/register`
- `POST /api/auth/login`
- Swagger documentation

**Protected Endpoints** (token required):
- `POST /api/auth/logout`
- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories`
- `PUT /api/categories/{id}`
- `DELETE /api/categories/{id}`

## 📁 Project Structure

```
Restful-API-Bookstore/
├── Models/
│   ├── User.cs
│   └── Category.cs
├── Controllers/
│   ├── AuthController.cs
│   └── CategoriesController.cs
├── Services/
│   ├── AuthService.cs
│   └── SessionService.cs
├── Middleware/
│   └── AuthenticationMiddleware.cs
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   ├── RegisterRequest.cs
│   ├── LoginRequest.cs
│   ├── AuthResponse.cs
│   └── CategoryRequest.cs
├── Program.cs
├── appsettings.json
├── RestfulApiDemo.csproj
└── README.md
```

## 🧠 Learning Concepts Covered

- **RESTful API Design**: Proper HTTP methods and status codes
- **Authentication & Authorization**: User registration, login, and token-based session management
- **In-Memory Session Storage**: Server-side session management using Dictionary with thread-safe locks
- **Middleware Pattern**: Custom middleware for cross-cutting concerns (authentication)
- **Entity Framework Core**: ORM for database operations
- **Soft Deletes**: Logical deletion using `deleted_at` timestamp instead of physical deletion
- **Dependency Injection**: ASP.NET Core's built-in DI container for service management
- **DTOs**: Separating request/response models from domain models
- **CRUD Operations**: Complete Create, Read, Update, Delete functionality
- **Error Handling**: Proper HTTP status codes and error responses
- **Swagger/OpenAPI**: API documentation and testing interface

## ⚠️ Important Notes

### Password Hashing
This project uses **MD5 hashing** for educational purposes. In a production environment, use strong hashing algorithms like:
- bcrypt
- PBKDF2
- Argon2

### Session Storage
Sessions are stored **in-memory**, which means:
- Sessions will be lost when the application restarts
- Not suitable for multi-instance deployments
- For production, use distributed cache like Redis or database storage

### CORS
CORS is configured to allow all origins for development. In production, restrict to specific origins:
```csharp
builder.AllowAnyOrigin()  // ❌ Not for production
```

## 🔄 Future Improvements

- [ ] Use bcrypt or Argon2 for password hashing
- [ ] Implement JWT tokens instead of in-memory sessions
- [ ] Add database-backed session storage
- [ ] Implement refresh tokens
- [ ] Add role-based access control (RBAC)
- [ ] Add request validation and logging
- [ ] Implement unit and integration tests
- [ ] Add API rate limiting
- [ ] Setup CI/CD pipeline

## 📝 License

This is a learning project. Feel free to use and modify as needed.

## 👤 Author

**Geoffrey Embuscado**

---

**This project represents my beginning journey into .NET RESTful API development. Happy coding! 🚀**
