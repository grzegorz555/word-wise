# Frontend (Angular 22): kontekst dla Claude

Pełne konwencje: `docs/conventions/frontend.md`. Autor zna Angulara z pracy, więc wyjaśnienia skupiaj na backendzie i integracji FE↔BE.

- Standalone components, signals, zoneless, nowa składnia kontroli przepływu (`@if`, `@for`).
- `inject()` zamiast wstrzykiwania przez konstruktor. Stan w `signal`/`computed`. RxJS tylko dla strumieni HTTP i eventów.
- Struktura: `src/app/core/` (i18n, http, auth), `src/app/features/<feature>/` (np. `study/`).
- i18n: Transloco, pliki `public/i18n/{pl,en,tr}.json`. Każdy tekst UI przez klucz, bez hardkodowanych napisów. Nowy klucz dodajesz we wszystkich 3 plikach.
- API: wywołania przez `/api/...` (proxy dev → `http://localhost:5033`). DTO po stronie TS mają odpowiadać kontraktowi OpenAPI z `/openapi/v1.json`.
- Testy: Vitest (`npx ng test --watch=false`). Formatowanie: Prettier (`npx prettier --check "src/**/*.{ts,html,scss}"`).
