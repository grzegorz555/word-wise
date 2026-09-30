# Spec 001: Word catalog (model, seed, reading cards)

- Status: Ready
- ADR: 0003, 0005, 0007
- User stories from `vision.md`: foundation for no. 3 and 4 (no auth: that's spec 002)

## Goal
The database contains Concepts and EN/PL/TR words from the seed, and the API returns a list of flashcards for a language pair, in order or randomly.

## Scope
1. `Concept`, `Word` entities + `WordWiseDbContext` + the first migration (Postgres).
2. Import of `data/seed/words.seed.csv` at startup in Development mode, idempotent.
3. `GET /api/words/cards?base=pl&target=tr&order=sequential|random`

## Contract
```http
GET /api/words/cards?base=pl&target=tr&order=sequential
200 OK
[
  { "wordId": 17, "text": "ev", "rank": 21, "partOfSpeech": "noun", "translations": ["dom"] },
  ...
]
```

## Rules
- R1: `base` and `target` ∈ {`en`,`pl`,`tr`}, `base ≠ target`. Otherwise **400 ValidationProblem**.
- R2: `order` defaults to `sequential` (by `rank` ascending, words without a rank are skipped). `random` returns the same cards in random order.
- R3: a target word with no translation in the base language is **not included** in the list.
- R4: a word with several meanings (e.g. EN *right*) produces **one card per Word**. Translations come from its Concept; synonyms are sorted alphabetically using the base language's culture.
- R5: re-importing the same CSV does not change the row count.

## Acceptance criteria
- [ ] `docker compose up -d` + `dotnet run` creates the schema and loads the seed.
- [ ] `GET /api/words/cards?base=pl&target=tr` returns cards sorted by `rank`.
- [ ] Test: `base=tr&target=tr` → 400.
- [ ] Test: the CSV parser correctly reads `a|b` synonyms and empty ranks.
- [ ] Test: importing 2× → the same number of words.
- [ ] Entry in `docs/learning/journal.md`.

## Out of scope
Auth, user progress (spec 002/003), pagination (≤1000 cards, not needed for now).
