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
dotnet tool restore                     # local tools from dotnet-tools.json (dotnet-ef)
cd src\web; npm ci; cd ..\..
```

## 2. Database
```powershell
docker compose up -d                    # start Postgres in the background
docker compose ps                       # STATUS: healthy
```
The API reads the connection string `ConnectionStrings:WordWise` from `appsettings.Development.json` (local defaults from `compose.yaml`). If you changed the password or port in `.env`, override it without editing the file, with an environment variable in the API terminal (`__` stands for `:` in configuration keys): `$env:ConnectionStrings__WordWise = "Host=localhost;Port=…;Database=wordwise;Username=wordwise;Password=…"`. Docs: https://learn.microsoft.com/aspnet/core/fundamentals/configuration/#environment-variables

Check the connection: `dotnet ef dbcontext info --project src/api/WordWise.Api` → `Provider name: Npgsql…`, `Database name: wordwise`. Create or update the tables: `dotnet ef database update --project src/api/WordWise.Api` (applies all migrations from `src/api/WordWise.Api/Migrations/`; run it after every pull that adds a migration). Docs: https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying#command-line-tools

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
- **`The container name "/wordwise-postgres" is already in use`:** the container was created before `compose.yaml` had `name: wordwise`, so Compose named the project after the folder (e.g. a worktree). Remove the old container once with `docker rm -f wordwise-postgres` and run `docker compose up -d` again; from then on every worktree shares the `wordwise` project. Docs: https://docs.docker.com/compose/how-tos/project-name/
- **Port in use (5033/4200):** change `applicationUrl` in `Properties/launchSettings.json` or use `npm start -- --port 4201`.
- **`npm ci` warns about install-scripts:** `npm install-scripts approve esbuild` (npm 11 blocks install scripts by default).
- **HTTPS certificate** (`https` profile): `dotnet dev-certs https --trust`.
