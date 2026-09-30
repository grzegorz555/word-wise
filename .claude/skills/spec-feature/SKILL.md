---
name: spec-feature
description: Guides the author through writing a spec for a new WordWise feature (docs/specs/NNN-name/spec.md + tasks.md with [solo]/[learn]/[gen] modes). Use before implementing any new feature or when the author says "nowa funkcja", "napiszmy spec".
---

# Feature spec (lightweight Spec-Driven Development)

1. Read `docs/vision.md`, `docs/specs/README.md` and the ADRs related to the feature. Don't read code unless it's needed.
2. Ask the author **max 3–4 questions** about unclear points (scope, edge-case rules, UI). Propose defaults for the rest.
3. Create `docs/specs/NNN-slug/spec.md` following the example `docs/specs/001-word-catalog/spec.md`:
   Goal · Scope · Contract (HTTP/UI) · Rules R1…Rn · Acceptance criteria (checklist, testable) · Out of scope.
4. Create `tasks.md`: 5–10 tasks, each with a mode. Splitting rule:
   - configuration/boilerplate → `[gen]`,
   - domain logic, EF queries, rule tests → `[learn]` or `[solo]`,
   - ~1/3 of each mode; for each task, a line *"To understand:"* listing the concepts.
5. A new architectural decision along the way → `new-adr` skill.
6. Update the table in `docs/specs/README.md`. The author approves the modes before starting.
