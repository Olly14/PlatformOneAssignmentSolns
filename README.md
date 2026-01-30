# PlatformOneAssignmentSolns
This project implements an HTTP API for storing and quering financial asset metadat (name, symbol, ISIN) and daily prices from multiple sources, with the constraint that each asset can have at most one price per source per day.

How to Run:
Prerequisites
  * .NET SDK(Latest Stable: .NET10)
  * Optional EF Coor Tools(Only needed if running migrations manually)

Restore dependencies: dotnet restore.

Database setup
This project uses SQLite via EF Core.

Option A: Auto-create the DB on startup.
The Api will call EnsureCreated on startup and seed sample data if the DB is Empty.
just run: dotnet run --project PlatformOne.Assets.Api.

Option B - Migrations
Install EF tools once:
dotnet tool install --global dotnet-ef

Create migration: dotnet ef migrations add InitialCreate --project PlatformOne.Assets.Infrastructure --startup-project PlatformOne.Assets.Api.
Apply migration: dotnet ef database update --project PlatformOne.Assets.Infrastructure --startup-project PlatformOne.Assets.Api.

Run: dotnet run --project PlatformOne.Assets.Api.

Swagger URL
After running the API
  * Swagger UI: https://localhost:<port>/swagger
  * OpenAPI JSON: https//localhost:<port>/swagger/v1/swagger.json

Endpoints (RESTful)
Assets
  * Get /api/assets?symbol=MSFT&isin=US -> Returns all assets, optionally filtered by symbol and/or ISIN.
  * PUT /api/assets/{symbol} -> Upserts an asset (create if missing, update if exists).

Prices
  * GET /api/prices?date=2026-01-29&source=Reuters&symbols=MSFT, AAPL -> Returns prices for one or more assets for a given date, optionally filtered by source. Response includes LastUpdatedUtc per row.
  * PUT /api/prices -> Upserts a price for an asset + source + date.

Design decisions
Why SQLite + EF Core
  * SQLite provides a lightweight file-based database that requires no external dependencies, making the project easy to run for reviewers.
  * EF Core provides a clean abstraction, supports migrations, and makes it straightforward to enforce constraints (such as unique indexes) at the database level.

Why upsert via HTTTPUT
  * The business requirement allows “Create or update an existing asset/price”.
  * PUT is idempotent: sending the same request multiple times results in the same final state.
  * It simplifies clients: they don’t need to check whether the record exists before writing.

Why DateOnly for price date
  * Prices are collected “per day”, not as an intraday timeseries.
  * DateOnly prevents confusion around time zones and avoids accidental off-by-one issues caused by DateTime conversions.

How "One price per source per asset per date" is enforced
This rule is enforced at the database level using a unique index over:
  * AssetId.
  * SourceId.
  * PriceDate.
That guarantees no duplicate daily prices for the same asset/source pair, even under concurrency.

Timestamp rules
  * Each stored price row has a LastUpdatedUtc (DateTimeOffset).
  * On create: set to DateTimeOffset.UtcNow.
  * On update: overwrite with a fresh DateTimeOffset.UtcNow.
  * All timestamps are UTC to avoid time zone ambiguity.

Assumptions
  * Asset Symbol is treated as a stable identifier and is normalised to uppercase on write.
  * Source is identified by its Name (string). If a source does not exist during price upsert, the API can optionally auto-create it (simplifies ingestion scenarios).
  * Prices are stored as decimal values with no currency conversion logic (currency concerns are outside this exercise scope).

Tests
  * Unit/integration style tests use an in-memory SQLite database per test scope.
  * Tests validate:
      (1) price filtering by date/source/symbols.
      (2) upsert creates a new price when missing.
      (3) upsert updates the existing price and updates the timestamp.
      (4) unknown asset produces a not-found style exception.

Run tests: dotnet test
 *Test Projects
    (a) PlatformOne.Assets.Api.Tests: Integration-style controller tests.
    (b) PlatformOne.Assets.Shared.Tests: Service + business logic tests

Testing Strategy
The solution uses a layered testing approach:
  * Service tests (Shared.Tests) -> Validate business logic in isolation using SQLite in-memory databases.
  * Controller tests (Api.Tests)  -> Validate controller behaviour by invoking controller actions directly with real services and DbContext, without hosting ASP.NET.
This provides fast, reliable integration-style coverage without the overhead of full HTTP hosting.
  



