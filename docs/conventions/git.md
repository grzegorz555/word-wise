# Konwencje gita

- **Conventional Commits** (https://www.conventionalcommits.org): `feat(api): add words endpoint`, `fix(web): ...`, `docs(adr): ...`, `test`, `refactor`, `chore`, `ci`.
- Scope: `api`, `web`, `data`, `docs`, `adr`, `ci`.
- **Nigdy nie pracujemy na `main`.** Gałęzie: `<typ>/<krótko>` (np. `feat/word-catalog`, `chore/project-setup`, `docs/references`). Numer spec w nazwie opcjonalnie (`feat/001-word-catalog`). PR do `main`, CI musi być zielone.
- Commity lokalne, małe i logiczne (jedna zmiana = jeden commit). `push` tylko na prośbę autora.
- Tożsamość gita ustawiona **lokalnie dla tego repo** (`git config --local`), oddzielnie od innych projektów.
- Globalne hooki są wyłączone: `git config --local core.hooksPath .git/hooks`. W nowym klonie trzeba to ustawić ponownie, bo konfiguracja lokalna nie jest częścią repo.
- Jeden PR = jedna spec albo jej spójny fragment. Commit bez zrozumienia każdej linii jest zakazany (patrz `ai-workflow.md`).
- Sekrety tylko w `.env` / user-secrets (oba poza repo).
