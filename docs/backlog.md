# Backlog (beyond the current MVP)

- [ ] Spaced repetition algorithm (SM-2 → later FSRS); fields in `UserWordProgress` (ease, interval, due).
- [ ] "Focus on weak words" mode.
- [ ] Games: multiple choice (4 answers), typing (culture-aware comparison for TR/PL!), matching pairs.
- [ ] Full top-1000 lists for EN/PL/TR: source, license, lemmatization (Turkish is an agglutinative language).
- [ ] Generating a TS client from OpenAPI (e.g. openapi-typescript / NSwag).
- [ ] Integration tests against a real Postgres (Testcontainers). Likely needed already in spec 002 (auth tests).
- [ ] Auth A3: email confirmation and password reset (`IEmailSender`). Plan: `architecture.md`, Authentication.
- [ ] Auth A4 (before deployment): HTTPS-only cookie, CSRF review, rate limiting on login, account deletion.
- [ ] Example sentences, audio (TTS).
- [ ] Deployment (container + hosting), external sign-in.
