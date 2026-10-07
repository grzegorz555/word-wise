# Learning journal

After each task Claude adds an entry in this format (newest on top). Fill in "In my own words" — that is your explain-back.

```
## YYYY-MM-DD: <spec/task>, mode [solo|learn|gen]
- Concepts: <names, e.g. Options pattern, scoped lifetime>
- Docs: <learn.microsoft.com links>
- In my own words: <2–3 sentences, written by the author>
- Check question: <question for /quiz-me>
- fCC course: <related module or "beyond the course">
```

---

## 2026-10-07: Spec 001 / T2, `Concept` and `Word` entities + `IEntityTypeConfiguration`, mode [learn]

- Concepts: entity vs navigation vs foreign key (FK is a column, navigations exist only in C#), required one-to-many (non-nullable FK → required → cascade delete by convention), `= null!` for a reference navigation filled by EF, Fluent API (`IsRequired`, `HasMaxLength` as a data contract), unique index as a business rule, composite index and the leftmost-prefix rule, EF skips the FK index when an existing index already starts with the FK. One-to-many vs many-to-many (join table, with payload = `UserWordProgress`) vs one-to-one (FK + unique index).
- Docs:
  - https://learn.microsoft.com/ef/core/modeling/relationships/one-to-many#required-one-to-many
  - https://learn.microsoft.com/ef/core/modeling/relationships/many-to-many
  - https://learn.microsoft.com/ef/core/modeling/indexes#composite-index
  - https://learn.microsoft.com/ef/core/modeling/relationships/conventions#indexes
  - https://learn.microsoft.com/ef/core/saving/cascade-delete
  - https://learn.microsoft.com/ef/core/miscellaneous/nullable-reference-types
- Fun fact: `dotnet build` passed, but `dotnet format --verify-no-changes` failed on `FINALNEWLINE` (missing newline at end of file), which CI would catch.
- In my own words: Adding models (entities) for the Concept and Word as a preparation to keep in the database
- Check question: The unique index is `(ConceptId, Language, Text)`. Why does it not speed up `WHERE Language = 'tr'`, and why will the migration have no separate `IX_Words_ConceptId`?
- fCC course: beyond the course (EF Core modeling, relationships, indexes); types, `int?` and properties are from the course.

## 2026-10-05: Spec 001 / T1, EF Core + Npgsql + `WordWiseDbContext`, mode [gen]

- Concepts: `DbContext` as a unit of work, `AddDbContext` = scoped lifetime (one context per HTTP request), `DbContextOptions<T>` passed through a primary constructor, configuration keys (`ConnectionStrings:WordWise`, `GetConnectionString`, env var `ConnectionStrings__WordWise`), `ApplyConfigurationsFromAssembly`, Central Package Management and pinning a transitive package (MSB3277), local .NET tools (`dotnet-tools.json`, `dotnet tool restore`).
- Docs:
  - https://learn.microsoft.com/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core
  - https://learn.microsoft.com/dotnet/core/extensions/dependency-injection/service-lifetimes#scoped
  - https://learn.microsoft.com/ef/core/miscellaneous/connection-strings#aspnet-core
  - https://learn.microsoft.com/ef/core/modeling/#grouping-configuration
  - https://learn.microsoft.com/dotnet/core/tools/local-tools-how-to-use
- Fun fact: Npgsql 10.0.3 depends on EF Core 10.0.4, `Design` on 10.0.12. The API built fine, but the test project saw both versions (MSB3277), so `Microsoft.EntityFrameworkCore.Relational` is pinned in `Directory.Packages.props`.
- In my own words: Setting the DB Context, Using EF Core Framework and definition of connection with Database by PSQL standing locally on the Docker container
- Check question: Why must a singleton service never take `WordWiseDbContext` in its constructor, and what do you use instead (hint: T5 `IHostedService`)?
- fCC course: beyond the course (DI, EF Core, configuration).

## 2026-10-05: Language switch styling from Figma, mode [learn]

- Concepts: design tokens as CSS custom properties, Tailwind v4 CSS-variable shorthand (`bg-(--token)`), data attribute variants (`data-[state=on]:`), stacking variants, conflicts decided by utility order in the generated CSS (not in `class`), overriding helm variant classes, non-text contrast.
- Docs:
  - https://tailwindcss.com/docs/hover-focus-and-other-states#data-attributes
  - https://www.w3.org/WAI/WCAG22/Understanding/non-text-contrast.html
- In my own words: Adjusting language switch using Tailwind & template optimalization
- Check question: Why does `data-[state=on]:bg-(--primary)` style only the active item without any `[class.x]` binding in the template?
- fCC course: beyond the course (frontend).

## 2026-10-04: Language switch with spartan Toggle Group, mode [learn]

- Concepts: helm as owned code (shadcn-style copy, not a library), host directives and their inputs (`[aria-label]` instead of `[attr.aria-label]`), `as const` and literal types, `(typeof X)[number]` (indexed access type), type predicate (`value is LanguageCode`) to narrow `unknown`, `toUpperCase()` for identifiers vs `toLocaleUpperCase('tr')` for text, `it.each` argument spreading, testing a pure function without TestBed.
- Docs:
  - https://www.spartan.ng/components/toggle-group
  - https://angular.dev/guide/directives/directive-composition-api
  - https://www.typescriptlang.org/docs/handbook/release-notes/typescript-3-4.html#const-assertions
  - https://www.typescriptlang.org/docs/handbook/2/narrowing.html#using-type-predicates
  - https://vitest.dev/api/#test-each
- Fun fact: removing `as const` made the build pass, because `LanguageCode` silently became `string` and the type guard stopped guarding anything. A green build is not proof that types work.
- In my own words: _…_
- Check question: Why does `it.each([['pl', 'en', 'tr']])` run only one test, and what does `value` hold in it?
- fCC course: beyond the course (TypeScript, frontend testing).

## 2026-10-03: Learning roadmap for full-stack delivery + spartan MCP, mode [gen]

- Concepts: MCP server as a documentation source, supply-chain risk of `npx -y` without a version (pinning), decision drill (criteria → options → trade-offs → "revisit when"), learning roadmap tied to project milestones.
- Docs:
  - https://www.spartan.ng/documentation/mcp
  - https://learn.microsoft.com/azure/well-architected/pillars
  - https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying
- In my own words: _…_
- Check question: Why does `.mcp.json` pin `@spartan-ng/mcp@1.5.0` instead of `npx -y @spartan-ng/mcp`, and when should we bump it?
- fCC course: beyond the course (delivery).

## 2026-10-02: OpenAPI document at build time + generated Angular client, mode [gen]

- Concepts: API contract as the source of truth, build-time OpenAPI generation (MSBuild properties, `PrivateAssets`), code generation from a contract, "generated code is never edited", CI drift check (`git diff --exit-code`), Observables vs Promises for HTTP.
- Docs:
  - https://learn.microsoft.com/aspnet/core/fundamentals/openapi/aspnetcore-openapi#generate-openapi-documents-at-build-time
  - https://github.com/cyclosproject/ng-openapi-gen
- Fun fact: build-time generation starts the app's `Program.cs` with a mock server, so anything that needs real infrastructure (the database in spec 001) must be skipped in that mode.
- In my own words: _…_
- Check question: You add a new property to a response DTO in C#. Which two files change after `npm run api`, and what does CI do if you commit only the C# change?
- fCC course: beyond the course (.NET tooling, MSBuild).

## 2026-10-01: spartan/ui setup + WordWise theme, mode [gen]

- Concepts: headless primitives (brain) vs copied, styled components (helm), CSS custom properties as design tokens, `@theme inline`, OKLCH colors, WCAG contrast ratio (relative luminance), self-hosted fonts and `unicode-range`, npm `overrides` for transitive vulnerabilities, TypeScript path aliases.
- Docs:
  - https://www.spartan.ng/documentation/installation
  - https://www.spartan.ng/documentation/theming
  - https://www.w3.org/WAI/WCAG22/Understanding/non-text-contrast.html
  - https://docs.npmjs.com/cli/v11/configuring-npm/package-json#overrides
- Fun fact: the input border color was far too light (#E4E4E7 in Figma, #D4D4D8 in the first code draft: at most 1.48:1 on white) contrast; WCAG 1.4.11 needs 3:1 for component boundaries, so it became #8A8A93 (3.42:1). `npm audit fix` made things worse (downgraded nx, 15 findings); scoped `overrides` fixed it (0 findings).
- In my own words: _…_
- Check question: Why is `@spartan-ng/helm/button` not an npm package, and what does that mean when spartan releases a new version?
- fCC course: beyond the course (frontend).

## 2026-10-01: App header in Tailwind utilities, mode [learn]

- Concepts: spacing scale (`gap-4` = 4 × 0.25rem), variant + utility (`aria-pressed:bg-zinc-800`), styling by state attribute, Preflight resets (headings, margins), `host` metadata instead of `:host`, Prettier plugins for class sorting and wrapping.
- Docs:
  - https://tailwindcss.com/docs/hover-focus-and-other-states
  - https://tailwindcss.com/docs/preflight
  - https://angular.dev/guide/components/host-elements
- Fun fact: `aria-pressed:zinc-800` silently generated nothing (a variant needs a full utility, e.g. `bg-zinc-800`). `prettier-plugin-tailwindcss` moved the unknown classes to the front of the list, which exposed the typo.
- In my own words: _…_
- Check question: Why is `aria-pressed:bg-zinc-800` better here than `[class.bg-zinc-800]="isActive()"`?
- fCC course: beyond the course (frontend).

## 2026-10-01: Tailwind CSS setup + authentication framework plan, mode [gen]

- Concepts: utility-first CSS, PostCSS plugin, Tailwind v4 CSS-first config (`@import 'tailwindcss'`, `@theme`), class detection by plain-text scanning; cookie auth with Identity API endpoints, 401 instead of redirect for `[ApiController]` (.NET 10), route guard, HTTP interceptor.
- Docs:
  - https://angular.dev/guide/tailwind
  - https://tailwindcss.com/docs/styling-with-utility-classes
  - https://learn.microsoft.com/aspnet/core/security/authentication/identity-api-authorization
  - https://learn.microsoft.com/aspnet/core/security/authentication/api-endpoint-auth
- Fun fact: Tailwind v4 "is not designed to be used with CSS preprocessors like Sass", so global styles moved from `styles.scss` to `styles.css`. Native CSS already has nesting and variables.
- In my own words: _…_
- Check question: Why does `'bg-' + color` not work in Tailwind, while `[class.bg-red-600]="error()"` does?
- fCC course: beyond the course (frontend tooling, ASP.NET Core security).

## 2026-09-30: Repo setup, mode [gen]

- Concepts: `.slnx` solution, `Directory.Build.props`, Central Package Management, Roslyn analyzers (`TreatWarningsAsErrors`), `WebApplicationFactory`, Microsoft.Testing.Platform.
- Docs:
  - https://learn.microsoft.com/visualstudio/msbuild/customize-by-directory
  - https://learn.microsoft.com/nuget/consume-packages/central-package-management
  - https://learn.microsoft.com/aspnet/core/test/integration-tests
- Fun fact: analyzer **CA1707** blocked a test name with underscores. We disabled it only for `tests/**` in `.editorconfig`, because the `Method_Scenario_Expected` convention is common in tests.
- In my own words: _…_
- Check question: How does `Directory.Build.props` differ from `Directory.Packages.props`, and why is the package version not in the `.csproj`?
- fCC course: beyond the course (.NET tooling).
