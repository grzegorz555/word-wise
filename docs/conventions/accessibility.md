# Accessibility conventions (WCAG 2.2 AA)

Target: **WCAG 2.2 level AA** for every screen (https://www.w3.org/TR/WCAG22/, quick reference: https://www.w3.org/WAI/WCAG22/quickref/). Accessibility is part of "done", not a later phase. Angular guide: https://angular.dev/best-practices/a11y.

## Perceivable
- **Text contrast** at least 4.5:1, large text (≥ 24 px, or ≥ 18.66 px bold) at least 3:1 (SC 1.4.3). Check new theme colors before adding them to `src/styles/tokens.css`, in both light and dark mode.
- **Non-text contrast** (input borders, focus ring, icons that carry meaning, toggle state) at least 3:1 against the background (SC 1.4.11).
- **Never color alone** (SC 1.4.1): "I know / I don't know" have a text label, the selected toggle also has `aria-pressed`/`aria-checked`.
- Text resizes to 200% and reflows at 320 px width without horizontal scrolling (SC 1.4.4, 1.4.10). Use `rem`, never fixed heights on text containers.
- Icons: decorative → `aria-hidden="true"`; icon-only buttons → `aria-label` (e.g. the account button, the speaker button).
- `lang` on `<html>` follows the UI language; a word in another language (the flashcard word) gets its own `lang` attribute, so screen readers pronounce it correctly (SC 3.1.1, 3.1.2).

## Operable
- **Everything works with a keyboard** (SC 2.1.1), in a logical order (SC 2.4.3), with no keyboard traps (SC 2.1.2).
- **Visible focus** (SC 2.4.7) that is not hidden behind sticky headers or menus (SC 2.4.11): `focus-visible:ring-2` with the `ring` color.
- **Touch targets** at least 24×24 px (SC 2.5.8, AA). Our own rule is stricter: 44×44 px for the main actions (see `frontend.md`, mobile first).
- Keyboard shortcuts on the flashcard (Space, ←, →) are single keys, so they must only work while the flashcard has focus, or be possible to turn off (SC 2.1.4).
- Gestures such as swipe always have a button alternative (SC 2.5.1). Motion and animation respect `prefers-reduced-motion` (`motion-safe:` / `motion-reduce:` in Tailwind).
- Each route sets a page title (`title` in the route config, translated) (SC 2.4.2).

## Understandable
- **Forms**: every field has a visible `<label>` (SC 3.3.2); errors are shown in text next to the field and linked with `aria-describedby` (SC 3.3.1); use `autocomplete="email"`, `"current-password"`, `"new-password"`.
- **Accessible authentication** (SC 3.3.8): allow paste and password managers in password fields, no puzzles or CAPTCHA that require memory.
- Consistent navigation and naming across screens (SC 3.2.3, 3.2.4).

## Robust
- Native elements first (`<button>`, `<a>`, `<input>`), ARIA only when there is no native element. spartan brain primitives provide the ARIA patterns for menus, toggle groups and dialogs.
- Status messages (e.g. "Progress saved") use a live region (`aria-live="polite"`) (SC 4.1.3).

## How we check
- Keyboard-only pass of every new screen (Tab, Shift+Tab, Enter, Space, Esc, arrows).
- Browser DevTools: Lighthouse accessibility audit and the contrast checker; zoom to 200%; device mode at 360 px.
- A screen reader pass for new components (NVDA on Windows).
- Automated checks catch only part of the problems; the manual pass is required.

## Resources
- WCAG 2.2: https://www.w3.org/TR/WCAG22/ · Quick reference: https://www.w3.org/WAI/WCAG22/quickref/
- ARIA Authoring Practices (patterns): https://www.w3.org/WAI/ARIA/apg/
- Angular accessibility: https://angular.dev/best-practices/a11y
