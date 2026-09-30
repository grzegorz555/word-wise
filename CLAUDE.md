# WordWise — instructions for Claude

> **Isolation:** this project is completely separate from the author's other repositories. Its methods, conventions, data and decisions apply **only here**. Don't carry them over to other projects, and don't bring rules from other projects in here (except general security rules).
> Work tooling is disabled here: the `kup-archiwum` skill (deny in `.claude/settings.json`) and global git hooks (`git config --local core.hooksPath .git/hooks`). Don't archive anything from this repo to SVN KUP.

Flashcards with the 1000 most common words in **EN / PL / TR** (each is also a UI language).
A **learning + portfolio** project. The author is learning C#/.NET (freeCodeCamp course "Foundational C# with Microsoft") and knows Angular from work.
**Priority: the author should understand the code, not just receive it.**

## Stack
- API: ASP.NET Core 10 (LTS), controllers + vertical slices → `src/api/WordWise.Api` (details: `src/api/CLAUDE.md`)
- Web: Angular 22 (standalone, signals, zoneless), Transloco → `src/web` (details: `src/web/CLAUDE.md`)
- DB: PostgreSQL 18 (`compose.yaml`), EF Core, ASP.NET Core Identity
- Tests: xUnit v3 + Shouldly (Microsoft.Testing.Platform), Vitest on the frontend
- CI: GitHub Actions (`.github/workflows/ci.yml`)

## Running locally
Full guide: `docs/how-to/run-locally.md`. API: http://localhost:5033 (Swagger: `/swagger`), web: http://localhost:4200.

## Commands (from the repo root)
```
docker compose up -d                         # Postgres
dotnet build WordWise.slnx                   # warnings = errors
dotnet test --solution WordWise.slnx
dotnet format WordWise.slnx                  # before committing
cd src/web && npm start                      # http://localhost:4200, proxy /api → :5033
cd src/web && npx ng test --watch=false
```
Environment: programs from the Windows PATH don't work in the Bash tool, so run them via `powershell.exe -NoProfile -Command "..."`.

## Where to find things (read only what you need)
- Vision and MVP: `docs/vision.md` · architecture and data model: `docs/architecture.md`
- Decisions: `docs/adr/` (new decision → `new-adr` skill)
- Conventions: `docs/conventions/{backend,frontend,testing,git,ai-workflow}.md`
- Features: `docs/specs/NNN-name/{spec,tasks}.md` · backlog: `docs/backlog.md`
- Learning: `docs/learning/journal.md`, `docs/learning/concepts/`, **documentation bookmarks: `docs/learning/references.md`**
- Session summaries (local, outside git): `.local/summaries/`

## Working rules (mandatory)
1. The **task mode** in `tasks.md` decides how much code you write (see `docs/conventions/ai-workflow.md`):
   - `[solo]`: the author writes the code; you only review and ask questions.
   - `[learn]`: you write a skeleton and leave `TODO(human)` in key places. You don't write the code for the author.
   - `[gen]`: you generate the code, then ask the author to explain it in their own words.
2. Justify every non-trivial choice in 1–2 sentences with a **link to the docs** (learn.microsoft.com / angular.dev). Verify links via the `microsoft-learn` MCP, not from memory. Name the concepts (e.g. "Options pattern", "scoped lifetime"). For broader topics, point to entries in `docs/learning/references.md`.
3. After a task, add an entry to `docs/learning/journal.md` (the format is in the file).
3a. **After every session with changes**, create `.local/summaries/YYYY-MM-DD-<topic>.md` following `.local/summaries/_TEMPLATE.md`: what was added (files), **how to verify it** (commands, URLs, expected result), commits, next steps. The folder is in `.gitignore`.
3b. Always work on a feature branch (`feat/…`, `docs/…`, `chore/…`), never on `main`. Small, logical commits (Conventional Commits).
4. Don't change ADR decisions without asking. Propose a new architectural decision first, then record it as an ADR.
5. Code, identifiers, commits and repo documentation in English (Conventional Commits). Explanations to the author in Polish.
5a. Every tracked doc has a local, git-ignored Polish copy: `X.local.md` next to `X.md`; for `CLAUDE.md` it is `CLAUDE.pl.local.md` (because `CLAUDE.local.md` is auto-loaded by Claude Code), and for `.claude/agents/*` and `.claude/output-styles/*` it lives under `.local/docs-pl/.claude/…` (Claude Code loads every .md in those folders). When you change a doc, update its Polish copy in the same session; when you add a new doc, create its Polish copy. Never read the `.local.md` copies for instructions — the English files are the source of truth.
6. Text comparisons in PL/TR always use an explicit culture or `StringComparison` (Turkish İ/ı!).
7. Commits are local only; `push` only when the author explicitly asks.
