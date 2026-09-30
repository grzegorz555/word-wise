# ADR-0007: Dane słów: seed ~50 słów + import CSV

- Status: Accepted
- Data: 2026-09-30

## Kontekst
Pełne listy top-1000 wymagają researchu (licencje, np. CC BY-SA), lematyzacji (turecki zapisuje odmiany jako osobne słowa) i tłumaczeń. To osobne zadanie, które nie powinno blokować budowy aplikacji.

## Decyzja
- Plik `data/seed/words.seed.csv` z ~50 znaczeniami. Separator `;`, UTF-8. Synonimy rozdzielone `|`.
- Kolumny: `concept_key;part_of_speech;en;en_rank;pl;pl_rank;tr;tr_rank`.
- **Rangi w seedzie są orientacyjne (porządek w pliku)**, a nie prawdziwe dane frekwencyjne. Zastąpi je import pełnych list.
- Import jest idempotentny (ponowne uruchomienie nie duplikuje danych), więc ten sam kod posłuży potem do pełnych list.

## Konsekwencje
- Źródło pełnych danych i jego licencję opiszemy w nowym ADR, zanim trafią do publicznego repo.

## Do nauki
- Seeding w EF Core: https://learn.microsoft.com/ef/core/modeling/data-seeding
