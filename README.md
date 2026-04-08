# 📖 About solidcode.work.infra

**solidcode.work.infra** is a reusable infrastructure library designed to simplify integration with databases, caching, messaging, HTTP clients, logging, and authentication in modern .NET applications.  
It provides a consistent set of utilities, repositories, and helpers that make enterprise and industrial applications easier to build, maintain, and scale.

Starting a new web application often means repeating the same setup steps: configuring databases, wiring up caching, adding messaging, setting up HTTP clients, and pulling in all the common packages. This process is time‑consuming, error‑prone, and something every developer ends up doing again and again for each project.

I built **solidcode.work.infra** to make this process easy, fast, and standardized. Instead of spending hours on boilerplate setup, you can install this package from NuGet and immediately have a ready‑to‑use infrastructure layer that follows best practices and is consistent across projects.

By utilizing this package:

🚀 You can start projects faster with the essential building blocks already in place.  
⚙️ Your applications are standardized and collaborative, reducing friction when working in teams.  
📦 Common integrations (databases, caching, messaging, HTTP, logging, authentication) are pre‑configured and ready to extend.  

This library is the backbone of my portfolio and reflects my approach to building clean, modular, and reusable infrastructure code for modern .NET applications. It’s designed to save time, enforce consistency, and let you focus on the unique parts of your project instead of reinventing the wheel.

*Created and maintained by **Shahram Etemadi** as part of the [solidcode.work](https://solidcode.work) portfolio.*

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
  - Strongly‑typed settings classes for each external dependency
- **Repositories**
  - Generic repository pattern for EF Core and MongoDB
- **Unified Responses**
  - `TResponse<T>` and `TResponseFactory` for consistent success/failure handling
  - `MessageErrorType` enum for standardized error classification
- **Logging**
  - Built‑in `ILogger<T>` integration via `AddSolidcodeLogging()`
  - Professional, structured logging with colors and file output via `UseSolidcodeSerilog()`
- **Authentication**
  - JWT bearer authentication with issuer, audience, and secret key validation
  - One‑line setup via `AddSolidcodeJwtAuthentication()`

---

## 📦 Installation

Clone the repository:

```bash
git clone https://github.com/<your-username>/solidcode.work.infra.git
cd solidcode.work.infra
