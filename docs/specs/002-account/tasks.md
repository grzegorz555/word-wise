# Tasks: Spec 002, account

Modes: `[solo]` you write it yourself · `[learn]` skeleton + TODO(human) · `[gen]` Claude generates, you do an explain-back.
**The modes below are a proposal.** Approve or change them before the first task (`docs/conventions/ai-workflow.md`). The open questions in `spec.md` are answered (2026-10-08).
Proposed split: 4 × `[gen]` (configuration), 4 × `[learn]` (security-critical logic), 2 × `[solo]` (patterns you already know: T6 from spec 001, Angular from work).

- [ ] **T1 `[gen]`** Identity packages, `AppUser : IdentityUser` (`UiLanguage`, `BaseLanguage`, `TargetLanguage`, nullable, max length 2), `WordWiseDbContext : IdentityDbContext<AppUser>`, `AddIdentityApiEndpoints<AppUser>()` + `AddEntityFrameworkStores`, migration `AddIdentity`.
  - *Predict before run:* which `AspNet*` tables will the migration create, and where does the password end up (and in what form)?
  - *To understand:* `IdentityDbContext` and why `base.OnModelCreating` matters, password hashing (PBKDF2), security stamp.
- [ ] **T2 `[learn]`** Password policy (R3): Identity options per NIST SP 800-63B-4 (length 15–128, no composition rules, lockout, unique email) + a custom `IPasswordValidator<AppUser>` for the blocklist (ADR-0015: local list + Pwned Passwords range API, fail open).
  - *You write:* the validator logic (local list first, then the API result; fail open) and its unit tests first, with a fake `HttpMessageHandler` (hit, miss, timeout). *Claude:* options, registration, the typed `HttpClient` for the range API (SHA-1 prefix, `Add-Padding`, timeout, resilience handler), the list file with its source and license.
  - *To understand:* the Options pattern for `IdentityOptions`, why composition rules weaken passwords (NIST), `IdentityResult` errors, k-anonymity, `IHttpClientFactory` and typed clients, fail open vs fail closed.
- [ ] **T3 `[learn]`** `/api/auth` group and authorization: `MapIdentityApi` in a group, built-in endpoints outside A1 → 404 and out of the contract (R5), logout, cookie options (R9), **fallback policy** + the anonymous list (R1), no enumeration on sign-up and sign-in (R4). Rate limiting comes later with A4 (Q2).
  - *You write:* the fallback policy, the anonymous list and the R4 endpoint filter. *Claude:* group wiring, cookie options, logout, the 404 filter for R5.
  - *To understand:* authentication vs authorization, the fallback policy vs `[Authorize]` everywhere, endpoint filters, account enumeration (OWASP).
- [ ] **T4 `[gen]`** Antiforgery (R2): `AddAntiforgery(o => o.HeaderName = "X-XSRF-TOKEN")`, `GET /api/antiforgery/token` writing the `XSRF-TOKEN` cookie, validation on every unsafe method (MVC + the `/api/auth` group), refresh after sign-in/sign-out; Angular `withXsrfConfiguration`.
  - *To understand:* CSRF and the double-submit cookie pattern, why `SameSite` alone is not enough, why the token is tied to the user.
- [ ] **T5 `[solo]`** `GET /api/account/me` + `PUT /api/account/preferences` (R6), tests first. The same patterns as spec 001 T6 (`[ApiController]`, request DTO, `IValidatableObject`, `LanguageCodes`), plus the user ID from `User`, never from the body.
- [ ] **T6 `[learn]`** Integration tests on the ADR-0013 fixture: register → login → `me`; logout → 401; no cookie → 401 (also `/api/words/cards`); missing XSRF header → 400; R4 (sign-up and sign-in responses look the same); preferences 400 cases. Update the spec 001 tests that now need a signed-in client.
  - *You write:* the test cases. *Claude:* a test helper that signs in and handles cookies + the XSRF header.
  - *To understand:* cookies in `HttpClient` (`HandleCookies`), test isolation in a shared database (ADR-0013).
- [ ] **T7 `[solo]`** Frontend core: `AuthService` (`currentUser` signal, `isSignedIn` computed), state loaded at startup (`provideAppInitializer`), functional `authGuard`, 401 interceptor → `/login?returnUrl=…`; Vitest with `HttpTestingController`.
- [ ] **T8 `[learn]`** Sign-in and sign-up pages (R10): forms with validation, Identity error codes → `account.errors.<code>` in EN/PL/TR, focus management after errors, SC 3.3.8 (paste, password managers, `autocomplete`).
  - *You write:* the accessibility parts (labels, `aria-describedby`, focus) and the error mapping. *Claude:* page skeletons, routes, translation keys.
- [ ] **T9 `[gen]`** Preferences page (UI language + pair, R6), "pair missing → preferences" guard (R8), UI language from the profile after sign-in and the first save (R7).
- [ ] **T10 `[gen]`** Review with the `code-reviewer` agent and `/security-review`, a pass through the ASVS L2 authentication and session chapters, journal entry, `/quiz-me` on T1–T9.
