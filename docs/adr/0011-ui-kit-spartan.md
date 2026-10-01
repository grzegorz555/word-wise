# ADR-0011: spartan/ui as the UI component kit

- Status: Accepted
- Date: 2026-10-01

## Context
The app needs ready-made, accessible components (button, card, input, dialog, toggle group) and a consistent brand, designed in Figma and mapped to code. The focus is **mobile first**. Styling is already Tailwind CSS v4 without Sass (ADR-0010).

## Considered options
1. **spartan/ui**: the Angular take on shadcn/ui. *Brain* (`@spartan-ng/brain`, npm) gives unstyled, accessible primitives (ARIA, keyboard, focus); *helm* components are Tailwind-styled code generated into our repo. Theme = CSS variables (`--primary`, `--radius`…) mapped with `@theme inline`. MIT, 1.0, supports the two latest Angular majors (22 included). Figma: any shadcn/ui kit, since component names, variants and variables match.
2. Angular Material 3: official M3 Figma kit and Theme Builder, but the theme is defined in Sass (`mat.theme`), which adds a second styling system next to Tailwind and goes against ADR-0010.
3. Headless only (Angular Aria, still in developer preview, or Angular CDK) + own Tailwind components + own Figma file: the most to learn, but every component is built from scratch.

## Decision
Option 1: **spartan/ui**. Setup with the Angular CLI generators (`ng g @spartan-ng/cli:init`, then `ng g @spartan-ng/cli:ui`), helm components in `src/web/src/app/shared/ui/`. Figma: the free **shadcncraft Free Starter Kit**, the only free shadcn kit with native Figma variables.

## Consequences
- Helm components are **our code**: we can edit them, and spartan updates are applied deliberately (re-generate and compare), not via `npm update`.
- `styles.css` switches to the layered Tailwind imports + `@spartan-ng/brain/hlm-tailwind-preset.css`, as in the spartan installation guide. The brand lives in the theme variables (`:root`, `.dark`).
- Figma variables use the same names as the CSS variables, so tokens move 1:1 (Figma MCP `get_variable_defs` → `styles.css`).
- New dependency `@angular/cdk` (overlays, a11y), required by brain.
- Mobile first is a convention, not a library feature: rules go into `docs/conventions/frontend.md`.

## Learning resources
- spartan/ui introduction (brain vs helm): https://www.spartan.ng/documentation/introduction
- Installation: https://www.spartan.ng/documentation/installation
- Theming (CSS variables, `@theme inline`): https://www.spartan.ng/documentation/theming
- shadcn/ui Figma kits: https://ui.shadcn.com/docs/figma
- Angular CDK: https://material.angular.dev/cdk/categories
- Angular Aria (for comparison): https://angular.dev/guide/aria/overview
