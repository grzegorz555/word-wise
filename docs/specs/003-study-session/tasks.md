# Tasks: Spec 003, study session

Modes: `[solo]` you write it yourself · `[learn]` skeleton + TODO(human) · `[gen]` Claude generates, you do an explain-back.
**The modes below are a proposal.** Approve or change them before the first task. Start after spec 002 (sign-in and the language pair) is done.
Proposed split: 3 × `[learn]`, 2 × `[solo]`, 3 × `[gen]`.

- [ ] **T1 `[learn]`** `UserWordProgress` entity + `IEntityTypeConfiguration` (unique index (UserId, WordId), FKs to `AspNetUsers` and `Words`, cascade delete with the user) + migration `AddUserWordProgress`.
  - *You write:* properties and the configuration (like spec 001 T2). *Claude:* file skeletons.
  - *Predict before run:* the table, the indexes and what `ON DELETE` does for each FK.
- [ ] **T2 `[learn]`** `POST /api/study/answers` (R3, R5): create or update the progress row; `TimeProvider` for `LastSeenAt`.
  - *You write:* the create-or-update logic and its integration tests first (ADR-0013). *Claude:* controller, request DTO, validation, `TimeProvider` registration.
  - *To understand:* change tracking for updates, what a unique-index violation looks like when two requests race, `TimeProvider` / `FakeTimeProvider` in tests.
- [ ] **T3 `[solo]`** `GET /api/study/cards` (R2, R4): extend your spec 001 T6 query with "latest answer is not I know" for the signed-in user, `order`, `count`; tests first.
- [ ] **T4 `[gen]`** `GET /api/study/overview` (R6, R7), OpenAPI document + Angular client, `.http` examples, the A2 rule (R1) checked by tests.
- [ ] **T5 `[gen]`** Session setup on the language pair screen (Figma 02, built in spec 002 T9): order, count (10 / 20 / 50 / all), "N words · EN → TR", "Start"; `StudySessionService` (signals) with `sessionStorage` (R8).
- [ ] **T6 `[learn]`** Flashcard screen (Figma 03/04, R10): reveal, "I know" / "I don't know", progress, buttons disabled while saving.
  - *You write:* the accessibility parts (`lang` on the word and the translation, live region, focus after reveal and after an answer). *Claude:* component skeleton, layout and styles from Figma.
- [ ] **T7 `[solo]`** Session summary (R9) and the resume/clear rules (R8: reload, sign-out, pair change), Vitest.
- [ ] **T8 `[gen]`** Review with the `code-reviewer` agent, journal entry, `/quiz-me` on T1–T7.
