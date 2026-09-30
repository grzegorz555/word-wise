# Vision: WordWise

## Problem
Learning a language starting from its most common words gives the fastest return, because about 1000 words cover a large share of everyday texts. I want a tool that walks me through these words flashcard by flashcard, in any EN/PL/TR language pair.

## User
A language learner (initially: the author). They have an account and choose the UI language, the base language and the target language.

## Project goals
1. **Learning .NET** (main goal) and a portfolio. Code quality should be "good enough to show".
2. A working MVP built in small, complete steps (each step: spec → tests → code → journal entry).

## MVP (user stories)
1. As a user, I create an account and sign in (email + password).
2. As a user, I choose the UI language (PL/EN/TR) and can change it at any time.
3. As a user, I choose a language pair (base → target, any pair from EN/PL/TR).
4. As a user, I browse target-word flashcards **in order** (by frequency) or **randomly** and reveal the translation.
5. As a user, I mark a flashcard as "I know" / "I don't know", and the app saves my progress.

## Beyond the MVP (non-goals for now) → `backlog.md`
- Spaced repetition algorithm (SM-2 / FSRS) and weighting of weaker words.
- Games / quizzes.
- Full 1000-word lists (a seed of ~50 words is enough for the MVP, see ADR-0007).
- Audio, pronunciation, example sentences.
- Public hosting, external sign-in (Google etc.).

## Success metrics
- Every MVP feature has tests (unit/integration) and passes CI.
- Every feature has an entry in `learning/journal.md`. The author can explain every layer without AI help.
