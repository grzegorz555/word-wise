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
