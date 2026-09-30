# Jak uruchomić WordWise lokalnie

## Wymagania
| Narzędzie | Wersja | Sprawdź |
|---|---|---|
| .NET SDK | 10.0.2xx (z `global.json`) | `dotnet --list-sdks` |
| Node.js | 26.x | `node --version` |
| Docker Desktop | dowolna aktualna | `docker --version` |

## 1. Pierwsze uruchomienie (raz)
```powershell
cd C:\Repozytorium\WordWise\word-wise
git config --local core.hooksPath .git/hooks   # izolacja od globalnych hooków gita
copy .env.example .env                  # lokalne hasło/port do Postgresa (plik jest poza repo)
dotnet restore WordWise.slnx
cd src\web; npm ci; cd ..\..
```

## 2. Baza danych
```powershell
docker compose up -d                    # start Postgresa w tle
docker compose ps                       # STATUS: healthy
```
Na tym etapie API jeszcze nie łączy się z bazą, bo połączenie dojdzie w spec 001 / T1. Postgres może już działać.

## 3. API (terminal 1)
```powershell
dotnet run --project src/api/WordWise.Api --launch-profile http
```
Sprawdź:
- http://localhost:5033/health → `Healthy`
- **Swagger UI:** http://localhost:5033/swagger
- Dokument OpenAPI: http://localhost:5033/openapi/v1.json
- Alternatywa: otwórz `src/api/WordWise.Api/WordWise.Api.http` w VS Code (rozszerzenie REST Client) lub Visual Studio i kliknij „Send Request”.

Hot reload: `dotnet watch --project src/api/WordWise.Api` zamiast `dotnet run`.

## 4. Frontend (terminal 2)
```powershell
cd src\web
npm start                               # http://localhost:4200
```
Przełącz PL / EN / TR w prawym górnym rogu. Żądania `/api/*` i `/health` idą przez proxy do API (`proxy.conf.json`).

## 5. Testy i formatowanie
```powershell
dotnet test --solution WordWise.slnx
dotnet format WordWise.slnx --verify-no-changes
cd src\web; npx ng test --watch=false; npx prettier --check "src/**/*.{ts,html,scss}"
```

## Zatrzymanie
`Ctrl+C` w terminalach, potem `docker compose down` (dane zostają w wolumenie `wordwise-pgdata`; `docker compose down -v` je usuwa).

## Problemy
- **Port zajęty (5033/4200):** zmień `applicationUrl` w `Properties/launchSettings.json` albo `npm start -- --port 4201`.
- **`npm ci` ostrzega o install-scripts:** `npm install-scripts approve esbuild` (npm 11 domyślnie blokuje skrypty instalacyjne).
- **Certyfikat HTTPS** (profil `https`): `dotnet dev-certs https --trust`.
