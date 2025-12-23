# solidcode.work.infra

`solidcode.work.infra` is a reusable infrastructure library for .NET developers.  
It provides ready-to-use building blocks for:

- **Databases**: EF Core (SQL Server, PostgreSQL), MongoDB
- **Caching**: Redis
- **Messaging**: MassTransit + RabbitMQ
- **Resilience**: Polly-based HTTP client helpers
- **Unified responses**: `TResult<T>` and `MessageErrorType`
- **Logging**: Built-in `ILogger<T>` integration with optional Serilog extension
- **Authentication**
  - JWT bearer authentication with issuer, audience, and secret key validation
  - One‑line setup via `AddSolidcodeJwtAuthentication()`

## Installation
```bash
dotnet add package solidcode.work.infra



### 🔐 Authentication

Enable JWT authentication with a single extension:

```csharp
builder.Services.AddSolidcodeJwtAuthentication();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
