# ADR-0002: Kontrolery + vertical slices

- Status: Accepted
- Data: 2026-09-30

## Kontekst
Chcę nauczyć się podejścia spotykanego w pracy, ale bez ceremonii wielu projektów-warstw.

## Rozważane opcje
1. Minimal API + slices: najmniej kodu, mniej spotykane w istniejących projektach firmowych.
2. **Kontrolery + slices:** klasyczne MVC, kod pogrupowany wg funkcji.
3. Kontrolery + Clean Architecture (Domain/Application/Infrastructure/Api, MediatR): dużo wzorców, ale też dużo ceremonii na start.

## Decyzja
Opcja 2. Jeden projekt `WordWise.Api`, foldery `Features/<Feature>/` z kontrolerem, DTO i logiką przypadku użycia. Bez MediatR: handler to zwykła klasa rejestrowana w DI.

## Konsekwencje
- Zmiana funkcji dotyka jednego folderu.
- Jeśli logika urośnie, można wydzielić projekt `WordWise.Domain` (nowy ADR).
- Dyscyplina: kontroler jest cienki, logika siedzi w klasie feature'a.

## Do nauki
- Kontrolery i routing atrybutowy: https://learn.microsoft.com/aspnet/core/web-api/
- Dependency Injection i lifetimes: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection
- Vertical Slice Architecture (Jimmy Bogard): https://www.jimmybogard.com/vertical-slice-architecture/
