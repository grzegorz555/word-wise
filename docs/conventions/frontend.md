# Frontend conventions (Angular 22)

Source: Angular Style Guide: https://angular.dev/style-guide and best practices from the Angular CLI MCP.

- Standalone, signals (`signal`, `computed`, `input()`, `output()`), zoneless, `@if/@for/@switch`.
- `inject()` instead of the constructor. `ChangeDetectionStrategy.OnPush` is the default in zoneless, so keep state in signals.
- Files: `name.ts`, `name.html`, `name.css` (only if needed), `name.spec.ts` (Angular 20+ convention without the `.component` suffix).
- Folders: `core/` (singletons: i18n, auth, http), `features/<feature>/` (pages + feature components), `shared/` (presentational components).
- All texts go through Transloco (`| transloco`), keys `feature.element` (`study.revealButton`), added in `pl`, `en`, `tr`.
- HTTP: services in `features/<feature>/data-access/`, DTO types match the OpenAPI contract.
- Accessibility: buttons are `<button>`, toggle state in `aria-pressed`, labels for navigation.

## Styling: Tailwind CSS v4 (ADR-0010)
- Utilities in the template first. A component stylesheet only for things utilities can't express nicely (e.g. a 3D card flip animation); there, use theme variables `var(--color-…)` instead of `@apply`.
- **No dynamically built class names** (`'bg-' + color`): Tailwind scans files as plain text and only sees complete names. Map state to full class names instead.
- State: `[class.bg-slate-800]="active()"` for one class; for several, an object binding `[class]="{ 'bg-slate-800 text-white': active() }"`. Long conditional lists go into a `computed` in the component.
- Theme tokens (brand colors, fonts) only in `@theme` in `src/styles.css`, so colors are not hardcoded in templates as `[#123456]`.
- Responsive: mobile first, add `sm:`/`md:` prefixes for bigger screens. Accessibility: visible focus (`focus-visible:ring-2`), don't communicate state with color alone.
- Repeated class sets (e.g. a button) → a small shared component in `shared/`, not copy-paste and not `@apply`.
- Docs: https://tailwindcss.com/docs/styling-with-utility-classes
