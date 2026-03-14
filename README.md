# AutoCenter

AutoCenter is an **ASP.NET Core 8 web application** for browsing, publishing, and managing car listings.
It is being built as a portfolio-grade project with a strong focus on **clean architecture direction, practical business features, and production-minded development practices**.

> **Project status:** In active development. Core marketplace functionality is already implemented, while some features are still being polished or expanded.

---

## Overview

The goal of AutoCenter is to simulate a real automotive marketplace where users can:
- browse vehicle listings,
- filter and inspect listing details,
- register and manage their own account,
- create, edit, and delete listings,
- upload and manage listing images,
- save listings to favourites,
- view seller profiles,
- leave user reviews and ratings.

This project is intentionally designed not as a tutorial clone, but as a realistic full-stack learning project that demonstrates:
- layered application structure,
- ASP.NET Core Identity integration,
- relational data modeling with Entity Framework Core,
- PostgreSQL-based persistence,
- Dockerized local setup,
- reusable Razor Pages UI components,
- service-oriented business logic.

---

## Current Status

### Implemented
- User authentication and authorization with **ASP.NET Core Identity**
- Razor Pages application structure
- Listing management:
  - create listing
  - edit listing
  - delete listing
  - listing details
  - listing index/catalog page
- Vehicle-related data model:
  - brand
  - model
  - vehicle specifications
- Listing image storage and management
- Favourite listings for authenticated users
- User profile/details page
- Review system between users
- Database seeding for brands, models, and demo data
- Docker Compose setup with PostgreSQL
- Issue templates and PR template for repository hygiene

### In Progress / Planned
- additional validation and UX improvements,
- stronger search and filtering experience,
- more complete production configuration,
- automated tests,
- CI pipeline improvements,
- role/permission expansion,
- better observability and deployment readiness.

---

## Tech Stack

### Backend
- **.NET 8**
- **ASP.NET Core Razor Pages**
- **Entity Framework Core 8**
- **ASP.NET Core Identity**
- **FluentResults**
- **Npgsql / PostgreSQL**

### Frontend
- Razor Pages
- HTML / CSS / JavaScript
- Reusable partial views for shared UI blocks

### Infrastructure
- **Docker**
- **Docker Compose**
- Local file-based image storage

---

## Architecture Highlights

The project follows a **structured, layered approach** instead of putting all logic directly into pages.

### Main architectural decisions
- **Razor Pages** for the web layer
- **Service layer** for business operations
- **DTOs** for transport/query models
- **Entity Framework Core** for persistence
- **ASP.NET Core Identity** for account management and authorization
- **Dedicated infrastructure layer** for database, seeding, and image storage
- **Reusable partial views** for shared UI composition

### Why this matters
This keeps the codebase easier to grow and maintain compared to a page-only or controller-heavy implementation where business logic becomes tightly coupled to UI code.

---

## Project Structure

```text
AutoCenter/
├── AutoCenter/                 # Main ASP.NET Core web application
│   ├── Areas/Identity/         # Identity UI and authentication-related pages
│   ├── Dtos/                   # Data transfer and filtering models
│   ├── Enums/                  # Domain enums
│   ├── Infrastructure/         # Data access, seeding, extensions, image infrastructure
│   ├── Migrations/             # Entity Framework Core migrations
│   ├── Models/                 # Domain entities
│   ├── Pages/                  # Razor Pages and page models
│   ├── Services/               # Business logic/services
│   ├── Settings/               # Strongly-typed configuration classes
│   └── wwwroot/                # Static assets and uploaded files
├── .github/                    # Issue templates and PR template
├── docker-compose.yaml         # Local container orchestration
├── Dockerfile                  # Application container build definition
└── README.md
```

---

## Domain Model Snapshot

The current model includes the following main entities:
- **ApplicationUser** – authenticated user profile based on Identity
- **Listing** – marketplace item published by a user
- **VehicleSpec** – structured vehicle information linked to a listing
- **Brand** / **CarModel** – normalized car reference data
- **ListingImage** – images attached to listings
- **Favourite** – saved listings per user
- **Review** – user-to-user rating and feedback

The database model already includes several useful constraints such as:
- unique brand names,
- unique model names per brand,
- unique favourite relation per user and listing,
- one primary image per listing,
- one review per author-target pair.

---

## Key Features

### 1. Marketplace Listings
Users can browse and manage car listings with structured vehicle data and attached images.

