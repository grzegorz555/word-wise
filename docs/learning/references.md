# Documentation bookmarks

This is where I point you when something needs deeper understanding. ⭐ = start here.

## C# (language)
- ⭐ **C# Guide** (Microsoft Learn): https://learn.microsoft.com/dotnet/csharp/
- ⭐ **Tour of C#**: https://learn.microsoft.com/dotnet/csharp/tour-of-csharp/
- The course you're taking: https://www.freecodecamp.org/learn/foundational-c-sharp-with-microsoft/
- Coding conventions: https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Naming: https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/identifier-names
- What's new in C#: https://learn.microsoft.com/dotnet/csharp/whats-new/
- Async/await: https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/
- LINQ: https://learn.microsoft.com/dotnet/csharp/linq/
- Practice: Exercism C# track: https://exercism.org/tracks/csharp

## .NET (platform)
- ⭐ **.NET docs**: https://learn.microsoft.com/dotnet/
- Framework Design Guidelines: https://learn.microsoft.com/dotnet/standard/design-guidelines/
- Strings and culture (important for TR!): https://learn.microsoft.com/dotnet/standard/base-types/best-practices-strings
- Code analysis (CAxxxx rules): https://learn.microsoft.com/dotnet/fundamentals/code-analysis/overview
- Unit testing (best practices): https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices
- Reference source code: https://github.com/dotnet/eShop

## ASP.NET Core (API)
- ⭐ **ASP.NET Core fundamentals**: https://learn.microsoft.com/aspnet/core/fundamentals/
- ⭐ **Web API with controllers**: https://learn.microsoft.com/aspnet/core/web-api/
- Dependency Injection: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection
- Middleware (pipeline): https://learn.microsoft.com/aspnet/core/fundamentals/middleware
- Configuration and the Options pattern: https://learn.microsoft.com/aspnet/core/fundamentals/configuration/
- OpenAPI: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/overview
- OpenAPI at build time: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/aspnetcore-openapi#generate-openapi-documents-at-build-time
- ng-openapi-gen (Angular client from OpenAPI): https://github.com/cyclosproject/ng-openapi-gen
- Identity for SPAs: https://learn.microsoft.com/aspnet/core/security/authentication/identity-api-authorization
- API endpoints and cookie auth (401/403 instead of redirects, .NET 10): https://learn.microsoft.com/aspnet/core/security/authentication/api-endpoint-auth
- Integration tests: https://learn.microsoft.com/aspnet/core/test/integration-tests
- Model validation (`[Required]`, `IValidatableObject`, automatic 400): https://learn.microsoft.com/aspnet/core/mvc/models/validation
- Model binding (`[FromQuery]`, `[BindRequired]`, complex types): https://learn.microsoft.com/aspnet/core/mvc/models/model-binding
- OpenAPI metadata (enums, `operationId`, required): https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata
- Hosted services and scoped services in the background: https://learn.microsoft.com/aspnet/core/fundamentals/host/hosted-services

## EF Core (database)
- ⭐ **EF Core**: https://learn.microsoft.com/ef/core/
- Relationships: https://learn.microsoft.com/ef/core/modeling/relationships
- Migrations: https://learn.microsoft.com/ef/core/managing-schemas/migrations/
- Querying and tracking: https://learn.microsoft.com/ef/core/querying/tracking
- ⭐ **Client vs server evaluation** (what runs in SQL, what in .NET): https://learn.microsoft.com/ef/core/querying/client-eval
- Loading related data and projections: https://learn.microsoft.com/ef/core/querying/related-data/
- Indexes (composite, unique): https://learn.microsoft.com/ef/core/modeling/indexes
- Collations and case sensitivity: https://learn.microsoft.com/ef/core/miscellaneous/collations-and-case-sensitivity
- Data seeding (`UseSeeding`, custom initialization, `HasData`): https://learn.microsoft.com/ef/core/modeling/data-seeding
- Npgsql (PostgreSQL): https://www.npgsql.org/efcore/

## Angular
- ⭐ **angular.dev** (official docs): https://angular.dev/overview
- ⭐ **Style guide**: https://angular.dev/style-guide
- Signals: https://angular.dev/guide/signals
- Zoneless: https://angular.dev/guide/zoneless
- HttpClient: https://angular.dev/guide/http
- Testing: https://angular.dev/guide/testing
- Transloco (i18n): https://jsverse.gitbook.io/transloco
- Route guards: https://angular.dev/guide/routing/route-guards
- HTTP interceptors: https://angular.dev/guide/http/interceptors

## Tailwind CSS (styling)
- ⭐ **Styling with utility classes** (the core idea): https://tailwindcss.com/docs/styling-with-utility-classes
- Tailwind in Angular (setup): https://angular.dev/guide/tailwind
- Hover, focus and other states (variants): https://tailwindcss.com/docs/hover-focus-and-other-states
- Responsive design (mobile first): https://tailwindcss.com/docs/responsive-design
- Theme variables (`@theme`): https://tailwindcss.com/docs/theme
- Class detection (why no dynamic class names): https://tailwindcss.com/docs/detecting-classes-in-source-files

## UI kit and design (spartan/ui, Figma)
- ⭐ **spartan/ui** (brain vs helm): https://www.spartan.ng/documentation/introduction
- spartan theming (CSS variables): https://www.spartan.ng/documentation/theming
- shadcn/ui Figma kits: https://ui.shadcn.com/docs/figma
- Angular CDK (a11y, overlays): https://material.angular.dev/cdk/categories
- spartan MCP server (docs and components for Claude): https://www.spartan.ng/documentation/mcp

