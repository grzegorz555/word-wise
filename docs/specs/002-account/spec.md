# Spec 002: Account (sign-up, sign-in, language preferences)

- Status: **Draft** (open questions below; the author approves the task modes before work starts)
- ADR: 0004 (Identity + cookie), 0006 (languages), 0013 (integration test database)
- Plan: `docs/architecture.md`, "Authentication: framework plan", phase **A1**
- User stories from `vision.md`: 1 (account and sign-in), 2 (UI language), 3 (language pair)

## Goal
A user creates an account with an email and a password, signs in and out with an HttpOnly cookie session, and saves their UI language and language pair. **Everything except signing in and signing up requires an account** (decided by the author, 2026-10-08).

## Scope
1. `AppUser : IdentityUser` with `UiLanguage`, `BaseLanguage`, `TargetLanguage` (all nullable, ISO 639-1 codes); `WordWiseDbContext : IdentityDbContext<AppUser>`; migration `AddIdentity`.
2. Identity API endpoints in an `/api/auth` group (register, login with cookies) + a custom logout.
3. Account endpoints: who am I, save preferences.
4. Authorization by default: a fallback policy that requires a signed-in user; explicit anonymous endpoints only.
5. Antiforgery for every state-changing request (ADR-0004, `security.md`).
6. Password policy per NIST SP 800-63B-4 (`security.md`), lockout, no account enumeration.
7. Frontend: auth state, guard, 401 interceptor, sign-in and sign-up pages, preferences page.

## Contract
```http
GET  /api/antiforgery/token            → 204, sets the XSRF-TOKEN cookie (anonymous)
POST /api/auth/register                 { "email", "password" }        → 200 (also for an existing email, see R4)
POST /api/auth/login?useCookies=true    { "email", "password" }        → 200 + auth cookie | 401 ProblemDetails
POST /api/auth/logout                                                   → 204 (signed in)
GET  /api/account/me                    → 200 { "email": "a@b.pl", "uiLanguage": "pl" | null, "baseLanguage": "pl" | null, "targetLanguage": "tr" | null } | 401
PUT  /api/account/preferences           { "uiLanguage": "pl", "baseLanguage": "pl", "targetLanguage": "tr" } → 204 | 400 ValidationProblem
```

## Rules
- R1: Every endpoint requires a signed-in user, except: `/api/antiforgery/token`, `/api/auth/register`, `/api/auth/login`, `/health`, and the OpenAPI/Swagger UI in Development. `GET /api/words/cards` (spec 001) now requires sign-in too. Without a session the API returns **401** (no redirect); a signed-in user without permission gets **403**.
- R2: Every POST/PUT/PATCH/DELETE, including `/api/auth/*`, requires a valid `X-XSRF-TOKEN` header matching the `XSRF-TOKEN` cookie; otherwise **400**. The token is refreshed after sign-in and sign-out.
- R3: Password: **15 to 128 characters**, no composition rules, rejected if it is on the blocklist of common or breached passwords (source: open question Q1). Identity lockout after repeated failed sign-ins.
- R4: **No account enumeration** (`security.md`): sign-up with an email that already exists answers exactly like a successful sign-up and changes nothing; every failed sign-in (unknown email, wrong password, locked out) returns the same 401 body.
- R5: Built-in Identity endpoints outside A1 (`/refresh`, `/confirmEmail`, `/resendConfirmationEmail`, `/forgotPassword`, `/resetPassword`, `/manage/*`) return **404** until their phase (A3/A5) and are not in the OpenAPI contract.
- R6: Preferences: every code ∈ {`en`, `pl`, `tr`}; `uiLanguage` is required; `baseLanguage` and `targetLanguage` are both set or both null, and `baseLanguage ≠ targetLanguage`. Otherwise **400 ValidationProblem**. The user's ID always comes from the cookie, never from the request.
- R7: After the first sign-in `uiLanguage` is null: the SPA saves its active UI language with one `PUT /api/account/preferences`. Later sign-ins switch the UI to `uiLanguage` from the profile (ADR-0006).
- R8: While the language pair is not set, signed-in pages that need it redirect to the preferences page (decided by the author: "pair later").
- R9: Auth cookie: HttpOnly, `SameSite=Lax`, `Secure` outside Development, sliding expiration.
- R10: Forms meet WCAG 2.2 AA (`docs/conventions/accessibility.md`): visible labels, errors as text linked with `aria-describedby`, `autocomplete="email" | "current-password" | "new-password"`, paste and password managers allowed (SC 3.3.8). Identity error codes map to Transloco keys `account.errors.<code>` in EN/PL/TR.

## Acceptance criteria
- [ ] `dotnet ef database update` creates the Identity tables (migration `AddIdentity`).
- [ ] Integration test (ADR-0013): register → login → `me` returns the email; logout → `me` → 401.
- [ ] Integration test: `me` and `GET /api/words/cards` without a cookie → 401.
- [ ] Integration test: a POST without the XSRF header → 400.
- [ ] Integration test: sign-up with an existing email looks the same as a new sign-up (R4); failed sign-ins all look the same.
- [ ] Unit tests: password blocklist validator; preferences validation (R6).
- [ ] Vitest: `AuthService`, the guard (signed in / out), the interceptor (401 → `/login`), the sign-in form (validation messages, submit).
- [ ] Manual check with the keyboard and a screen reader on the sign-in and sign-up pages.
- [ ] Entry in `docs/learning/journal.md`.

## Out of scope
Email confirmation and password reset (A3), security headers, account deletion and HTTPS-only deployment settings (A4), external sign-in and 2FA (A5), a demo account for portfolio visitors (backlog idea), user progress (spec 003).

## Open questions (the author decides before the status becomes Ready)
- **Q1, password blocklist source:** (a) a list of common passwords embedded in the app (a few thousand to 100k entries from a permissively licensed source, checked locally), or (b) the Have I Been Pwned "Pwned Passwords" range API (k-anonymity: only the first 5 characters of the SHA-1 hash leave the server; a much larger breach corpus, but an external call at sign-up). Run `/compare-options`.
- **Q2, rate limiting on `/api/auth/login` and `/register`:** `security.md` lists it with the authentication rules, `architecture.md` plans it for A4 (before deployment). In this spec, or with A4? (Algorithm choice: roadmap §2, "fixed window · sliding window · token bucket".)
- **Q3, R4 user experience:** with no email confirmation yet (A3), a person who already has an account and signs up again sees "account created" and then signs in with their old password. Acceptable until A3?
