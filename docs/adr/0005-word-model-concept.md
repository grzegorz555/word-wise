# ADR-0005: Model słów „Concept + formy per język”

- Status: Accepted
- Data: 2026-09-30

## Kontekst
Listy top-1000 dla EN, PL i TR różnią się. Słowa bywają wieloznaczne (EN *right* = prawy / racja). Potrzebujemy dowolnej pary kierunków nauki.

## Rozważane opcje
1. **Concept + Word(Language, Text, FrequencyRank?)**: tłumaczenie = słowa o tym samym Concept.
2. Word per język + tabela par tłumaczeń: wiele-do-wielu, trzy siatki powiązań do utrzymania.
3. Płaskie karty per kierunek: duplikacja danych, 6 kierunków, postęp rozbity.

## Decyzja
Opcja 1. Szczegóły w `docs/architecture.md#model-danych-adr-0005`.
- Jedno znaczenie = jeden Concept (wieloznaczne słowo występuje w kilku Conceptach).
- Synonimy w jednym języku dopuszczalne (kilka Word z tym samym Concept i Language).
- `FrequencyRank` jest per język, `null` = spoza top-1000.
- Postęp (`UserWordProgress`) liczony per słowo docelowe, wspólny dla wszystkich języków bazowych.

## Konsekwencje
- Dodanie języka = nowe wiersze Word, bez zmian schematu.
- Słowo bez tłumaczenia w języku bazowym jest pomijane w sesji (reguła w spec 001).
- Praca na danych: przypisanie słów do Conceptów przy imporcie.

## Do nauki
- Relacje w EF Core: https://learn.microsoft.com/ef/core/modeling/relationships
- Indeksy (unikalne, złożone): https://learn.microsoft.com/ef/core/modeling/indexes
