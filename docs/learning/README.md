# Learning guide: start here

A lot of learning tooling lives in this repo. This page answers three questions: **what do I use right now**, **what matters most**, and **what is next in the queue**.
Tick the queue as you go: change `[ ]` to `[x]` in this file. Topic levels (`[ ]` / `[~]` / `[x]`) live in [roadmap.md](roadmap.md).

---

## 1. "I want to…" → use this

| I want to… | Use | Notes |
|---|---|---|
| **learn by building** a feature | `/spec-feature`, then the tasks in `docs/specs/NNN/tasks.md` | The task mode decides who writes the code: `[solo]` you, `[learn]` you with a skeleton, `[gen]` Claude + your explain-back. The **Mentor** output style is on by default. |
| understand **a piece of code** | `/explain-this <file or lines>` | Step by step, with concept names and links. |
| understand **a C#/.NET concept** (DI, async, LINQ, EF) | ask the `backend-mentor` agent | Socratic: hints and an example on a different problem, not your solution. |
| understand **architecture, CI/CD, Azure, releases, production** | ask the `delivery-mentor` agent | Always shows 2–3 options and what can go wrong in production. |
| **make a decision** (library, hosting, pattern) | `/compare-options <topic>` → `/new-adr` | You choose first, then compare with the recommendation. |
| **read documentation** | [references.md](references.md) (⭐ = start here) | Or ask Claude to search: `microsoft-learn` (.NET, Azure), `angular-cli` (Angular), `spartan-ui` (UI components). |
| **check myself** | `/quiz-me` · `/quiz-me <topic>` · `/quiz-me roadmap` | Weekly. Weak topics go into the journal. |
| get **feedback on my code** | the `code-reviewer` agent | Fresh context, checks the conventions and ADRs. Use it before every commit of `[solo]`/`[learn]` work. |
| write **my own notes** | [concepts/](concepts/README.md) | One concept = one file. Writing it yourself is the real test of understanding. |
| see **what I already did** | [journal.md](journal.md), `.local/summaries/` | Fill in "In my own words" in the journal; that is your explain-back. |
| see **the big picture** | [roadmap.md](roadmap.md) | 8 tracks, each topic tied to a WordWise step. |

## 2. Rhythm

- **Each task session:** predict before you run, explain-back after `[gen]`, `code-reviewer` before you commit.
- **Weekly:** `/quiz-me` (15 min) and a read of the last journal entries.
- **Monthly:** one design kata from [roadmap.md](roadmap.md#exercises-beyond-normal-tasks), and update the levels in the roadmap.
- **Outside the project:** the freeCodeCamp C# course and the Exercism C# track, a little at a time.

## 3. What matters most (priorities)

My view of a developer's job in a few years, as of 2026. This is an opinion, not a fact. AI agents already write a large share of routine code, and that share keeps growing. The scarce skills are the ones that tell **whether the code is right and whether the system works**: specifying the problem, judging generated code, verifying it, designing systems, keeping production healthy and secure. Syntax and boilerplate matter less, but you still need the fundamentals: you cannot review what you cannot read.

### Tier 1: core, highest return (do these first and deeply)
| Skill | Why it lasts | Roadmap |
|---|---|---|
| **Reading and reviewing code critically** (C# fundamentals included) | Most of your code will be written by someone else (or something else). Review is the job. | fCC course, `[solo]` tasks, `code-reviewer` |
| **Testing and verification** | "It compiles" and "the AI says it works" are not proof. Knowing what to test, at which level, is how you trust anything. | §4 Testing |
| **Data modeling, SQL and EF Core** | Data outlives code. A bad schema is expensive to change; a slow query takes production down. | §3 Data |
| **HTTP, APIs and security fundamentals** (auth, OWASP) | Every web system is built on them, and security mistakes are the costliest ones. | §2 API, §7 Security |
| **Debugging and observability** | Production problems come without a stack trace in your IDE. Logs, metrics and traces are how you find them. | §8 Operations |
| **Trade-offs and decisions** (ADR) | There is rarely one right answer. Naming the options and the cost of each is what senior engineers do. | `/compare-options`, §1 Architecture |
| **Working with AI agents** (specs, context, review) | Clear specs and good review decide whether an agent speeds you up or creates debt. This repo is a practice ground for exactly that. | `docs/conventions/ai-workflow.md` |

### Tier 2: needed to ship on your own
| Skill | Roadmap |
|---|---|
| CI/CD pipelines and containers | §5 CI/CD |
| Cloud basics (Azure): hosting, managed database, identity, cost | §5, §7, §8 |
| Infrastructure as code (Bicep) | §5 |
| Releases: SemVer, changelog, feature flags, rollback | §6 Releasing |
| Accessibility (WCAG 2.2 AA) | `docs/conventions/accessibility.md` |
| Performance basics: caching, indexes, bundle size | §1, §3 |

### Tier 3: learn when a real need appears
Microservices and messaging, Kubernetes, distributed caching (Redis), load testing at scale, GraphQL, DORA metrics, multi-region setups. Know **what they are and when they are worth it** (one design kata each is enough). Go deep only when WordWise or work needs them.

---

## 4. The queue (tick as you go)

Ordered by importance and dependency. Top = next.

### Now
- [ ] **Spec 001: word catalog, T1–T8** ([tasks](../specs/001-word-catalog/tasks.md)). First real backend work: EF Core, migrations, LINQ, a parser written test-first. *Tier 1: data, testing, C#.*
- [ ] Fill in "In my own words" for the existing journal entries ([journal.md](journal.md)).
- [ ] First `/quiz-me` and first concept note in [concepts/](concepts/README.md) (suggestion: dependency injection).

### Next
- [ ] `/compare-options` for the Azure services (Container Apps or App Service, Static Web Apps, Postgres flexible server) → ADR-0013. *Tier 1: trade-offs.*
- [ ] Integration tests against a real Postgres (Testcontainers). Needed before auth. *Tier 1: testing.*
- [ ] **Spec 002: account** (sign-up, sign-in, cookie session, language preferences; auth A1). *Tier 1: security.*
- [ ] **Spec 003: study session** (flashcards, progress; auth A2).

### Deployment to Azure (one milestone = one spec)
- [ ] 1. Container image for the API (Dockerfile vs SDK container publish, health checks).
- [ ] 2. CI extensions: Playwright e2e + axe, CodeQL, Dependabot.
- [ ] 3. CD to staging: OIDC login, Bicep, EF migration bundle, secrets.
- [ ] 4. First release `v0.1.0`: SemVer, changelog from commits.
- [ ] 5. Observability: structured logs, OpenTelemetry, alerts, one SLO.
- [ ] 6. Production + rollback drill + first "break and fix" exercise with a postmortem.
- [ ] Auth A4 before production: HTTPS-only cookie, security headers, rate limiting, account deletion.

### Later (product features, each one also teaches something)
- [ ] Spaced repetition (SM-2), released behind a feature flag. *Teaches: algorithms, feature flags.*
- [ ] Games: multiple choice, typing (culture-aware comparison for TR/PL), matching pairs.
- [ ] Listen to the word (Web Speech API).
- [ ] Dark mode.
- [ ] Email confirmation and password reset (auth A3).

### Recurring
- [ ] This week's `/quiz-me` (untick and tick again each week, or keep the score in the journal).
- [ ] This month's design kata.
