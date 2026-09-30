# Seed słów (ADR-0007)

`words.seed.csv`: UTF-8, separator `;`, synonimy rozdzielone `|`. Jeden wiersz = jedno znaczenie (Concept).

| Kolumna | Opis |
|---|---|
| `concept_key` | unikalny klucz znaczenia (snake_case, EN), np. `right_direction` |
| `part_of_speech` | `noun`, `verb`, `adjective`, `adverb`, `pronoun`, `conjunction`, `interjection` |
| `en`, `pl`, `tr` | forma(y) słownikowa(e) w danym języku |
| `en_rank`, `pl_rank`, `tr_rank` | pozycja na liście frekwencyjnej danego języka (puste = spoza top-1000) |

**Uwaga: rangi w tym pliku są orientacyjne** (kolejność w pliku), a nie prawdziwe dane frekwencyjne. Tłumaczenia przygotowało AI. Warto je zweryfikować, zwłaszcza TR.

Przykłady wieloznaczności celowo zawarte w seedzie:
- `old_thing` / `old_age`: EN *old* i PL *stary* w dwóch znaczeniach, TR rozróżnia *eski* / *yaşlı*. To samo słowo EN/PL ma ten sam rank w obu wierszach.
- `right_direction` / `right_correct`: EN *right* → *prawy* / *słuszny*, TR *sağ* / *doğru*.
