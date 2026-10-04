# Backlog (beyond the current MVP)

- [ ] Spaced repetition algorithm (SM-2 → later FSRS); fields in `UserWordProgress` (ease, interval, due).
- [ ] "Focus on weak words" mode.
- [ ] Games: multiple choice (4 answers), typing (culture-aware comparison for TR/PL!), matching pairs.
- [ ] Full top-1000 lists for EN/PL/TR: source, license, lemmatization (Turkish is an agglutinative language).
- [x] Generating a TS client from OpenAPI: ADR-0012, set up (`npm run api`, CI check).
- [ ] Integration tests against a real Postgres (Testcontainers). Likely needed already in spec 002 (auth tests).
- [ ] Auth A3: email confirmation and password reset (`IEmailSender`). Plan: `architecture.md`, Authentication.
- [ ] Auth A4 (before deployment): HTTPS-only cookie, security headers (CSP), rate limiting on login, account deletion.
- [ ] Example sentences.
- [ ] Listen to the word: speaker button on the flashcard (lucide `volume-2`, `aria-label="Play pronunciation"`). First version: the browser Web Speech API (`speechSynthesis`, utterance `lang` = `en-US` / `pl-PL` / `tr-TR`), free and without a backend. Voices depend on the device, so hide or disable the button when there is no voice for the language. Later option: server-side TTS for consistent quality. Docs: https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesis
- [ ] Deployment to Azure (container + hosting), external sign-in. Planned as a series of specs, one milestone each: container image → CI extensions (e2e, axe, CodeQL, Dependabot) → CD to staging (OIDC, Bicep) → releases and changelog → observability → production and a rollback drill. Topics and options: `docs/learning/roadmap.md`. First step: `/compare-options` for the Azure services (Container Apps / App Service, Static Web Apps, Postgres flexible server) → ADR.
- [ ] Dark mode:
  - Theme switch (light / dark / system) in the account menu; the code already has the colors (`src/web/src/styles/tokens.css`, `:root.dark`).
  - Figma: dark variants of the screens. The Starter plan allows one variable mode only, so either upgrade (modes Light/Dark) or add a second collection `theme-dark` with the same variable names and duplicate the screens. Values: copy the hex comments from `:root.dark` in `tokens.css`.
  - Figma: fix the light `input` variable `#E4E4E7` → `#8A8A93` (WCAG 1.4.11, 3:1 for field borders).
- [ ] Language switch styled as in Figma ("WordWise — mobile proposal", node 7:26): separate buttons with a gap, border, white background, active item `primary` with white text. Helm mapping: `variant="outline"` and `spacing` on `hlm-toggle-group`, `bg-card`, `data-[state=on]:bg-primary data-[state=on]:text-primary-foreground` plus a hover variant for the active item (helm's `hover:bg-muted` would turn it grey). Decide where: classes in `app.html` (only this switch) or `hlm-toggle-group-item.ts` (every toggle group). Check the active-state contrast ≥ 3:1 (SC 1.4.11).
- [ ] Label in Name for code-only buttons (SC 2.5.3, level A): the language switch shows "TR" but its accessible name is "Türkçe", so speech-input users saying "click TR" may miss it. Proposal: `aria-label="TR, Türkçe"` (visible text first), then update `docs/conventions/frontend.md`, "Language names and tooltips". https://www.w3.org/WAI/WCAG22/Understanding/label-in-name.html
