# Frontend conventions (Angular 22)

Source: Angular Style Guide: https://angular.dev/style-guide and best practices from the Angular CLI MCP.

- Standalone, signals (`signal`, `computed`, `input()`, `output()`), zoneless, `@if/@for/@switch`.
- `inject()` instead of the constructor. `ChangeDetectionStrategy.OnPush` is the default in zoneless, so keep state in signals.
- Files: `name.ts`, `name.html`, `name.css` (only if needed), `name.spec.ts` (Angular 20+ convention without the `.component` suffix).
- Folders: `core/` (singletons: i18n, auth, http), `features/<feature>/` (pages + feature components), `shared/` (presentational components).
- All texts go through Transloco (`| transloco`), keys `feature.element` (`study.revealButton`), added in `pl`, `en`, `tr`.
- HTTP: services in `features/<feature>/data-access/` wrap the client generated from OpenAPI in `core/api/` (ADR-0012, `npm run api:gen`). Never edit generated files and never write DTOs by hand.
- UI components: spartan/ui helm components in `shared/ui/` (ADR-0011). Use them before building your own; edit a helm component only on purpose, since it is our code.
- Accessibility: buttons are `<button>`, toggle state in `aria-pressed`, labels for navigation.

## Styling: Tailwind CSS v4 (ADR-0010)
- Utilities in the template first. A component stylesheet only for things utilities can't express nicely (e.g. a 3D card flip animation); there, use theme variables `var(--color-…)` instead of `@apply`.
- **No dynamically built class names** (`'bg-' + color`): Tailwind scans files as plain text and only sees complete names. Map state to full class names instead.
- State, in this order of preference:
  1. A state attribute + variant: if the element already has `[attr.aria-pressed]`, `aria-expanded`, `aria-selected` etc., style it with `aria-pressed:bg-slate-800`. One source of truth for accessibility and looks. For non-ARIA state: `[attr.data-active]="active() || null"` + `data-active:…` (`null` removes the attribute; `false` would render `data-active="false"`, which the variant still matches because it only checks that the attribute exists).
  2. `[class.bg-slate-800]="active()"`: **one class per binding**.
  3. `[class]` with an object: **one class per key**. Angular class bindings do not support space-separated class names in a single key (https://angular.dev/guide/templates/binding); only `ngClass` does.
  Long conditional lists go into a `computed` in the component.
- Host element: classes for the component's own element go into `host: { class: '…' }` in `@Component`, not `:host` in a stylesheet (https://angular.dev/guide/components/host-elements). Remember `block`: custom elements are inline by default.
- Theme tokens (brand colors, fonts) only in `@theme` in `src/styles.css`, so colors are not hardcoded in templates as `[#123456]`.
- Responsive: mobile first, add `sm:`/`md:` prefixes for bigger screens. Accessibility: visible focus (`focus-visible:ring-2`), don't communicate state with color alone.
- Repeated class sets (e.g. a button) → a small shared component in `shared/`, not copy-paste and not `@apply`.
- Docs: https://tailwindcss.com/docs/styling-with-utility-classes

## Responsive design: mobile first
- Base styles target a phone; `sm:`/`md:`/`lg:` only extend the layout for bigger screens (https://tailwindcss.com/docs/responsive-design). Design and review the 390 px width first.
- Touch targets for the main actions (flashcard, "I know / I don't know", language switch) are at least 44×44 px.
- Full-height screens use `min-h-dvh`, not `h-screen` (the mobile address bar changes the viewport height). Respect `env(safe-area-inset-*)` on edge-to-edge layouts.
- Primary actions sit within thumb reach (bottom of the screen). Gestures such as swipe are an extra, never the only way to do something.
- Check every view in DevTools device mode at 360, 390, 768 and 1280 px before a PR.

## Language names and tooltips
- **Learning-language pickers** ("I know", "I'm learning") show full names in the current UI language: English, Polish, Turkish (Transloco keys `languages.en`, `languages.pl`, `languages.tr`).
- **UI-language switch** shows each language in its own name (endonym): English, Polski, Türkçe, so people can find their language even if they can't read the current one. Endonyms are constants, not translated.
- **Codes (EN / PL / TR) only where space is tight**, e.g. the header switch on a phone. A button with only a code gets an `aria-label` with the full name and a tooltip (`hlmTooltip`, https://www.spartan.ng/components/tooltip).
- Tooltips appear on hover and keyboard focus only; touch screens have no hover. A tooltip is therefore never the only place where information lives (https://www.w3.org/WAI/ARIA/apg/patterns/tooltip/).
