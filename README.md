# TSNO Backend

## Overview
The backend of **"Transfer Short Notes Online" (TSNO)** is a **.NET Core Web API** application that integrates with **SQL Server** using **Entity Framework Core**. It manages note creation, retrieval, and expiration, as well as automatic cleanup of expired notes. It also includes rate limiting and maximum active note constraints, ensuring performance and security.

## Key Technologies
- **.NET Core Web API**  
  - High-performance and secure RESTful APIs.
  - Well-documented and widely supported.

- **Entity Framework Core & SQL Server**  
  - Simplifies database operations with a code-first approach.
  - Automatically applies migrations and handles schema changes.

- **ASP.NET Core Rate Limiting**  
  - Limits to 15 requests per minute, preventing abuse and overload.

## Features of the Backend
- **Create Notes:** A POST endpoint to store notes with configurable expiration conditions.  
- **Retrieve Notes:** A GET endpoint to fetch notes and trigger expiration if needed.
- **Expiration Logic:** Notes can expire immediately after one view or after 5 minutes if untouched.  
- **Background Cleanup:** A service runs every 5 minutes to remove expired notes.  
- **Maximum Limits:** Supports up to 9999 active notes and 10,000 characters per message.

## Running the Backend
**Set Connection String:**  
Ensure `ConnectionStrings__DefaultConnection` in `appsettings.json` or environment variables points to the SQL Server instance.

**Run the Application:**
```bash
dotnet restore
dotnet run
```
The backend is available at http://localhost:8080.
On startup, it checks database connectivity and applies any pending migrations.

## Environment Variables
- **ConnectionStrings__DefaultConnection:** The SQL Server connection string.
- **CLIENT_URL:** The frontend URL for CORS configuration.