### 2. Authentication & Authorization
Identity is integrated directly into the application. Protected actions such as creating, editing, and deleting listings require authentication.

### 3. Favourites
Authenticated users can save listings and manage their favourites page.

### 4. Reviews & Ratings
Users can leave reviews for other users, which helps move the project closer to a real marketplace scenario.

### 5. Image Handling
Listings support image upload and image ordering, with dedicated services for storage and image-related operations.

### 6. Seeded Reference Data
The project contains seeders for brands, models, and optional demo listings, making local development faster and easier.

---

## Configuration

The project currently uses PostgreSQL connection strings via configuration.

### Relevant configuration sections
- `ConnectionStrings`
- `Seed`
- `SMTP`
- `ImageStorage`

### Example environment file
Use `.env.example` as a starting point:

```env
POSTGRES_DB=autocenter
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_PORT=5433

ASPNETCORE_ENVIRONMENT=Development
SEED_DEMO_LISTINGS=true
```

---

## Getting Started

## Option 1 — Run with Docker Compose

### 1. Clone the repository
```bash
git clone https://github.com/BriberixDev/AutoCenter.git
cd AutoCenter
```

### 2. Create environment file
Copy `.env.example` to `.env` and adjust values if needed.

### 3. Start containers
```bash
docker compose up --build
```

### 4. Open the application
By default, the app is exposed on:
```text
http://localhost:8080
```

---

## Option 2 — Run locally with .NET SDK

### Prerequisites
- .NET 8 SDK
- PostgreSQL
- EF Core tools

Install EF Core tools if needed:
```bash
dotnet tool install --global dotnet-ef
```

### 1. Restore dependencies
```bash
dotnet restore
```

### 2. Update the database connection string
Set the local PostgreSQL connection in `appsettings.json` or through environment variables.

### 3. Apply migrations
```bash
dotnet ef database update --project AutoCenter/AutoCenter.Web.csproj
```

### 4. Run the app
```bash
dotnet run --project AutoCenter/AutoCenter.Web.csproj
```

---

## Database Notes

The application is configured to work with **PostgreSQL** and uses **Entity Framework Core migrations**.

The startup flow also supports optional demo data seeding through configuration:

```json
"Seed": {
  "DemoListings": true
}
```

This is useful for local development and UI testing.

---

## Security & Auth Notes

The application already includes several practical security-oriented defaults:
- authenticated access for listing create/edit/delete pages,
- Identity-based user management,
- unique email requirement,
- account lockout configuration,
- cookie authentication configuration,
- anti-forgery validation on relevant POST actions.

This is still a development-stage project, so security hardening is an ongoing process.
Before production deployment, the following should be reviewed carefully:
- secret management,
- SMTP secrets and credentials,
- production cookie settings,
- file upload validation,
- logging and exception handling,
- HTTPS and reverse proxy configuration.

---

## Repository Standards

The repository already includes:
- bug report template,
- feature request template,
- refactor template,
- pull request template.

This helps keep collaboration cleaner and closer to professional team workflows.

---

## What This Project Demonstrates

AutoCenter is meant to showcase practical backend and full-stack engineering skills, including:
- domain modeling,
- relational database design,
- EF Core configuration,
- Identity integration,
- service extraction,
- CRUD workflows,
- file handling,
- Docker-based local development,
- scalable project structure.

It is especially suitable as a portfolio project for **junior .NET / ASP.NET Core backend or full-stack roles**.

---

## Known Limitations

Because the project is still evolving, some parts are intentionally not presented as “production complete” yet.
Current areas that can still be improved include:
- broader automated test coverage,
- CI/CD automation,
- more advanced search/filtering,
- pagination/sorting refinements,
- stricter validation and domain invariants,
- centralized error handling,
- cloud storage option for listing images,
- richer admin capabilities.

---

## Roadmap Ideas

Potential next improvements:
- add unit and integration tests,
- introduce result/error handling consistently across services,
- add pagination and sorting abstraction,
- improve listing ownership and moderation flows,
- add image size/type validation policies,
- move static uploads to external storage,
- add health checks and structured logging,
- add GitHub Actions CI for build and test validation.

---

## License

This repository includes a `LICENSE` file.
Use it as the source of truth for licensing terms.

---

## Author

Built by **BriberixDev** as part of a long-term journey toward production-quality .NET development.

If you want to discuss architecture, improvements, or collaboration ideas, feel free to open an issue or start a discussion in the repository.
