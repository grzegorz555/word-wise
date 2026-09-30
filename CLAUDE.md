# WordWise — instrukcje dla Claude

> **Izolacja:** ten projekt jest całkowicie odseparowany od innych repozytoriów autora. Metodyki, konwencje, dane i decyzje obowiązują **tylko tutaj**. Nie przenoś ich do innych projektów i nie przenoś tu reguł z innych projektów (poza ogólnymi zasadami bezpieczeństwa).
> Narzędzia służbowe są tu wyłączone: skill `kup-archiwum` (deny w `.claude/settings.json`) i globalne hooki gita (`git config --local core.hooksPath .git/hooks`). Nie archiwizuj niczego z tego repo do SVN KUP.

Fiszki z 1000 najczęstszych słów w językach **EN / PL / TR** (każdy jest też językiem UI).
Projekt do **nauki + portfolio**. Autor uczy się C#/.NET (kurs freeCodeCamp „Foundational C# with Microsoft”), a Angulara zna z pracy.
**Priorytet: autor ma zrozumieć kod, a nie tylko go dostać.**

## Stack
- API: ASP.NET Core 10 (LTS), kontrolery + vertical slices → `src/api/WordWise.Api` (szczegóły: `src/api/CLAUDE.md`)
- Web: Angular 22 (standalone, signals, zoneless), Transloco → `src/web` (szczegóły: `src/web/CLAUDE.md`)
- DB: PostgreSQL 18 (`compose.yaml`), EF Core, ASP.NET Core Identity
- Testy: xUnit v3 + Shouldly (Microsoft.Testing.Platform), Vitest na froncie
- CI: GitHub Actions (`.github/workflows/ci.yml`)

## Uruchomienie lokalne
Pełna instrukcja: `docs/how-to/run-locally.md`. API: http://localhost:5033 (Swagger: `/swagger`), web: http://localhost:4200.

## Komendy (z katalogu głównego)
```
docker compose up -d                         # Postgres
dotnet build WordWise.slnx                   # warningi = błędy
dotnet test --solution WordWise.slnx
dotnet format WordWise.slnx                  # przed commitem
cd src/web && npm start                      # http://localhost:4200, proxy /api → :5033
cd src/web && npx ng test --watch=false
```
Środowisko: w narzędziu Bash nie działają programy z PATH Windows, więc uruchamiaj je przez `powershell.exe -NoProfile -Command "..."`.

## Gdzie czego szukać (czytaj tylko to, czego potrzebujesz)
- Wizja i MVP: `docs/vision.md` · architektura i model danych: `docs/architecture.md`
- Decyzje: `docs/adr/` (nowa decyzja → skill `new-adr`)
- Konwencje: `docs/conventions/{backend,frontend,testing,git,ai-workflow}.md`
- Funkcje: `docs/specs/NNN-nazwa/{spec,tasks}.md` · backlog: `docs/backlog.md`
- Nauka: `docs/learning/journal.md`, `docs/learning/concepts/`, **zakładki do dokumentacji: `docs/learning/references.md`**
- Podsumowania sesji (lokalne, poza gitem): `.local/summaries/`

## Zasady pracy (obowiązkowe)
1. **Tryb zadania** z `tasks.md` decyduje, ile kodu piszesz (patrz `docs/conventions/ai-workflow.md`):
   - `[solo]`: autor pisze sam, a Ty tylko robisz review i zadajesz pytania.
   - `[learn]`: piszesz szkielet i zostawiasz `TODO(human)` w kluczowych miejscach. Kodu za autora nie piszesz.
   - `[gen]`: generujesz kod, a potem prosisz autora o wyjaśnienie go własnymi słowami.
2. Każdy nietrywialny wybór uzasadnij 1–2 zdaniami i **linkiem do docs** (learn.microsoft.com / angular.dev). Linki weryfikuj przez MCP `microsoft-learn`, nie z pamięci. Nazywaj koncepcje (np. „Options pattern”, „scoped lifetime”). Przy szerszych tematach odsyłaj do pozycji z `docs/learning/references.md`.
3. Po zadaniu dopisz wpis do `docs/learning/journal.md` (format jest w pliku).
3a. **Po każdej sesji z zmianami** utwórz `.local/summaries/RRRR-MM-DD-<temat>.md` wg `.local/summaries/_TEMPLATE.md`: co dodano (pliki), **jak to sprawdzić** (komendy, URL-e, oczekiwany wynik), commity, co dalej. Folder jest w `.gitignore`.
3b. Praca zawsze na gałęzi funkcji (`feat/…`, `docs/…`, `chore/…`), nigdy na `main`. Małe, logiczne commity (Conventional Commits).
4. Nie zmieniaj decyzji z ADR bez pytania. Nową decyzję architektoniczną najpierw zaproponuj, potem zapisz jako ADR.
5. Kod, identyfikatory i commity po angielsku (Conventional Commits). Dokumentacja i wyjaśnienia po polsku.
6. Porównania tekstu w językach PL/TR zawsze z jawnie podaną kulturą albo `StringComparison` (tureckie İ/ı!).
7. Commity tylko lokalnie; `push` wyłącznie na wyraźną prośbę autora.
