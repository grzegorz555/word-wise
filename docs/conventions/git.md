# Git conventions

- **Conventional Commits** (https://www.conventionalcommits.org): `feat(api): add words endpoint`, `fix(web): ...`, `docs(adr): ...`, `test`, `refactor`, `chore`, `ci`.
- Scope: `api`, `web`, `data`, `docs`, `adr`, `ci`.
- **We never work on `main`.** Branches: `<type>/<short-name>` (e.g. `feat/word-catalog`, `chore/project-setup`, `docs/references`). Spec number in the name is optional (`feat/001-word-catalog`). PR into `main`, CI must be green.
- Commits are local, small and logical (one change = one commit). `push` only when the author asks.
- Git identity is set **locally for this repo** (`git config --local`), separately from other projects.
- Global hooks are disabled: `git config --local core.hooksPath .git/hooks`. In a fresh clone this must be set again, because local config is not part of the repo.
- One PR = one spec or a coherent part of it. Committing without understanding every line is forbidden (see `ai-workflow.md`).
- Secrets only in `.env` / user-secrets (both outside the repo).
