---
name: new-adr
description: Creates a new Architecture Decision Record in docs/adr/ from the template. Use when an architectural/technology decision is being made or proposed (library, pattern, data structure, auth approach, etc.).
---

# New ADR

1. Determine the next number: the highest `docs/adr/NNNN-*.md` + 1.
2. If the author hasn't made the decision yet, present 2–3 options with pros/cons and a recommendation, ask (AskUserQuestion), status `Proposed`.
3. Create `docs/adr/NNNN-short-slug.md` following `docs/adr/0000-template.md`, filling in the **"Learning resources"** section with links from the `microsoft-learn`/`angular-cli` MCP.
4. If the ADR supersedes another one, change the old one's status to `Superseded by ADR-NNNN`.
5. If the decision changes the architecture/model, update `docs/architecture.md`.
6. Keep it concise: an ADR should fit on one screen.
