# ADR-0012: Generating the Angular API client from OpenAPI (ng-openapi-gen)

- Status: Accepted
- Date: 2026-10-01

## Context
The API publishes its contract as OpenAPI 3.1 (`Microsoft.AspNetCore.OpenApi`, `/openapi/v1.json`, ADR-0009). So far the rule was "TS DTOs must match the contract", kept by hand. Hand-written DTOs drift from the API without anyone noticing. We want models and HTTP calls generated from the contract, using Angular's `HttpClient` so the interceptors from the auth plan (401 → sign-in) still apply.

## Considered options
1. **ng-openapi-gen**: built for Angular. Generates model interfaces, one function per operation and optional `@Injectable` services on `HttpClient`. Supports OpenAPI 3.0 and 3.1, Angular 16+, standalone + `provideHttpClient`.
2. openapi-typescript: types only, services written by hand. Lightest, but half of the drift problem remains.
3. NSwag / openapi-generator: mature, but large generated client classes; openapi-generator needs Java. Less idiomatic for Angular 22.

## Decision
Option 1: **ng-openapi-gen**.
- The OpenAPI document is produced **at build time** by `Microsoft.Extensions.ApiDescription.Server` (`OpenApiGenerateDocumentsOnBuild`, `OpenApiDocumentsDirectory` → `src/web/openapi/`), so generating the client does not need a running API.
- `src/web/ng-openapi-gen.json` → output `src/web/src/app/core/api/`, script `npm run api:gen`.
- **Both the OpenAPI JSON and the generated code are committed**: contract changes show up in PR diffs.
- CI: `dotnet build` → `npm run api:gen` → `git diff --exit-code`, so the contract and the client cannot drift.

## Consequences
- Generated files are never edited by hand. Feature services in `features/<feature>/data-access/` wrap the generated functions.
- Build-time generation runs `Program.cs` with a mock server. Code that needs real infrastructure (the database from spec 001) must be guarded, as described in the docs below.
- The quality of the client depends on the quality of the contract: `[ProducesResponseType]`, clear DTO names, XML doc comments.
- Replaces the backlog item "Generating a TS client from OpenAPI".

## Learning resources
- Build-time OpenAPI documents: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/aspnetcore-openapi#generate-openapi-documents-at-build-time
- OpenAPI in ASP.NET Core: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/overview
- ng-openapi-gen: https://github.com/cyclosproject/ng-openapi-gen
- Angular HttpClient: https://angular.dev/guide/http
