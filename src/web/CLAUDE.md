# Frontend (Angular 22): context for Claude

Full conventions: `docs/conventions/frontend.md`. The author knows Angular from work, so focus explanations on the backend and FE↔BE integration.

- Standalone components, signals, zoneless, the new control flow syntax (`@if`, `@for`).
- Styling: Tailwind CSS v4 utilities in templates (ADR-0010); global CSS in `src/styles.css`. The author is learning Tailwind: explain non-obvious utilities and link the Tailwind docs.
- `inject()` instead of constructor injection. State in `signal`/`computed`. RxJS only for HTTP streams and events.
- Structure: `src/app/core/` (i18n, http, auth), `src/app/features/<feature>/` (e.g. `study/`).
- i18n: Transloco, files `public/i18n/{pl,en,tr}.json`. Every UI text goes through a key, no hardcoded strings. Add each new key to all 3 files.
- API: calls via `/api/...` (dev proxy → `http://localhost:5033`). TS-side DTOs must match the OpenAPI contract from `/openapi/v1.json`.
- Tests: Vitest (`npx ng test --watch=false`). Formatting: Prettier (`npx prettier --check "src/**/*.{ts,html,scss,css}"`).
