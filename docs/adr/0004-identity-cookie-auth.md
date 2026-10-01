# ADR-0004: ASP.NET Core Identity with cookie authentication

- Status: Accepted (transport mechanism, cookie vs token, may be revisited)
- Date: 2026-09-30

## Context
Auth belongs in the MVP because learning progress is per user. The SPA and API run on a single origin (proxy in dev, shared host in production).

## Considered options
1. **Identity + cookie (Identity API endpoints `MapIdentityApi<AppUser>()`, `useCookies=true`)**: HttpOnly cookie, so JS has no access to the session. The least code, secure by default.
2. Identity + bearer token: token stored in JS. Convenient for mobile apps, but requires refresh handling and is more exposed to XSS.
3. External IdP (Entra ID, Auth0, Keycloak): too much for the MVP.

## Decision
Option 1. A custom `AppUser : IdentityUser` entity with language preferences.

## Consequences
- The frontend sends requests with `withCredentials` (or via the same origin), and the API returns 401 instead of a redirect.
- Registration/login via the built-in endpoints (`/api/auth/register`, `/api/auth/login?useCookies=true`).
- CSRF protection: SameSite=Strict/Lax + same origin, **plus antiforgery tokens** (updated 2026-10-01, decided by the author, following OWASP and Microsoft guidance). The API issues an `XSRF-TOKEN` cookie (readable by JS, not HttpOnly) and validates the `X-XSRF-TOKEN` header on POST/PUT/PATCH/DELETE; Angular `HttpClient` sends the header automatically for same-origin requests.

## Learning resources
- Identity for SPAs: https://learn.microsoft.com/aspnet/core/security/authentication/identity-api-authorization
- Authentication vs authorization: https://learn.microsoft.com/aspnet/core/security/authentication/
- Antiforgery (XSRF/CSRF) in ASP.NET Core, SPA section: https://learn.microsoft.com/aspnet/core/security/anti-request-forgery
- Angular HttpClient XSRF protection: https://angular.dev/best-practices/security
- Framework plan (phases, endpoints, tests): `docs/architecture.md`, section "Authentication: framework plan"
