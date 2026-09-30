# Dziennik nauki

Po każdym zadaniu Claude dopisuje wpis w tym formacie (najnowszy na górze). Uzupełnij „Moimi słowami”, bo to jest Twój explain-back.

```
## RRRR-MM-DD: <spec/task>, tryb [solo|learn|gen]
- Koncepcje: <nazwy, np. Options pattern, scoped lifetime>
- Docs: <linki learn.microsoft.com>
- Moimi słowami: <2–3 zdania, pisze autor>
- Pytanie kontrolne: <pytanie do /quiz-me>
- Kurs fCC: <powiązany moduł albo „poza kursem”>
```

---

## 2026-09-30: Setup repo, tryb [gen]
- Koncepcje: solution `.slnx`, `Directory.Build.props`, Central Package Management, analyzery Roslyn (`TreatWarningsAsErrors`), `WebApplicationFactory`, Microsoft.Testing.Platform.
- Docs:
  - https://learn.microsoft.com/visualstudio/msbuild/customize-by-directory
  - https://learn.microsoft.com/nuget/consume-packages/central-package-management
  - https://learn.microsoft.com/aspnet/core/test/integration-tests
- Ciekawostka: analyzer **CA1707** zablokował nazwę testu z podkreśleniami. Wyłączyliśmy go tylko dla `tests/**` w `.editorconfig`, bo w testach konwencja `Method_Scenario_Expected` jest powszechna.
- Moimi słowami: _…_
- Pytanie kontrolne: Czym różni się `Directory.Build.props` od `Directory.Packages.props` i dlaczego wersji pakietu nie ma w `.csproj`?
- Kurs fCC: poza kursem (tooling .NET).
