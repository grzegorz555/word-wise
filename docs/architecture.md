# Architektura

Lekki arc42 + diagramy C4 (https://c4model.com). Decyzje szczegółowe są w `adr/`.

## Kontekst (C4 poziom 1)
```mermaid
flowchart LR
  user([Użytkownik]) -->|przeglądarka| ww[WordWise]
```
Brak systemów zewnętrznych w MVP.

## Kontenery (C4 poziom 2)
```mermaid
flowchart LR
  user([Użytkownik]) --> web[Angular SPA<br/>src/web]
  web -->|HTTP JSON /api, cookie auth| api[ASP.NET Core API<br/>src/api/WordWise.Api]
  api -->|EF Core / Npgsql| db[(PostgreSQL)]
```
W dev Angular (`:4200`) przekazuje `/api` przez proxy do API (`:5033`). Dzięki temu SPA i API są dla przeglądarki jednym originem (ADR-0004).

## Backend: vertical slices (ADR-0002)
Kod zorganizowany wg funkcji (`Features/Words`, `Features/Study`, `Features/Account`), a nie wg warstw technicznych. Każdy slice zawiera kontroler, DTO i logikę przypadku użycia. Wspólne są tylko `Domain/` (encje) i `Infrastructure/` (DbContext).

## Model danych (ADR-0005)
```mermaid
erDiagram
  Concept ||--o{ Word : "ma formy"
  Word ||--o{ UserWordProgress : "postęp"
  AppUser ||--o{ UserWordProgress : "postęp"
  Concept {
    int Id
    string Key "np. house, right_direction"
    string PartOfSpeech
  }
  Word {
    int Id
    int ConceptId
    string Language "en | pl | tr"
    string Text
    int FrequencyRank "nullable; ranking w danym języku"
  }
  UserWordProgress {
    string UserId
    int WordId "słowo w języku docelowym"
    int TimesSeen
    int TimesKnown
    datetime LastSeenAt
  }
  AppUser {
    string Id "IdentityUser"
    string UiLanguage
    string BaseLanguage
    string TargetLanguage
  }
```
- Unikalność: `Word(ConceptId, Language, Text)`, `Concept(Key)`, `UserWordProgress(UserId, WordId)`.
- Sesja nauki (bazowy B → docelowy T): słowa `Language = T` z rankiem, sortowane po ranku lub losowo; tłumaczenia to słowa `Language = B` o tym samym `ConceptId`.
- Postęp jest per słowo docelowe, wspólny dla wszystkich języków bazowych.

## Jakość i przekroje
- Błędy API: `ProblemDetails` (RFC 9457).
- Kontrakt: OpenAPI (`/openapi/v1.json`) jako źródło prawdy dla DTO w TS.
- i18n UI: Transloco (ADR-0006). Treść (słowa) przychodzi z API.
