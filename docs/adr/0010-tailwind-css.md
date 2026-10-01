# ADR-0010: Tailwind CSS for frontend styling

- Status: Accepted
- Date: 2026-10-01

## Context
The frontend needs a consistent look (flashcards, forms, navigation) without spending much time on hand-written CSS. The author also wants to **learn the utility-first approach**, so the choice is partly a learning goal. Until now components used plain SCSS files.

## Considered options
1. **Tailwind CSS v4 (utility-first)**: styling via classes in templates, design tokens (colors, spacing) from one theme, small CSS output (only used classes). Cons: long `class` attributes, a new vocabulary to learn, it is not designed to work with Sass.
2. Hand-written SCSS per component (current state): full control and nothing new to learn, but every component reinvents spacing, colors and breakpoints.
3. A component library (Angular Material, PrimeNG): ready-made widgets, but a heavy visual identity and little to learn about CSS itself.

## Decision
Option 1: **Tailwind CSS v4**, integrated through PostCSS, as described in the Angular guide (manual setup, equivalent to `ng add tailwindcss`):
- packages `tailwindcss`, `@tailwindcss/postcss`, `postcss`; config in `src/web/.postcssrc.json`;
- global styles in **`src/styles.css` (plain CSS, not SCSS)** with `@import 'tailwindcss';`, because Tailwind v4 "is not designed to be used with CSS preprocessors like Sass". Native CSS already has nesting and variables;
- theme customization (colors, fonts) in CSS via `@theme` in `styles.css`; there is no `tailwind.config.js` in v4;
- new components get `.css` style files (`angular.json` schematics), and usually none at all: utilities go in the template.

## Consequences
- Rules for templates are in `docs/conventions/frontend.md` (no dynamically built class names, `[class.x]` for state, when a component stylesheet is still OK).
- When a component stylesheet needs theme values, use CSS variables (`var(--color-sky-600)`) instead of `@apply`. If `@apply` is really needed, the file must start with `@reference` to the global stylesheet.
- `src/app/app.scss` stays for now. Rewriting the header into utilities and deleting the file is the first Tailwind learning task (`[learn]`).
- Prettier checks `.css` files too (CI and `src/web/CLAUDE.md`).
- Prettier plugins (added 2026-10-01): `prettier-plugin-tailwindcss` sorts classes in the recommended order, `prettier-plugin-classnames` wraps long `class` values at `printWidth`, `prettier-plugin-merge` lets both run on the same attribute (order in `.prettierrc` matters). Unknown classes (typos, missing utility prefix) are sorted to the front, which makes them easy to spot.
- Possible later: dark mode via the `dark:` variant.

## Learning resources
- Tailwind in Angular: https://angular.dev/guide/tailwind
- Utility-first approach (start here): https://tailwindcss.com/docs/styling-with-utility-classes
- Class detection (why no dynamic class names): https://tailwindcss.com/docs/detecting-classes-in-source-files
- Theme variables (`@theme`): https://tailwindcss.com/docs/theme
- Sass and component styles: https://tailwindcss.com/docs/compatibility
