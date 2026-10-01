# Backlog (beyond the current MVP)

- [ ] Spaced repetition algorithm (SM-2 → later FSRS); fields in `UserWordProgress` (ease, interval, due).
- [ ] "Focus on weak words" mode.
- [ ] Games: multiple choice (4 answers), typing (culture-aware comparison for TR/PL!), matching pairs.
- [ ] Full top-1000 lists for EN/PL/TR: source, license, lemmatization (Turkish is an agglutinative language).
- [ ] Generating a TS client from OpenAPI: decided in ADR-0012 (ng-openapi-gen), setup pending.
- [ ] Integration tests against a real Postgres (Testcontainers). Likely needed already in spec 002 (auth tests).
- [ ] Auth A3: email confirmation and password reset (`IEmailSender`). Plan: `architecture.md`, Authentication.
- [ ] Auth A4 (before deployment): HTTPS-only cookie, CSRF review, rate limiting on login, account deletion.
- [ ] Example sentences.
- [ ] Listen to the word: speaker button on the flashcard (lucide `volume-2`, `aria-label="Play pronunciation"`). First version: the browser Web Speech API (`speechSynthesis`, utterance `lang` = `en-US` / `pl-PL` / `tr-TR`), free and without a backend. Voices depend on the device, so hide or disable the button when there is no voice for the language. Later option: server-side TTS for consistent quality. Docs: https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesis
- [ ] Deployment (container + hosting), external sign-in.
