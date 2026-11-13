# Spec 003: Study session (flashcards, "I know / I don't know", progress)

- Status: **Draft** (the author approves the task modes before work starts)
- ADR: 0004 (cookie auth), 0005 (progress per target word), 0006 (languages), 0013 (integration test database)
- Depends on: spec 001 (card query), spec 002 (sign-in, language pair in the profile; auth phase **A2**)
- User stories from `vision.md`: 4 (browse flashcards in order or randomly, reveal the translation), 5 ("I know / I don't know", progress is saved)
- Design: Figma "WordWise — mobile proposal" (`https://www.figma.com/design/POPMX4N2JceYUnbwbYebRU`), screens 02 (language pair), 03/04 (flashcard hidden / revealed), D02–D04 (desktop)

## Goal
A signed-in user starts a session of N cards for their language pair, reveals each translation, answers "I know" or "I don't know", and the answers are saved. Words whose latest answer is "I know" don't come back in later sessions.

## Scope
1. `UserWordProgress` entity (per user and target word) + migration.
2. API: cards for a session, save an answer, an overview for the session setup screen.
3. Session setup on the language pair screen (Figma 02): order and number of cards, "Start".
4. Flashcard screen (Figma 03/04) and a summary at the end of the session.
5. Resuming an unfinished session after a reload (per browser tab).

## Contract
```http
GET  /api/study/overview                          → 200 { "baseLanguage": "en", "targetLanguage": "tr", "totalWords": 50, "knownWords": 13 } | 400 (no pair)
GET  /api/study/cards?order=sequential&count=20   → 200 [ WordCardResponse, ... ] (spec 001 shape) | 400
POST /api/study/answers  { "wordId": 17, "known": true }   → 204 | 400 | 404 (no such study word)
```

## Rules
- R1 (A2): every study endpoint requires sign-in; the user ID comes from the cookie, never from the request. The language pair comes from the profile (spec 002); without a pair → **400 ProblemDetails**.
- R2: `GET /api/study/cards` returns up to `count` cards (1–1000; omitted = all) from the target words of the pair that have a rank (spec 001, R2–R4) **and whose latest answer is not "I know"**. `sequential` = by rank ascending (so a new session continues where the user stopped); `random` = a random selection of `count` from those words, in random order.
- R3: `POST /api/study/answers` creates or updates the user's `UserWordProgress` for that word: `TimesSeen + 1`, `TimesKnown + 1` if known, `LastAnswerKnown = known`, `LastSeenAt` = now (from `TimeProvider`, so tests control time). One row per (user, word), guaranteed by a unique index.
- R4: progress is per target word and shared across base languages (ADR-0005): a word known in EN → TR is also skipped in PL → TR.
- R5: answers are not idempotent in this version: the SPA disables both buttons until the answer is saved. (Roadmap §2 "idempotency" is the topic to revisit.)
- R6: the session setup (Figma 02) offers the order (sequential / random) and the number of cards (10 / 20 / 50 / all); the label shows how many words are left to learn ("37 words · EN → TR"). Order and count are not saved in the profile.
- R7: when no words are left, the setup screen says so (all words of the pair are known) instead of starting an empty session.
- R8: resume: the session (pair, card list, position) lives in `sessionStorage` of the tab; a reload continues at the same card. It is cleared at the end of the session, on sign-out and when the pair changes. No personal data is stored there.
- R9: the end of a session shows a summary ("You knew X of N") with "Next session" and "Change language pair".
- R10: accessibility (WCAG 2.2 AA): the word and the translation carry `lang` (`tr`, `en`, `pl`; SC 3.1.2); revealing the translation is announced (live region) and moves focus predictably; the progress is text ("12 / 20") plus a progress bar with an accessible name; "I know" / "I don't know" are labelled by text, not by colour only; mobile-first layout (`docs/conventions/frontend.md`).

## Acceptance criteria
- [ ] Migration `AddUserWordProgress` with the unique index (UserId, WordId); deleting a user deletes their progress.
- [ ] Integration test (ADR-0013): answer "I know" for a word → it is not in the next `GET /api/study/cards`; "I don't know" → it still is.
- [ ] Integration test: the same answer for EN → TR also hides the word in PL → TR (R4).
- [ ] Integration test: study endpoints without a cookie → 401; another user's progress is never read or written.
- [ ] Unit/integration tests: `count` and `order` (sequential continues by rank; random returns `count` distinct cards).
- [ ] Vitest: the session service (next card, answer, end, resume from `sessionStorage`, cleared on sign-out); the flashcard component (reveal, buttons disabled while saving).
- [ ] Manual check with the keyboard and a screen reader on the flashcard screen.
- [ ] Entry in `docs/learning/journal.md`.

## Out of scope
Keyboard shortcuts (separate spec; see the note in `docs/backlog.md` about Figma vs the backlog plan), spaced repetition (SM-2), "focus on weak words", reviewing known words again, listening to the word, games, dark mode.
