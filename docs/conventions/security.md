# Security conventions

Baseline: **OWASP ASVS 5.0, level 2** (https://owasp.org/www-project-application-security-verification-standard/) as the checklist, **OWASP Top 10:2025** (https://top10.owasp.org/) as awareness, the **OWASP Cheat Sheet Series** (https://cheatsheetseries.owasp.org/) for "how", and the official framework guidance: ASP.NET Core security (https://learn.microsoft.com/aspnet/core/security/) and Angular security (https://angular.dev/best-practices/security). When sources disagree, the stricter rule wins.

## Principles
- **Deny by default, least privilege.** An endpoint that needs a user has `[Authorize]`; the user's ID always comes from the auth cookie (`User`), never from the request body or URL.
- **Never trust the client.** All validation that matters happens on the server; frontend validation is only for user experience.
- **Secure defaults, defense in depth.** Do not switch off a framework protection (sanitization, antiforgery, HTTPS) without an ADR.
- **Fail safely.** Errors return ProblemDetails without stack traces or internal details outside Development.

## Authentication and sessions (ADR-0004)
- Passwords follow **NIST SP 800-63B-4** (https://pages.nist.gov/800-63-4/sp800-63b.html): passwords are our only factor, so **minimum 15 characters**, maximum at least 64, **no composition rules** (no "one digit, one uppercase"), no forced periodic change, and a **check against a list of common or breached passwords**. ASP.NET Core Identity requires digits and uppercase by default, so we turn those options off and add a custom `IPasswordValidator` for the blocklist.
- Lockout after repeated failed sign-ins (Identity lockout) plus **rate limiting** on `/api/auth/login` and `/api/auth/register` (https://learn.microsoft.com/aspnet/core/performance/rate-limit).
- Auth cookie: HttpOnly, `Secure` outside Development, `SameSite=Lax` or stricter. Sign-in and sign-up errors don't reveal whether an email exists.
- **CSRF** (ADR-0004): `SameSite` + single origin **and antiforgery tokens**. ASP.NET Core antiforgery with `HeaderName = "X-XSRF-TOKEN"`; the API sets the `XSRF-TOKEN` cookie and validates the header on every state-changing request (POST/PUT/PATCH/DELETE), including the Identity API endpoints under `/api/auth` (minimal APIs with JSON bodies are not validated automatically, so the `/api/auth` group gets an explicit validation filter). The token is tied to the user, so it is refreshed after sign-in and sign-out. Angular `HttpClient` sends the header automatically (https://learn.microsoft.com/aspnet/core/security/anti-request-forgery, https://angular.dev/best-practices/security, https://cheatsheetseries.owasp.org/cheatsheets/Cross-Site_Request_Forgery_Prevention_Cheat_Sheet.html). GET never changes state.

## Input, output and data access
- Request DTOs are validated (data annotations / validation filter) with limits on length and allowed values (e.g. language codes ∈ {en, pl, tr}).
- **SQL**: only EF Core LINQ or `FromSql` with interpolated parameters; never build SQL by concatenation, never `FromSqlRaw` with user input (https://learn.microsoft.com/ef/core/querying/sql-queries).
- **XSS**: Angular escapes template bindings by default. Never use `bypassSecurityTrust*` or direct DOM APIs (`innerHTML`, `document.write`) with data from the API or the user.
- Mass assignment: map request DTOs to entities explicitly; never bind entities directly.

## Transport and HTTP headers
- HTTPS everywhere outside local development: `UseHttpsRedirection` + `UseHsts` (https://learn.microsoft.com/aspnet/core/security/enforcing-ssl).
- Before deployment: Content Security Policy with nonces and Trusted Types (Angular guide above), `X-Content-Type-Options: nosniff`, `Referrer-Policy`, frame protection (OWASP Secure Headers: https://owasp.org/www-project-secure-headers/).
- CORS stays off: the SPA and API share one origin (ADR-0004). If it is ever needed, list exact origins; never `AllowAnyOrigin` with credentials.

## Secrets and configuration
- No secrets in the repo, in logs or in AI conversations. Local development uses **user-secrets** (https://learn.microsoft.com/aspnet/core/security/app-secrets) or the git-ignored `.env`; `.env.example` contains only placeholders.
- Claude never reads `.env`, user-secrets or the credential manager without asking the author first.
- Connection strings and keys in production come from the hosting platform's secret store, not from `appsettings.json`.

## Personal data and logging
- Collect only what the app needs (email, language preferences, learning progress). Users can delete their account and data (GDPR / RODO, auth phase A4).
- Logs contain no passwords, tokens, cookies or full request bodies with personal data. Use structured logging with explicit fields.

## Dependencies and supply chain
- Lockfiles are committed (`package-lock.json`, central package versions in `Directory.Packages.props`).
- Check for known vulnerabilities: `npm audit` and `dotnet list package --vulnerable` (add both to CI); enable Dependabot alerts and secret scanning on GitHub.
- GitHub Actions use pinned versions and the minimum `permissions:` they need.
- New dependencies are chosen deliberately: maintained, widely used, permissive license; record bigger ones in an ADR.

## How we check
- The `code-reviewer` agent checks the points above for every change.
- Before deployment: a pass through the relevant ASVS L2 chapters (authentication, session management, access control, validation, configuration) and the security review (`/security-review`).

## Resources
- OWASP ASVS 5.0: https://owasp.org/www-project-application-security-verification-standard/
- OWASP Top 10:2025: https://top10.owasp.org/
- OWASP Cheat Sheet Series: https://cheatsheetseries.owasp.org/
- NIST SP 800-63B-4 (passwords, authentication): https://pages.nist.gov/800-63-4/sp800-63b.html
- ASP.NET Core security: https://learn.microsoft.com/aspnet/core/security/
- Angular security: https://angular.dev/best-practices/security
