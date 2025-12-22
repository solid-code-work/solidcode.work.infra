# solidcode.work.infra

`solidcode.work.infra` is a reusable infrastructure library for .NET developers.  
It provides ready-to-use building blocks for:

- **Databases**: EF Core (SQL Server, PostgreSQL), MongoDB
- **Caching**: Redis
- **Messaging**: MassTransit + RabbitMQ
- **Resilience**: Polly-based HTTP client helpers
- **Unified responses**: `TResult<T>` and `MessageErrorType`
- **Logging**: Built-in `ILogger<T>` integration with optional Serilog extension

## Installation
```bash
dotnet add package solidcode.work.infra
