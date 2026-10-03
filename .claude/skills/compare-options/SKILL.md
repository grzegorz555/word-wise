---
name: compare-options
description: Decision drill for WordWise. Presents 2–4 options for a technical decision (architecture, hosting, CI/CD, testing, release strategy…) with pros/cons and trade-offs, asks the author to choose first, then gives a recommendation and hands off to /new-adr. Use when the author says "porównaj", "jakie mamy opcje", "co wybrać", "za i przeciw", or for "Options to compare" items in docs/learning/roadmap.md.
---

# Compare options (decision drill)

Goal: the author learns **how to decide**, not only what to pick. Respond in Polish; anything written to the repo in English.

1. **Frame the decision** (3–5 lines): the problem, constraints from the repo (ADRs, `docs/conventions/security.md`, `accessibility.md`, hosting target Azure from `docs/learning/roadmap.md`), and the **criteria** with a weight (e.g. learning value, cost, simplicity, security, lock-in, effort to change later). Ask the author to adjust the criteria in one question if anything is unclear.
2. **Options** (2–4, including "do nothing / later" when it is valid). For each:
   - what it is (1–2 sentences, concept name),
   - pros / cons,
   - cost (money and effort) and how hard it is to reverse,
   - when it is the right choice (and who typically uses it),
   - 1 docs link, verified via MCP (`microsoft-learn`, `angular-cli`, `spartan-ui`) or the official site. If unverified, say so.
   Then a comparison table: options × criteria (✅ / 🟡 / ❌).
3. **Author's turn:** ask (AskUserQuestion) which option they would choose and why, in 1–2 sentences. Don't show your recommendation before the answer.
4. **Feedback:** compare their reasoning with yours. Name what they weighed well and what they missed (e.g. operations cost, rollback, security). Then give your recommendation and the **"revisit when"** condition (what would make us change the decision).
5. **Record:** if it is an architectural decision, run `/new-adr` with the options, criteria and "revisit when". Otherwise add the result to the topic in `docs/learning/roadmap.md` (level `[~]`).
6. Add a journal entry (`docs/learning/journal.md`) with a check question about the trade-off.

Keep it on one screen per step. Don't turn it into an essay.