## Fundamentals: computer, network, debugging (see `roadmap.md` §0)
- ⭐ **Debugging a .NET app (VS Code / Visual Studio)**: breakpoints, conditional breakpoints, stepping: https://learn.microsoft.com/dotnet/core/tutorials/debug-console-app
- ⭐ **Processes and threads** (program vs process vs thread): https://learn.microsoft.com/windows/win32/procthread/about-processes-and-threads
- How .NET runs code (C# → IL → JIT): https://learn.microsoft.com/dotnet/standard/managed-execution-process
- Memory and garbage collection (managed heap, stack roots, generations): https://learn.microsoft.com/dotnet/standard/garbage-collection/fundamentals
- async/await explained with a breakfast: https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/
- ASP.NET Core middleware pipeline (the server half of a request): https://learn.microsoft.com/aspnet/core/fundamentals/middleware/
- OSI model, layer by layer (Windows networking view): https://learn.microsoft.com/windows-hardware/drivers/network/windows-network-architecture-and-the-osi-model
- Windows Firewall profiles (why a café network must be "Public"): https://learn.microsoft.com/windows/security/operating-system-security/network-security/windows-firewall/
- HTTPS and HSTS in ASP.NET Core: https://learn.microsoft.com/aspnet/core/security/enforcing-ssl
- MDN, an overview of HTTP (not checked via MCP): https://developer.mozilla.org/docs/Web/HTTP/Guides/Overview
- MDN, the JavaScript event loop (not checked via MCP): https://developer.mozilla.org/docs/Web/JavaScript/Reference/Execution_model
- Chrome DevTools docs (Sources, Network; not checked via MCP): https://developer.chrome.com/docs/devtools

## Delivery and operations (see `roadmap.md`)
Architecture
- ⭐ **Azure Architecture Center: architecture styles**: https://learn.microsoft.com/azure/architecture/guide/architecture-styles/
- ⭐ **Well-Architected Framework pillars**: https://learn.microsoft.com/azure/well-architected/pillars
- Cloud design patterns: https://learn.microsoft.com/azure/architecture/patterns/
- The Twelve-Factor App: https://12factor.net

Testing
- The Practical Test Pyramid (Martin Fowler): https://martinfowler.com/articles/practical-test-pyramid.html
- Testcontainers for .NET: https://testcontainers.com/guides/getting-started-with-testcontainers-for-dotnet/
- Playwright: https://playwright.dev/docs/intro · accessibility testing (axe): https://playwright.dev/docs/accessibility-testing
- k6 (load testing): https://grafana.com/docs/k6/latest/

Containers and Azure
- Containerize a .NET app (Dockerfile): https://learn.microsoft.com/dotnet/core/docker/build-container
- Containerize with `dotnet publish` (no Dockerfile): https://learn.microsoft.com/dotnet/core/containers/sdk-publish
- ⭐ **.NET on Azure Container Apps**: https://learn.microsoft.com/azure/container-apps/dotnet-overview
- Container Apps revisions, traffic splitting, blue-green: https://learn.microsoft.com/azure/container-apps/revisions · https://learn.microsoft.com/azure/container-apps/blue-green-deployment
- Azure Database for PostgreSQL flexible server: https://learn.microsoft.com/azure/postgresql/overview
- Bicep (infrastructure as code): https://learn.microsoft.com/azure/azure-resource-manager/bicep/overview · learning path: https://learn.microsoft.com/training/paths/fundamentals-bicep/
- Bicep vs Terraform: https://learn.microsoft.com/azure/developer/terraform/comparing-terraform-and-bicep

CI/CD and releases
- GitHub Actions OIDC (no long-lived secrets): https://docs.github.com/en/actions/security-for-github-actions/security-hardening-your-deployments/about-security-hardening-with-openid-connect
- Deploy to App Service with GitHub Actions (OIDC options explained): https://learn.microsoft.com/azure/app-service/deploy-github-actions
- ⭐ **EF Core: applying migrations (bundles, scripts, trade-offs)**: https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying
- Semantic Versioning: https://semver.org
- release-please (changelog and versions from Conventional Commits): https://github.com/googleapis/release-please
- Trunk-based development: https://trunkbaseddevelopment.com
- Feature toggles (Martin Fowler): https://martinfowler.com/articles/feature-toggles.html
- DORA metrics: https://dora.dev

Supply chain security
- Dependabot: https://docs.github.com/en/code-security/dependabot
- CodeQL code scanning: https://docs.github.com/en/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql

Operations
- ⭐ **.NET observability with OpenTelemetry**: https://learn.microsoft.com/dotnet/core/diagnostics/observability-with-otel
- Health checks in ASP.NET Core: https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks
- Google SRE book (SLO, incidents, postmortems): https://sre.google/sre-book/table-of-contents/

## Architecture and methodologies
- ADR: https://adr.github.io
- C4 model: https://c4model.com
- Diátaxis (how to write documentation): https://diataxis.fr
- Conventional Commits: https://www.conventionalcommits.org
- GitHub Spec Kit (spec-driven development): https://github.com/github/spec-kit
- Claude Code (best practices): https://docs.anthropic.com/en/docs/claude-code
