# WordWise

Flashcards for the 1000 most common words in English, Polish and Turkish. The interface is available in the same three languages.
A learning project built with ASP.NET Core 10 + Angular 22 + PostgreSQL.

## Quick start
Full guide: **[docs/how-to/run-locally.md](docs/how-to/run-locally.md)**.
```bash
cp .env.example .env            # optional: custom Postgres password/port
docker compose up -d            # PostgreSQL (one shared container for every worktree)
dotnet tool restore             # local tools, including dotnet-ef
dotnet ef database update --project src/api/WordWise.Api         # apply EF Core migrations (again after pulling new ones)
dotnet run --project src/api/WordWise.Api --launch-profile http   # API: http://localhost:5033, Swagger: /swagger
cd src/web && npm ci && npm start                                  # Web: http://localhost:4200
```

Tests: `dotnet test --solution WordWise.slnx` and `cd src/web && npx ng test --watch=false`.

## Documentation
- [Vision and MVP](docs/vision.md) · [Architecture](docs/architecture.md) · [Decisions (ADR)](docs/adr/)
- [Conventions](docs/conventions/) · [Feature specs](docs/specs/) · [Backlog](docs/backlog.md)
- [How we work with AI](docs/conventions/ai-workflow.md) · [Learning journal](docs/learning/journal.md) · [Documentation bookmarks](docs/learning/references.md)
