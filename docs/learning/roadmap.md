# Learning roadmap: full-stack and software delivery

WordWise is the training ground. Each topic below is tied to a **real step in this project**, so you learn it by doing, not by reading.
Hosting target: **Azure** (author's choice, 2026-10-03). The specific Azure services are still to be compared (first `/compare-options` exercise, then an ADR).

## How to use this file
- Each topic has a **level**: `[ ]` not started · `[~]` used with help · `[x]` can explain it and do it alone (that is the bar for "done").
- **Options to compare** = decisions where more than one answer is valid. Run `/compare-options <topic>`: you guess first, then see the trade-offs, then record an ADR.
- Questions about delivery, architecture or operations → the `delivery-mentor` agent. Questions about C#/.NET → `backend-mentor`.
- `/quiz-me roadmap` draws questions from the topics marked `[~]`. `/quiz-me basics` drills track §0, and every quiz includes one §0 question.
- Order: the tracks go roughly top to bottom, but take topics when a WordWise task needs them.

---

## 0. Fundamentals: computer, network, debugging
Interview classics and the layer under every bug. No "options to compare" here: these are facts to understand, so the right column says **how to see it** on your own machine.
| Level | Topic | Where in WordWise | How to see it |
|---|---|---|---|
| [ ] | Program vs process vs thread | `dotnet run` starts `WordWise.Api`; Postgres runs as a process in a container | Task Manager → Details (PID, threads); `docker compose top` |
| [ ] | Memory: stack vs heap, value vs reference types, garbage collection | DTO `record`s, `string`, `int` | debugger: Locals; `dotnet-counters` (GC heap size) |
| [ ] | How .NET runs your code: C# → IL → JIT → machine code, the CLR | `bin/Debug/net10.0/WordWise.Api.dll` | open the `.dll` in ILSpy or `ildasm` |
| [ ] | async/await, the thread pool, and the browser event loop | `MigrateAsync`, `SaveChangesAsync`; Angular HTTP calls | debugger: Threads / Parallel Stacks; DevTools Performance |
| [ ] | Debugging C#: breakpoints (conditional, hit count), step into/over/out, Locals/Watch, Call Stack, break on exceptions | a test from `PostgresApiFactoryTests`, a controller action | VS Code: Run and Debug, or debug a single test |
| [ ] | Debugging Angular: DevTools Sources (source maps, breakpoints), Network, Console, Angular DevTools | language switch, `/api` calls | Chrome DevTools with `npm start` |
| [ ] | One request end to end: browser → dev proxy → Kestrel → middleware → controller → EF Core → Npgsql → Postgres, and back | `src/web/proxy.conf.json`, `Program.cs`, a controller | breakpoint in the controller + DevTools Network on the same request |
| [ ] | OSI and TCP/IP models: which layer HTTP, TLS, TCP, IP and Wi-Fi live on | every API call | DevTools Network → Timing (DNS, connect, TLS, waiting) |
| [ ] | IP, ports, sockets, DNS; `localhost` vs `0.0.0.0` | ports `5033`, `4200`, `5432` in `compose.yaml` and `launchSettings.json` | `netstat -ano`, `Test-NetConnection localhost -Port 5432`, `nslookup` |
| [ ] | HTTP: methods, status codes, headers, cookies, same origin, CORS | ProblemDetails, ADR-0004 (cookie auth), the dev proxy | DevTools Network → Headers; `curl -i` |
| [ ] | TLS/HTTPS and certificates, HSTS | `dotnet dev-certs https`, Azure later | the padlock → certificate; `curl -v https://…` |
| [ ] | Working from a café: public Wi-Fi risks (fake hotspot, eavesdropping, man-in-the-middle) and protection (HTTPS only, VPN, firewall "public" profile, MFA, locked screen, updates) | the app side: HTTPS + HSTS, `Secure` cookies, no secrets in the repo (`security.md`) | `Get-NetConnectionProfile`; check that a café network is "Public" |
| [ ] | Git internals: commits, branches as pointers, merge vs rebase, conflicts | PR merges, worktrees | `git log --graph --oneline`, `git merge-tree` |

## 1. Architecture and design
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [~] | Modular monolith, vertical slices vs layers | ADR-0002, `docs/architecture.md` | layered (N-tier) · vertical slices · Clean Architecture · microservices |
| [ ] | Architecture styles and when to split a system | thought exercise: "what if WordWise had 100k users?" | monolith · Web-Queue-Worker · microservices · serverless |
| [ ] | C4 diagrams (context, containers) | `docs/architecture.md` | C4 · arc42 · no diagrams |
| [ ] | Well-Architected pillars (reliability, security, cost, operations, performance) | review before the first production deploy | which trade-off we accept, and why |
| [ ] | Caching (HTTP, in-memory, distributed) | word catalog (rarely changes) | `Cache-Control` · `IMemoryCache` · `HybridCache` · Redis |

## 2. API and full-stack integration
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [ ] | REST resource design, status codes, ProblemDetails | spec 001 endpoints | — |
| [~] | Contract-first vs code-first, client generation | ADR-0012 (OpenAPI → ng-openapi-gen) | hand-written DTOs · generated client · tRPC/GraphQL |
| [ ] | Pagination, filtering, API versioning | word list | offset · cursor (keyset) |
| [ ] | Auth for an SPA: cookies vs tokens, CSRF, CORS | auth spec (planned), ADR-0004 | cookie + same-site · JWT in memory · BFF |
| [ ] | Rate limiting, idempotency | login, saving progress | fixed window · sliding window · token bucket |

## 3. Data
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [ ] | EF Core model, migrations in development | ADR-0003 | — |
| [ ] | **Migrations in production** | first deploy | runtime `MigrateAsync` · migration bundle · reviewed SQL script |
| [ ] | Backward-compatible schema changes (expand/contract) | first column rename | big-bang · expand/contract |
| [ ] | Indexes, query plans (`EXPLAIN`) | word lookup, user progress | — |
| [ ] | Backups, restore drill, RPO/RTO | Azure Postgres | built-in PITR · `pg_dump` · long-term backup |

## 4. Testing and quality
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [~] | Unit tests (xUnit + Shouldly, Vitest) | ADR-0008 | — |
| [ ] | Integration tests against a real database | ADR-0013 | in-memory provider · SQLite · Testcontainers Postgres |
| [ ] | Test pyramid vs testing trophy: what to test at which level | all specs | — |
| [ ] | End-to-end tests | login + study flow | Playwright · Cypress · none (cost vs confidence) |
| [ ] | Automated accessibility checks | every screen (WCAG 2.2 AA) | axe in Playwright · Lighthouse CI · manual only |
| [ ] | Contract tests (API ↔ client) | OpenAPI document in CI | diff of `openapi.json` · Pact |
| [ ] | Load testing | word catalog endpoint | k6 · none until needed |

## 5. CI/CD (pipeline)
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [~] | CI: build, format, test on every PR | `.github/workflows/ci.yml` | — |
| [ ] | Containers: Dockerfile vs SDK container publish, multi-stage, image size | API image | Dockerfile · `dotnet publish /t:PublishContainer` |
| [~] | Where the SPA lives | ADR-0014: served by the API | Static Web Apps · served by the API · separate container |
| [ ] | Container registry and image tags (commit SHA, SemVer) | first deploy | ACR · GitHub Container Registry |
| [ ] | Azure login from CI without secrets (OIDC, workload identity federation) | deploy workflow | OIDC · service principal secret · publish profile |
| [ ] | Infrastructure as code | Azure resources | Bicep · Terraform · portal by hand (why not) |
| [ ] | Environments and promotion (staging → prod), approvals | GitHub Environments | one env · staging + prod · preview env per PR |
| [ ] | Deployment strategies and rollback | Container Apps revisions | recreate · rolling · blue-green · canary |

## 6. Releasing
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [~] | Conventional Commits | `docs/conventions/git.md` | — |
| [ ] | Semantic Versioning, changelog generated from commits | first release `v0.1.0` | release-please · semantic-release · manual |
| [ ] | Branching model | current flow: feature branch → PR → main | trunk-based · GitHub Flow · GitFlow |
| [ ] | Feature flags: release ≠ deploy | spaced repetition (big feature) | config flag · `Microsoft.FeatureManagement` · none |
| [ ] | DORA metrics (deploy frequency, lead time, change failure rate, time to restore) | after a few releases | — |

## 7. Security and supply chain
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [~] | OWASP ASVS L2, NIST 800-63B (passwords) | `docs/conventions/security.md` | — |
| [~] | Dependency pinning, `npm audit`, `overrides` | 2026-10-01 journal entry, pinned MCP versions | — |
| [ ] | Dependency updates | GitHub | Dependabot · Renovate |
| [ ] | Static analysis (SAST) | CI | CodeQL · analyzers only |
| [ ] | Secrets in the cloud: managed identity, Key Vault, Container Apps secrets | Postgres connection | connection string secret · managed identity (passwordless) |
| [ ] | Security headers (CSP, HSTS), HTTPS-only cookies | backlog: Auth A4 | — |

## 8. Operations (after release)
| Level | Topic | Where in WordWise | Options to compare |
|---|---|---|---|
| [ ] | Structured logging | API | console JSON · Serilog · OpenTelemetry logs |
| [ ] | Observability: logs, metrics, traces (OpenTelemetry) | API + Postgres | Application Insights · Aspire Dashboard (local) · Grafana stack |
| [ ] | Health checks: liveness vs readiness | `/healthz`, Container Apps probes | — |
| [ ] | Alerts, SLO and error budget | "API answers in < 500 ms for 99% of requests" | — |
| [ ] | Incidents and postmortems (blameless) | "break and fix" exercises on staging | — |
| [ ] | Cost control: budgets, scale to zero, stop the dev database | Azure subscription | — |

---

## Exercises (beyond normal tasks)
- **Break and fix:** on staging, Claude breaks one thing (wrong config, slow query, failing migration). You diagnose it only from logs and metrics, roll back, and write a short postmortem in `docs/learning/postmortems/`.
- **Design kata:** one question per month about WordWise at a different scale (100k users, offline mode, audio for every word). 30 minutes, a C4 sketch, list of trade-offs.
- **Predict before run:** before every deploy, write what you expect to happen. Compare afterwards.

## Bookmarks
Links for every track are in `docs/learning/references.md`, section "Delivery and operations".
