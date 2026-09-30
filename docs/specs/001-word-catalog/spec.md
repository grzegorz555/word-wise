# Spec 001: Katalog słów (model, seed, odczyt kart)

- Status: Ready
- ADR: 0003, 0005, 0007
- User stories z `vision.md`: fundament pod nr 3 i 4 (bez auth: to spec 002)

## Cel
Baza zawiera Concepty i słowa EN/PL/TR z seeda, a API zwraca listę fiszek dla pary języków po kolei lub losowo.

## Zakres
1. Encje `Concept`, `Word` + `WordWiseDbContext` + pierwsza migracja (Postgres).
2. Import `data/seed/words.seed.csv` przy starcie w trybie Development, idempotentny.
3. `GET /api/words/cards?base=pl&target=tr&order=sequential|random`

## Kontrakt
```http
GET /api/words/cards?base=pl&target=tr&order=sequential
200 OK
[
  { "wordId": 17, "text": "ev", "rank": 21, "partOfSpeech": "noun", "translations": ["dom"] },
  ...
]
```

## Reguły
- R1: `base` i `target` ∈ {`en`,`pl`,`tr`}, `base ≠ target`. W przeciwnym razie **400 ValidationProblem**.
- R2: `order` domyślnie `sequential` (po `rank` rosnąco, słowa bez ranku pomijane). `random` zwraca te same karty w losowej kolejności.
- R3: słowo docelowe bez żadnego tłumaczenia w języku bazowym **nie trafia** do listy.
- R4: słowo z kilkoma znaczeniami (np. EN *right*) daje **jedną kartę na Word**. Tłumaczenia z jego Conceptu, synonimy w kolejności alfabetycznej wg kultury języka bazowego.
- R5: ponowny import tego samego CSV nie zmienia liczby wierszy.

## Kryteria akceptacji
- [ ] `docker compose up -d` + `dotnet run` tworzy schemat i wczytuje seed.
- [ ] `GET /api/words/cards?base=pl&target=tr` zwraca karty posortowane po `rank`.
- [ ] Test: `base=tr&target=tr` → 400.
- [ ] Test: parser CSV poprawnie czyta synonimy `a|b` i puste rangi.
- [ ] Test: import 2× → ta sama liczba słów.
- [ ] Wpis w `docs/learning/journal.md`.

## Poza zakresem
Auth, postęp użytkownika (spec 002/003), paginacja (≤1000 kart, na razie niepotrzebna).
