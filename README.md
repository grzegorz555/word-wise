# WordWise

Fiszki z 1000 najczęstszych słów języka angielskiego, polskiego i tureckiego. Interfejs jest dostępny w tych samych trzech językach.
Projekt edukacyjny: ASP.NET Core 10 + Angular 22 + PostgreSQL.

## Szybki start
Pełna instrukcja: **[docs/how-to/run-locally.md](docs/how-to/run-locally.md)**.
```bash
cp .env.example .env            # opcjonalnie: własne hasło/port do Postgresa
docker compose up -d            # PostgreSQL
dotnet run --project src/api/WordWise.Api --launch-profile http   # API: http://localhost:5033, Swagger: /swagger
cd src/web && npm ci && npm start                                  # Web: http://localhost:4200
```

Testy: `dotnet test --solution WordWise.slnx` oraz `cd src/web && npx ng test --watch=false`.

## Dokumentacja
- [Wizja i MVP](docs/vision.md) · [Architektura](docs/architecture.md) · [Decyzje (ADR)](docs/adr/)
- [Konwencje](docs/conventions/) · [Specyfikacje funkcji](docs/specs/) · [Backlog](docs/backlog.md)
- [Jak pracujemy z AI](docs/conventions/ai-workflow.md) · [Dziennik nauki](docs/learning/journal.md) · [Zakładki do dokumentacji](docs/learning/references.md)
