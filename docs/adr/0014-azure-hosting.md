# ADR-0014: Azure hosting: Container Apps, SPA served by the API, production database decided later

- Status: Accepted (compute and SPA); production database: open, decide before the "CD to staging" milestone
- Date: 2026-10-08

## Context
WordWise needs a home on Azure (backlog: deployment, one milestone per spec). The API is ASP.NET Core 10, the SPA is a static Angular build, the database is PostgreSQL 18. Traffic is tiny (learning and portfolio), so cost matters. Hard constraint from ADR-0004 and `security.md`: the SPA and the API share **one origin** (HttpOnly cookie, antiforgery, CORS off), and `security.md` asks for a CSP with nonces. The API sorts TR/PL text with `CultureInfo` (ADR-0006), so it needs ICU at runtime.

## Decision criteria
1. Cost at hobby scale. 2. Fit with ADR-0004 (one origin). 3. Learning value for the delivery roadmap (§5 CI/CD, §7 security, §8 operations). 4. Simplicity and operations effort. 5. Reversibility.

## Considered options
**Compute for the API**
1. **Azure Container Apps (Consumption)**: serverless containers, scale to zero, a monthly free grant, revisions with traffic splitting for rollback. Needs a Dockerfile and a registry; cold start after idle.
2. **App Service (Linux, B1)**: classic PaaS, code deploy without Docker, always on; no scale to zero, deployment slots only from the Standard tier.
3. **A VM with Docker Compose**: cheapest in the first year, but patching, TLS, backups and monitoring are all ours; little overlap with the roadmap.

**Where the SPA lives**
1. **Served by the API** (`wwwroot`, static files + fallback to `index.html`): one origin by construction, one artifact to deploy and roll back, `index.html` can carry a per-request CSP nonce.
2. **Static Web Apps Standard + linked backend** (`/api` proxied to the API): CDN and preview environments, but a paid plan, and cookies/XSRF through the proxy would need a spike. (Static Web Apps Free with a separate API needs CORS and breaks ADR-0004.)
3. **A separate nginx container** (SPA + reverse proxy for `/api`): one origin through the proxy, but two images, an `nginx.conf`, and one more proxy hop for forwarded headers.

**Production database** (not decided yet): Azure Database for PostgreSQL flexible server (Burstable B1ms; managed backups and point-in-time restore; free for 12 months on an Azure free account), Postgres in a container (no backups, stateless platform: not for real data), or a serverless Postgres outside Azure.

## Decision
- **Compute: Azure Container Apps (Consumption plan)**, chosen by the author; it matches Claude's recommendation.
- **SPA: served by the API**, chosen by the author after the drill (first idea: an nginx container).
- **Production database: decided later.** The first milestone (container image) does not need it. Decide before "CD to staging", together with the Azure subscription type (it decides whether the free database year applies).

## Consequences
- One container image for the API + SPA: a multi-stage build (Node for `ng build`, the .NET SDK for publish), `UseStaticFiles` + `MapFallbackToFile("index.html")`, `Cache-Control: no-cache` for `index.html` and long caching for hashed assets.
- Requirements for every deploy behind the Container Apps ingress, to be covered in the milestone specs:
  - **Forwarded headers** before HTTPS redirection and auth, otherwise redirect loops and cookies without `Secure`.
  - **Persistent Data Protection keys**, otherwise every restart, scale-to-zero or new revision signs everybody out and invalidates XSRF tokens.
  - **ICU in the image**: a Debian/Ubuntu `aspnet` base image, not Alpine or chiseled without ICU (globalization-invariant mode makes `CultureInfo.GetCultureInfo("tr")` throw).
  - **Cold start**: health probes on `/health` and enough startup time; `minReplicas = 1` if the first request after idle is too slow.
- Open roadmap decisions that stay open: Dockerfile vs SDK container publish, container registry (ACR vs GitHub Container Registry), migrations in production.

## Revisit when
- Compute: cold starts are unacceptable even with `minReplicas = 1`, or slot-style swaps with simple operations are needed (App Service Standard).
- SPA: a CDN or an independent SPA release cadence is needed, or static files measurably load the API.
- Database: before the "CD to staging" milestone (this ADR gets an update or a new ADR).

## Learning resources
- Container Apps billing (free grant, scale to zero): https://learn.microsoft.com/azure/container-apps/billing
- Container Apps revisions (rollback, traffic splitting): https://learn.microsoft.com/azure/container-apps/revisions
- Cold start in Container Apps: https://learn.microsoft.com/azure/container-apps/cold-start
- App Service plans (the alternative): https://learn.microsoft.com/azure/app-service/overview-hosting-plans
- Static Web Apps APIs and linked backends: https://learn.microsoft.com/azure/static-web-apps/apis-overview
- Host name preservation and forwarded headers behind a proxy: https://learn.microsoft.com/azure/architecture/best-practices/host-name-preservation
- Data Protection keys when scaling out: https://learn.microsoft.com/aspnet/core/host-and-deploy/scaling-aspnet-apps/scaling-aspnet-apps
- .NET container images (ICU, chiseled, Alpine): https://learn.microsoft.com/dotnet/core/docker/container-images
- Culture creation in globalization-invariant mode: https://learn.microsoft.com/dotnet/core/compatibility/globalization/6.0/culture-creation-invariant-mode
- Azure Database for PostgreSQL on a free account: https://learn.microsoft.com/azure/postgresql/configure-maintain/how-to-deploy-on-azure-free-account
