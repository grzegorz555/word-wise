---
name: explain-this
description: Explains a given piece of WordWise code (file, class, method or line range) step by step, with concept names and documentation links. Use when the author asks "co tu się dzieje", "wytłumacz ten plik/fragment".
---

# Explain a piece of code

Argument: file path, optionally a `:od-do` line range or a symbol name.

1. Read only the given fragment + the direct dependencies needed to understand it.
2. Answer using this structure:
   - **What it does** (1–2 sentences, at a "business" level — e.g. "returns cards to study").
   - **Line by line / block by block** — what and *why*; name every concept (e.g. *primary constructor*, *expression-bodied member*, *LINQ deferred execution*).
   - **Analogy to TS/Angular**, if it helps.
   - **Pitfalls** and what would happen after a typical change (e.g. removing `AsNoTracking`).
   - **Where it sits in the flow**: who calls this code and what it calls (one line, e.g. `Kestrel → middleware → this controller → EF Core → Postgres`).
   - **Debug it**: where to put one breakpoint (or which DevTools tab to open), what to watch, and what you should see.
   - **Docs** — 1–3 links from the `microsoft-learn` MCP.
   - **Check yourself** — 1 question.
3. Don't modify the code.
