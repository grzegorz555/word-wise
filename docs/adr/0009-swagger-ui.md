# ADR-0009: Swagger UI nad wbudowanym dokumentem OpenAPI

- Status: Accepted
- Data: 2026-09-30

## Kontekst
Od .NET 9 szablon `webapi` nie zawiera już Swaggera (Swashbuckle). Dokument OpenAPI generuje wbudowany pakiet `Microsoft.AspNetCore.OpenApi` (`/openapi/v1.json`), ale bez UI do klikania.

## Rozważane opcje
1. **Swashbuckle.AspNetCore.SwaggerUI (tylko UI)** wskazujący na `/openapi/v1.json`: znany interfejs Swagger, bez generatora Swashbuckle.
2. Scalar (`Scalar.AspNetCore`): nowocześniejsze UI, promowane w dokumentacji MS.
3. Tylko plik `.http` w VS Code / Visual Studio.

## Decyzja
Opcja 1, tylko w środowisku Development: `http://localhost:5033/swagger`. Plik `WordWise.Api.http` zostaje jako alternatywa.

## Konsekwencje
- Jeden generator dokumentu (Microsoft), UI łatwe do podmiany na Scalar.
- W produkcji UI i dokument są wyłączone.

## Do nauki
- OpenAPI w ASP.NET Core: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/overview
- Użycie wygenerowanego dokumentu (Swagger UI / Scalar): https://learn.microsoft.com/aspnet/core/fundamentals/openapi/using-openapi-documents
- Pliki `.http`: https://learn.microsoft.com/aspnet/core/test/http-files
