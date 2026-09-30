# How to run WordWise locally

## Requirements
| Tool | Version | Check |
|---|---|---|
| .NET SDK | 10.0.2xx (from `global.json`) | `dotnet --list-sdks` |
| Node.js | 26.x | `node --version` |
| Docker Desktop | any recent | `docker --version` |

## 1. First run (once)
```powershell
cd C:\Repozytorium\WordWise\word-wise
git config --local core.hooksPath .git/hooks   # isolate from global git hooks
copy .env.example .env                  # local Postgres password/port (file is not committed)
dotnet restore WordWise.slnx
cd src\web; npm ci; cd ..\..
```

## 2. Database
```powershell
docker compose up -d                    # start Postgres in the background
docker compose ps                       # STATUS: healthy
```
At this stage the API doesn't connect to the database yet; the connection arrives in spec 001 / T1. Postgres can already be running.

## 3. API (terminal 1)
```powershell
dotnet run --project src/api/WordWise.Api --launch-profile http
```
Check:
- http://localhost:5033/health → `Healthy`
- **Swagger UI:** http://localhost:5033/swagger
- OpenAPI document: http://localhost:5033/openapi/v1.json
- Alternative: open `src/api/WordWise.Api/WordWise.Api.http` in VS Code (REST Client extension) or Visual Studio and click "Send Request".

Hot reload: `dotnet watch --project src/api/WordWise.Api` instead of `dotnet run`.

## 4. Frontend (terminal 2)
```powershell
cd src\web
npm start                               # http://localhost:4200
```
Switch PL / EN / TR in the top-right corner. `/api/*` and `/health` requests are proxied to the API (`proxy.conf.json`).

## 5. Tests and formatting
```powershell
dotnet test --solution WordWise.slnx
dotnet format WordWise.slnx --verify-no-changes
cd src\web; npx ng test --watch=false; npx prettier --check "src/**/*.{ts,html,scss}"
```

## Stopping
`Ctrl+C` in the terminals, then `docker compose down` (data stays in the `wordwise-pgdata` volume; `docker compose down -v` removes it).

## Troubleshooting
- **Port in use (5033/4200):** change `applicationUrl` in `Properties/launchSettings.json` or use `npm start -- --port 4201`.
- **`npm ci` warns about install-scripts:** `npm install-scripts approve esbuild` (npm 11 blocks install scripts by default).
- **HTTPS certificate** (`https` profile): `dotnet dev-certs https --trust`.
