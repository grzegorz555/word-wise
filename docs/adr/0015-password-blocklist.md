# ADR-0015: Password blocklist: a local list of common passwords + the Pwned Passwords range API

- Status: Accepted
- Date: 2026-10-08

## Context
`security.md` (NIST SP 800-63B-4) requires checking new passwords against a list of common or breached passwords; ASP.NET Core Identity has no such check, so spec 002 (T2) adds a custom `IPasswordValidator<AppUser>`. The question is where the list comes from. The check runs at sign-up (and later at password change), so it must not leak the password and must not make sign-up unavailable.

## Decision criteria
1. Coverage: common **and** breached passwords. 2. Privacy: the password (or anything that identifies it) never leaves the server. 3. Availability: sign-up keeps working when a third party is down. 4. Learning value. 5. Effort and maintenance.

## Considered options
1. **A local list** of common passwords in the repo (a few thousand to 100k entries in a `HashSet<string>`): no external call, but no breach data; the source must have a permissive license.
2. **The Have I Been Pwned "Pwned Passwords" range API**: the server sends only the first 5 characters of the password's SHA-1 hash (k-anonymity) and compares the returned suffixes locally; covers a large breach corpus; free, no API key, no rate limit, no license or attribution requirement (HIBP API v3 docs). Costs one external call per sign-up.
3. **Both layers** (chosen): the local list always, the range API as a second layer.

## Decision
Option 3, chosen by the author in a `/compare-options` drill; it matches Claude's recommendation. Rules:
- The local list is checked first and always.
- The range API is called with the `Add-Padding: true` header (so the response size doesn't reveal the prefix), through a typed `HttpClient` with a short timeout and a resilience handler.
- **Fail open:** if the API is unavailable or too slow, the password is judged by the local list only and a warning is logged (without the password or its hash).
- The password is rejected if the API reports any occurrence (count > 0).

## Consequences
- One outgoing HTTPS dependency at sign-up: `api.pwnedpasswords.com`. When the app is deployed it needs outbound internet access (Container Apps has it by default).
- Tests never call the real API: the typed client gets a fake `HttpMessageHandler` (hit, miss, timeout → fail open).
- The local list is a file in the repo with its source and license noted next to it; choosing the source is part of T2.
- Logs never contain the password, the full hash or the prefix tied to a user.

## Revisit when
- The Pwned Passwords terms change (key, rate limit, license), or the API often fails or slows sign-ups.
- A self-hosted copy of the breach corpus becomes worth it (many sign-ups, offline requirement).

## Learning resources
- NIST SP 800-63B-4 (password requirements, blocklists): https://pages.nist.gov/800-63-4/sp800-63b.html (not on Microsoft Learn)
- HIBP API v3, Pwned Passwords range and padding: https://haveibeenpwned.com/API/v3#PwnedPasswords (checked 2026-10-08)
- Custom password validators in Identity (`IPasswordValidator<TUser>`): https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.identity.ipasswordvalidator-1
- `IHttpClientFactory` and typed clients: https://learn.microsoft.com/aspnet/core/fundamentals/http-requests
- HTTP resilience (timeouts, retries): https://learn.microsoft.com/dotnet/core/resilience/http-resilience
