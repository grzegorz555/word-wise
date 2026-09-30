---
name: new-adr
description: Tworzy nowy Architecture Decision Record w docs/adr/ na podstawie szablonu. Używaj, gdy zapada lub jest proponowana decyzja architektoniczna/technologiczna (biblioteka, wzorzec, struktura danych, sposób auth itp.).
---

# Nowy ADR

1. Ustal kolejny numer: najwyższy `docs/adr/NNNN-*.md` + 1.
2. Jeśli decyzja nie jest jeszcze podjęta przez autora — przedstaw 2–3 opcje z plusami/minusami i rekomendacją, zapytaj (AskUserQuestion), status `Proposed`.
3. Utwórz `docs/adr/NNNN-krotki-slug.md` wg `docs/adr/0000-template.md`, wypełniając sekcję **„Do nauki”** linkami z MCP `microsoft-learn`/`angular-cli`.
4. Jeśli ADR zastępuje inny — zmień status starego na `Superseded by ADR-NNNN`.
5. Jeśli decyzja zmienia architekturę/model — zaktualizuj `docs/architecture.md`.
6. Zwięźle: ADR ma mieścić się na jednym ekranie.
