# solidcode.work.infra

**solidcode.work.infra** is a reusable infrastructure library designed to simplify integration with databases, caching, messaging, HTTP clients, and logging in modern .NET applications.  
It provides a consistent set of utilities, repositories, and helpers that make enterprise and industrial applications easier to build, maintain, and scale.

---

## ✨ Features

- **Database Support**
  - Entity Framework Core (SQL Server, PostgreSQL, MSSQL)
  - MongoDB integration
- **Caching**
  - Redis cache service with `ICacheService` abstraction
- **Messaging**
  - MassTransit + RabbitMQ integration with retry policies
- **HTTP**
  - Resilient `HttpServiceHelper` with Polly retry policies
- **Configuration**
  - Strongly-typed settings classes for each external dependency
- **Repositories**
  - Generic repository pattern for EF Core and MongoDB
- **Unified Responses**
  - `TResult<T>` and `TResultFactory` for consistent success/failure handling
  - `MessageErrorType` enum for standardized error classification
- **Logging**
  - Built-in `ILogger<T>` integration via `AddSolidcodeLogging()`
  - Professional, structured logging with colors and file output via `UseSolidcodeSerilog()`

---

## 📦 Installation

Clone the repository:

```bash
git clone https://github.com/<your-username>/solidcode.work.infra.git
cd solidcode.work.infra

Or install via NuGet:

dotnet add package solidcode.work.infra

