# Jak pracujemy z AI (Claude Code)

Metoda: lekki **Spec-Driven Development** (inspirowany GitHub Spec Kit) + workflow **Explore → Plan → Code → Verify** z best practices Claude Code.

## Cykl jednej funkcji
1. **Spec** (`docs/specs/NNN-nazwa/spec.md`): co i po co, kryteria akceptacji. Skill `/spec-feature`.
2. **Tasks** (`tasks.md`): małe kroki, każdy oznaczony trybem (niżej). Tryby ustala autor.
3. **Implementacja** zadanie po zadaniu, w plan mode przy większych krokach.
4. **Review:** agent `code-reviewer` (świeży kontekst) sprawdza zgodność z konwencjami.
5. **Nauka:** wpis w `docs/learning/journal.md` + `/quiz-me` co tydzień.
6. `/clear` między zadaniami: kontekst to koszt.

## Tryby zadań
| Tryb | Kto pisze kod | Rola Claude | Warunek „done” |
|---|---|---|---|
| `[solo]` | autor | review, pytania, linki do docs | autor sam przeszedł testy |
| `[learn]` | autor (kluczowe części) | szkielet + `TODO(human)`, podpowiedzi zamiast rozwiązań | autor wypełnił TODO, testy zielone |
| `[gen]` | Claude | generuje + wyjaśnia | **explain-back**: autor opisuje kod własnymi słowami |

Proporcja startowa: ok. 1/3 każdego trybu. Boilerplate i konfiguracja są `[gen]`, logika domeny i zapytania EF są `[learn]`/`[solo]`.

## Narzędzia w repo
- Output style **Mentor** (`.claude/output-styles/mentor.md`) jest włączony domyślnie w `.claude/settings.json`. Zmiana: `/output-style`.
- Agenci (`.claude/agents/`): `backend-mentor` (tłumaczy, prowadzi sokratejsko), `code-reviewer` (ocenia kod wg konwencji).
- Skille (`.claude/skills/`): `/spec-feature`, `/new-adr`, `/explain-this`, `/quiz-me`.
- MCP (`.mcp.json`): `microsoft-learn` (oficjalne docs .NET) i `angular-cli` (docs + best practices Angulara). Źródło linków w wyjaśnieniach.

## Samodyscyplina (challenge)
- **Predict before run:** zanim uruchomisz, zapisz, co się stanie.
- **Explain-back** po każdym `[gen]`.
- **Najpierw testy** w `[learn]`/`[solo]`.
- **Zakaz commita bez zrozumienia** każdej linii diffu.
- Raz w tygodniu `/quiz-me` + przegląd dziennika.
- Poza projektem: ścieżka C# na Exercism, kod referencyjny `dotnet/eShop` (czytanie i porównywanie).

## Oszczędność tokenów
- CLAUDE.md krótkie, szczegóły w `docs/`, ładowane tylko gdy potrzebne.
- Jedna spec = jeden folder. ADR zamiast długich dyskusji w czacie.
- Przeszukiwanie większego kodu zlecaj subagentom (Explore), żeby nie zaśmiecać kontekstu.
