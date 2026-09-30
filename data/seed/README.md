# Word seed (ADR-0007)

`words.seed.csv`: UTF-8, separator `;`, synonyms separated by `|`. One row = one meaning (Concept).

| Column | Description |
|---|---|
| `concept_key` | unique meaning key (snake_case, EN), e.g. `right_direction` |
| `part_of_speech` | `noun`, `verb`, `adjective`, `adverb`, `pronoun`, `conjunction`, `interjection` |
| `en`, `pl`, `tr` | dictionary form(s) in the given language |
| `en_rank`, `pl_rank`, `tr_rank` | position on the frequency list of the given language (empty = outside the top 1000) |

**Note: ranks in this file are approximate** (order in the file), not real frequency data. The translations were prepared by AI. They are worth verifying, especially TR.

Ambiguity examples intentionally included in the seed:
- `old_thing` / `old_age`: EN *old* and PL *stary* in two meanings, TR distinguishes *eski* / *yaşlı*. The same EN/PL word has the same rank in both rows.
- `right_direction` / `right_correct`: EN *right* → *prawy* / *słuszny*, TR *sağ* / *doğru*.
