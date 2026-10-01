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
