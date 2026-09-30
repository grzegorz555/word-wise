# ADR-0004: ASP.NET Core Identity z uwierzytelnianiem cookie

- Status: Accepted (sposób przekazywania: cookie vs token, do ewentualnej rewizji)
- Data: 2026-09-30

## Kontekst
Auth ma być w MVP, bo postęp nauki jest per użytkownik. SPA i API działają pod jednym originem (proxy w dev, wspólny host w produkcji).

## Rozważane opcje
1. **Identity + cookie (Identity API endpoints `MapIdentityApi<AppUser>()`, `useCookies=true`)**: ciasteczko HttpOnly, więc JS nie ma dostępu do sesji. Najmniej kodu, bezpieczne domyślnie.
2. Identity + bearer token: token trzymany w JS. Wygodne dla aplikacji mobilnych, ale wymaga obsługi odświeżania i jest bardziej podatne na XSS.
3. Zewnętrzny IdP (Entra ID, Auth0, Keycloak): za dużo na MVP.

## Decyzja
Opcja 1. Własna encja `AppUser : IdentityUser` z preferencjami językowymi.

## Konsekwencje
- Frontend wysyła żądania z `withCredentials` (lub przez ten sam origin), a API zwraca 401 zamiast przekierowania.
- Rejestracja/logowanie przez gotowe endpointy (`/api/auth/register`, `/api/auth/login?useCookies=true`).
- Ochrona przed CSRF: SameSite=Strict/Lax + ten sam origin. Do przejrzenia przy wdrożeniu.

## Do nauki
- Identity dla SPA: https://learn.microsoft.com/aspnet/core/security/authentication/identity-api-authorization
- Uwierzytelnianie vs autoryzacja: https://learn.microsoft.com/aspnet/core/security/authentication/
