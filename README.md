# SaaSify

A production-grade **Multi-Tenant SaaS Platform** built with .NET 8, demonstrating real-world architecture patterns.

## Architecture Overview

```
SaaSify/
├── src/
│   ├── SaaSify.Api            # HTTP entry point
│   ├── SaaSify.Application    # CQRS handlers & business logic
│   ├── SaaSify.Domain         # Core entities, no dependencies
│   └── SaaSify.Infrastructure # EF Core, payments, AI
└── tests/
    └── SaaSify.Tests          # xUnit test suite
```

### Layers

| Layer | Responsibility | Key Dependencies |
|---|---|---|
| **Domain** | Entities, value objects, domain events, interfaces | None |
| **Application** | CQRS commands/queries via MediatR, validation, pipeline behaviours | Domain |
| **Infrastructure** | EF Core DbContext, multi-tenancy resolution, Stripe, Razorpay, Semantic Kernel | Domain, Application |
| **Api** | Controllers, middleware (tenant resolution, auth), DI wiring | Application, Infrastructure |
| **Tests** | Unit & integration tests | Application, Domain |

### Dependency Flow

```
Api → Application → Domain
Api → Infrastructure → Domain
         ↑
  Infrastructure also → Application (implements interfaces)
```

Domain is the innermost ring — it has zero external dependencies.

---

## Key Features

### Multi-Tenant Data Isolation
Each tenant's data is scoped at the database level via a `TenantId` discriminator. The `ITenantContext` interface (resolved per-request from the JWT or subdomain) is injected into the EF Core `DbContext` to automatically filter all queries.

### CQRS with MediatR
Commands mutate state; queries return read models. Pipeline behaviours in `Application/Common/Behaviours` handle cross-cutting concerns (logging, validation, performance). Handlers live under feature folders (`Tenants/`, `Subscriptions/`).

### Payment Integration
- **Stripe** — primary payment gateway for subscription billing and webhooks.
- **Razorpay** — secondary gateway for INR-denominated markets.
Both are abstracted behind an `IPaymentGateway` interface so the application layer stays provider-agnostic.

### AI Assistant
Microsoft **Semantic Kernel** connects to **Azure OpenAI** to power a tenant-scoped AI assistant. Plugins in `Infrastructure/AI` implement the SK plugin interface and can be swapped for other providers.

---

## Getting Started

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run API
dotnet run --project src/SaaSify.Api

# Run tests
dotnet test
```

## Tech Stack

- .NET 8 / ASP.NET Core
- MediatR 12
- Entity Framework Core 8
- Stripe.net / Razorpay SDK
- Microsoft Semantic Kernel
- Azure OpenAI
- xUnit + FluentAssertions
