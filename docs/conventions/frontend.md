# Frontend conventions (Angular 22)

Source: Angular Style Guide: https://angular.dev/style-guide and best practices from the Angular CLI MCP.

- Standalone, signals (`signal`, `computed`, `input()`, `output()`), zoneless, `@if/@for/@switch`.
- `inject()` instead of the constructor. `ChangeDetectionStrategy.OnPush` is the default in zoneless, so keep state in signals.
- Files: `name.ts`, `name.html`, `name.scss`, `name.spec.ts` (Angular 20+ convention without the `.component` suffix).
- Folders: `core/` (singletons: i18n, auth, http), `features/<feature>/` (pages + feature components), `shared/` (presentational components).
- All texts go through Transloco (`| transloco`), keys `feature.element` (`study.revealButton`), added in `pl`, `en`, `tr`.
- HTTP: services in `features/<feature>/data-access/`, DTO types match the OpenAPI contract.
- Accessibility: buttons are `<button>`, toggle state in `aria-pressed`, labels for navigation.
